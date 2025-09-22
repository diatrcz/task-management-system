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

        [HttpPost("tasks")]
        public async Task<ActionResult<string>> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var taskId = await _taskService.CreateTask(request, creatorId);
            return Ok(taskId);
        }

        [HttpGet("tasks/types")]
        public async Task<ActionResult<List<TaskTypeDto>>> GetAllTaskTypes() 
        {
            var taskTypes = await _taskService.GetTaskTypes();
            if (taskTypes == null || taskTypes.Count == 0)
                return NotFound("No task types found.");

            return Ok(taskTypes);
        }

        [HttpGet("tasks/{task_id}")]
        public async Task<ActionResult<TaskSummaryDto>> GetTaskById([FromRoute] string task_id)
        {
            var task = await _taskService.GetTask(task_id);
            return Ok(task);
        }

        [HttpPatch("tasks/{task_id}")]
        public async Task<ActionResult<string>> UpdateTask(
            [FromRoute] string task_id, 
            [FromBody] MoveTaskRequest request
        ){
            var taskId = await _taskService.MoveTask(request);
            return Ok();
        }

        [HttpGet("tasks/teams/{team_id}/closed")]
        public async Task<ActionResult<List<TaskSummaryDto>>> GetClosedTasksByTeamId([FromRoute] string team_id) 
        {
            var tasks = await _taskService.GetClosedTasksByTeamId(team_id);
            return Ok(tasks);
        }

        [HttpGet("taks/teams/{team_id}/unassigned")]
        public async Task<ActionResult<List<TaskSummaryDto>>> GetUnassignedTasksByTeamId([FromRoute] string team_id)
        {
            var tasks = await _taskService.GetUnassignedTasksByTeamId(team_id);
            return Ok(tasks);
        }

        [HttpGet("tasks/teams/{team_id}/user")]
        public async Task<ActionResult<List<TaskSummaryDto>>> GetAssignedTasksForUserByTeamId([FromRoute] string team_id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var tasks = await _taskService.GetAssignedTasksForUserByTeamId(team_id, userId);
            return Ok(tasks);
        }


    }

}
