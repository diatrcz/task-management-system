using BOBA.Server.Data;
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

        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask([FromBody] string taskTypeId)
        {
            
            var taskType = await _context.TaskTypes
                                         .FirstOrDefaultAsync(tt => tt.Id == taskTypeId);

            if (taskType == null)
            {
                return BadRequest("TaskType not found.");
            }

            var workflow = await _context.TaskFlows
                                        .Include(w => w.CurrentState)
                                        .FirstOrDefaultAsync(w => w.TaskTypeId == taskTypeId && w.CurrentState.Name == "Planning");

            if (workflow == null)
            {
                return BadRequest("No Workflow found with 'Planning' status.");
            }

            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var creator = _context.Users.Where(u => u.Id == creatorId).FirstOrDefault();

            var task = new BOBA.Server.Data.Task
            {
                Id = Guid.NewGuid().ToString(), 
                TaskTypeId = taskTypeId,
                TaskType = taskType,
                CreatorId = creatorId,
                Creator = creator,
                AssigneeId = creatorId,
                Assignee = creator,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

    }

}
