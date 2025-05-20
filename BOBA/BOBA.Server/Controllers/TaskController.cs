using BOBA.Server.Models;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("tasktypes")]
        public async Task<ActionResult<List<TaskTypeDto>>> GetTaskTypes()
        {
            var taskTypes = await _taskService.GetTaskTypes();
            if (taskTypes == null || taskTypes.Count == 0)
                return NotFound("No task types found.");

            return Ok(taskTypes);
        }

        [HttpGet("task")]
        public async Task<ActionResult<TaskTypeDto>> GetTask([FromQuery] string taskId)
        {
            var task = await _taskService.GetTask(taskId);
            return Ok(task);
        }

        [HttpGet("taskflow")]
        public async Task<ActionResult<TaskFlowSummaryDto>> GetTaskFLow([FromQuery] string taskId)
        {
            var taskflow = await _taskService.GetTaskFlow(taskId);
            return Ok(taskflow);
        }

        [HttpGet("choices")]
        public async Task<ActionResult<List<ChoiceSummaryDto>>> GetChoices([FromQuery] List<string> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("At least one ID must be provided.");

            var choices = await _taskService.GetChoices(ids);
            if (choices.Count == 0)
                return NotFound("No choices found for the provided IDs.");

            return Ok(choices);
        }

        [HttpGet("state-name")]
        public async Task<ActionResult<string>> GetTaskStateName([FromQuery] string stateId)
        {
            var stateName = await _taskService.GetTaskStateName(stateId);
            return Ok(new { stateName });
        }

        [HttpGet("own-tasks")]
        public async Task<ActionResult<List<TaskSummaryDto>>> GetUserTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tasks = await _taskService.GetUserTasks(userId);
            return Ok(tasks);
        }

        [HttpGet("closed-tasks")]
        public async Task<ActionResult<List<TaskSummaryDto>>> GetClosedTask()
        {
            var tasks = await _taskService.GetClosedTasks();
            return Ok(tasks);
        }

        [HttpPost("create-task")]
        public async Task<ActionResult<string>> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var taskId = await _taskService.CreateTask(request, creatorId);
            return Ok(new { taskId });
        }

        [HttpPost("move-task")]
        public async Task<ActionResult<string>> MoveTask([FromBody] MoveTaskRequest request)
        {
            var taskId = await _taskService.MoveTask(request);
            return Ok(new { taskId });
        }
    }

}
