using BOBA.Server.Data.implementation;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Security.Claims;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet("task/fields")]
        public async Task<ActionResult<List<TaskFieldDto>>> GetTaskFieldsByName([FromQuery] List<string> fieldNames)
        {
            var taskFields = await _formService.GetTaskTypesByName(fieldNames);

            return Ok(taskFields);
        }

        [HttpPost("form/{task_id}/fields")]
        public async Task<ActionResult<List<string>>> SaveFormFields([FromRoute] string task_id, [FromBody] List<FormFieldDto> formFields)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var savedIds = await _formService.SaveFormFields(formFields, userId);
                return Ok(savedIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("task/{task_id}/fields")]
        public async Task<ActionResult<List<FormFieldDto>>> GetSavedFieldsForTask([FromRoute] string task_id, [FromQuery] List<string> fieldIds)
        {
            var savedFields = await _formService.GetSavedFieldsForTask(fieldIds, task_id);

            return Ok(savedFields);
        }
    }
}
