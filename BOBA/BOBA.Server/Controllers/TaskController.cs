using Azure.Core;
using BOBA.Server.Data;
using BOBA.Server.Migrations;
using BOBA.Server.Models;
using BOBA.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetTaskTypes()
        {
            var taskTypes = await _taskService.GetTaskTypes();
            if (taskTypes == null || taskTypes.Count == 0)
                return NotFound("No task types found.");

            return Ok(taskTypes);
        }

        [HttpGet("task")]
        public async Task<IActionResult> GetTask([FromQuery] string taskId)
        {
            var task = await _taskService.GetTask(taskId);
            return Ok(task);
        }

        [HttpGet("taskflow")]
        public async Task<IActionResult> GetTaskFLow([FromQuery] string taskId)
        {
            var taskflow = await _taskService.GetTaskFlow(taskId);
            return Ok(taskflow);
        }

        [HttpGet("choices")]
        public async Task<IActionResult> GetChoices([FromQuery] List<string> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("At least one ID must be provided.");

            var choices = await _taskService.GetChoices(ids);
            if (choices.Count == 0)
                return NotFound("No choices found for the provided IDs.");

            return Ok(choices);
        }

        [HttpGet("state-name")]
        public async Task<IActionResult> GetTaskStateName([FromQuery] string stateId)
        {
            var stateName = await _taskService.GetTaskStateName(stateId);
            return Ok(new { stateName });
        }

        [HttpGet("own-tasks")]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tasks = await _taskService.GetUserTasks(userId);
            return Ok(tasks);
        }

        [HttpGet("closed-tasks")]
        public async Task<IActionResult> GetClosedTask()
        {
            var tasks = await _taskService.GetClosedTasks();
            return Ok(tasks);
        }

        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var taskId = await _taskService.CreateTask(request, creatorId);
            return Ok(new { taskId });
        }

        [HttpPost("move-task")]
        public async Task<IActionResult> MoveTask([FromBody] MoveTaskRequest request)
        {
            var taskId = await _taskService.MoveTask(request);
            return Ok(new { taskId });
        }
    }

}
