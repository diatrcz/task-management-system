using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const string starterId = "1";

        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var document = new FormDocument
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskId = "2",
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = ms.ToArray(),
                    UploadeddAt = DateTime.UtcNow,
                    UploaderId = userId
                };

                _context.FormDocuments.Add(document);
                await _context.SaveChangesAsync();

                return Ok("Uploaded");
            }

            return BadRequest("Invalid file");
        }

    }
}
