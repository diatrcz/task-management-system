using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api/document")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("{task_id}/doctypes")]
        public async Task<ActionResult<List<TaskDocTypeDto>>> GetDocTypes([FromRoute] string task_id)
        {
            var docTypes = await _documentService.GetDocTypesByTaskId(task_id);
            return Ok(docTypes);
        } 

        [HttpPost("{task_id}/upload")]
        public async Task<ActionResult<List<string>>> UploadFiles([FromRoute] string task_id, List<IFormFile> files,[FromForm] string docTypeId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var documentDtos = new List<FormDocumentDto>();
            var fileDtos = new List<FormDocumentFileDto>();

            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                string documentId = Guid.NewGuid().ToString();

                documentDtos.Add(new FormDocumentDto
                {
                    Id = documentId,
                    DocTypeId = docTypeId,
                    FileName = file.FileName,
                    UploadedAt = DateTime.UtcNow,
                    UploaderName = userId
                });

                fileDtos.Add(new FormDocumentFileDto
                {
                    Id = documentId,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = ms.ToArray()
                });
            }

            var savedIds = await _documentService.SaveDocumentsForTask(documentDtos, fileDtos, userId, task_id);

            return Ok(savedIds);
        }

        [HttpGet("{task_id}/{file_id}")]
        public async Task<ActionResult> DownloadFile([FromRoute] string task_id, [FromRoute] string file_id)
        {
            var fileDto = await _documentService.GetFileById(file_id);

            if (fileDto == null)
                return NotFound();

            return File(fileDto.Data, fileDto.ContentType, fileDto.FileName);
        }

        [HttpPatch("{task_id}/{file_id}/delete")]
        public async Task<ActionResult<string>> DeleteFile([FromRoute] string task_id, [FromRoute] string file_id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var deletedId = await _documentService.SetDeleteFile(file_id, userId);
            return Ok(deletedId);
        }

        [HttpGet("{task_id}/documents")]
        public async Task<ActionResult<List<FormDocumentDto>>> GetFilesForTask([FromRoute] string task_id)
        {
            var documents = await _documentService.GetDocumentsForTask(task_id);
            return Ok(documents);
        }
    }
}
