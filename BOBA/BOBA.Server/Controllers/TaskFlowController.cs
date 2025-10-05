using BOBA.Server.Models.Dto;
using BOBA.Server.Services;
using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TaskFlowController : ControllerBase
    {
        private readonly ITaskFlowService _taskFlowService;

        public TaskFlowController(ITaskFlowService taskFlowService)
        {
            _taskFlowService = taskFlowService;
        }

        [HttpGet("taskflows/{task_id}")]
        public async Task<ActionResult<TaskFlowSummaryDto>> GetTaskFlow([FromRoute] string task_id)
        {
            var taskflow = await _taskFlowService.GetTaskFlowById(task_id);
            return Ok(taskflow);
        }

        [HttpGet("taskflows/choices")]
        public async Task<ActionResult<List<ChoiceSummaryDto>>> GetChoices([FromQuery] List<string> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("At least one ID must be provided.");

            var choices = await _taskFlowService.GetChoices(ids);
            if (choices.Count == 0)
                return NotFound("No choices found for the provided IDs.");

            return Ok(choices);
        }

        [HttpGet("taskflows/states/{state_id}")]
        public async Task<ActionResult<string>> GetTaskStateName([FromRoute] string state_id)
        {
            var stateName = await _taskFlowService.GetTaskStateNameById(state_id);
            return Ok(new { stateName });
        }
    }
}
