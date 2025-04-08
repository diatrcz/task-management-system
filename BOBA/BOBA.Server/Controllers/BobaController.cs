using Azure.Core;
using BOBA.Server.Data;
using BOBA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BOBA.Server.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class BobaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BobaController(ApplicationDbContext context) => _context = context;

        private const string starterId = "1";


        //-------------Get----------------------


        [HttpGet("tasktypes")]
        public async Task<IActionResult> GetTaskTypes()
        {
            var taskTypes = await _context.TaskTypes.ToListAsync();

            if (taskTypes == null || taskTypes.Count == 0)
            {
                return NotFound("No task types found.");
            }

            return Ok(taskTypes);
        }

        [HttpGet("task")]
        public async Task<IActionResult> GetTask([FromQuery] string taskId)
        {
            var task = await _context.Tasks
                                     .Include(t => t.CurrentState)
                                     .Include(t => t.TaskType)
                                     .Include(t => t.Creator)
                                     .Include(t => t.Assignee)
                                     .SingleAsync(t => t.Id == taskId);

            return Ok(task);
        }

        [HttpGet("taskflow")]
        public async Task<IActionResult> GetTaskFLow([FromQuery] string taskId)
        {
            var task = await _context.Tasks
                                     .Include(t => t.CurrentState)
                                     .SingleAsync(t => t.Id == taskId);

            var taskflow = await _context.TaskFlows
                                 .SingleAsync(tf => tf.CurrentStateId == task.CurrentStateId &&
                                                            tf.TaskTypeId == task.TaskTypeId);
            return Ok(taskflow);
        }

        [HttpGet("choices")]
        public async Task<IActionResult> GetChoices([FromQuery] List<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("At least one ID must be provided.");
            }

            List<Choice> choices = new List<Choice>();

            foreach (var id in ids)
            {
                var choice = await _context.Choices.FindAsync(id);
                if (choice != null)
                {
                    choices.Add(choice);
                }
            }

            if (choices.Count == 0)
            {
                return NotFound("No choices found for the provided IDs.");
            }

            return Ok(choices);
        }

        [HttpGet("state-name")]
        public async Task<IActionResult> GetTaskStateName([FromQuery] string stateId)
        {
            var stateName = await _context.TaskStates
                .Where(s => s.Id == stateId)
                .Select(s => s.Name)
                .FirstAsync();

            return Ok(new { stateName } );
        }

        //-------------Post----------------------

        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskType = await _context.TaskTypes
                                         .SingleAsync(tt => tt.Id == request.TaskTypeId);

            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var creator = _context.Users.Where(u => u.Id == creatorId).Single();

            var starterState = await _context.TaskStates
                                             .Where(s => s.Id == starterId)
                                             .SingleAsync(); 

            var task = new BOBA.Server.Data.Task
            {
                Id = Guid.NewGuid().ToString(),
                TaskTypeId = request.TaskTypeId,
                CreatorId = creatorId,
                CurrentStateId = starterId,
                AssigneeId = creatorId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }


        [HttpPost("move-task")]
        public async Task<IActionResult> MoveTask([FromBody] MoveTaskRequest request)
        {
            var task = await _context.Tasks
                                     .Include(t => t.CurrentState)
                                     .SingleAsync(t => t.Id == request.TaskId);

            var taskflow = await _context.TaskFlows
                                 .SingleAsync(tf => tf.CurrentStateId == task.CurrentStateId &&
                                                            tf.TaskTypeId == task.TaskTypeId);
            

            var nextStateItem = taskflow.NextState.Single(ns => ns.ChoiceId == request.ChoiceId);


            task.CurrentStateId = nextStateItem.NextStateId;
            task.UpdatedAt = DateTime.UtcNow;
            task.AssigneeId = null;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

    }

}
