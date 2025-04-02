using BOBA.Server.Data;
using BOBA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

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

        [HttpGet("taskflow")]
        public async Task<IActionResult> GetTaskFlow([FromQuery] string currentStateId, [FromQuery] string taskTypeId)
        {
            var taskflow = await _context.TaskFlows
                     .FirstOrDefaultAsync(tf => tf.CurrentStateId == currentStateId &&
                                          tf.TaskTypeId == taskTypeId);
            if (taskflow == null)
            {
                return BadRequest("TaskFlow not found.");
            }

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



        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskType = await _context.TaskTypes
                                         .FirstOrDefaultAsync(tt => tt.Id == request.TaskTypeId);
            if (taskType == null)
            {
                return BadRequest("TaskType not found.");
            }

            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var creator = _context.Users.Where(u => u.Id == creatorId).FirstOrDefault();

            var starterState = await _context.TaskStates
                                             .Where(s => s.Id == starterId)
                                             .FirstOrDefaultAsync(); // Get a single TaskState

            if (starterState == null)
            {
                return BadRequest("Starter state not found.");
            }

            var task = new BOBA.Server.Data.Task
            {
                Id = Guid.NewGuid().ToString(),
                TaskTypeId = request.TaskTypeId,
                TaskType = taskType,
                CreatorId = creatorId,
                Creator = creator,
                CurrentStateId = starterId,
                CurrentState = starterState,
                AssigneeId = creatorId,
                Assignee = creator,
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

            //var nextStateChoices = JsonSerializer.Deserialize<List<NextStateItem>>(taskflow.NextStateJson) ?? new List<NextStateItem>();
            

            var nextStateItem = taskflow.NextState.Single(ns => ns.ChoiceId == request.ChoiceId);


            //var nextState = await _context.TaskStates
            //                              .SingleAsync(ts => ts.Id == nextStateItem.NextStateId);

            task.CurrentStateId = nextStateItem.NextStateId;
            //task.CurrentState = nextState;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPost("move-task-fr")]
        public async Task<IActionResult> MoveTaskFr([FromBody] MoveTaskRequest2 request) {
            var task = await _context.Tasks
                                     .Include(t => t.CurrentState)
                                     .FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var nextState = await _context.TaskStates
                                          .FirstOrDefaultAsync(ts => ts.Id == request.NextStateId);

            if (nextState == null)
            {
                return BadRequest("Next state not found.");
            }

            task.CurrentStateId = nextState.Id;
            task.CurrentState = nextState;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        public class MoveTaskRequest
        {
            public string ChoiceId { get; set; }
            public string TaskId { get; set; }
        }

        public class MoveTaskRequest2
        {
            public string ChoiceId { get; set; }
            public string TaskId { get; set; }
            public string NextStateId { get; set; }
        }


    }

}
