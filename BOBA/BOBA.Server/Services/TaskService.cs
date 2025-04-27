using BOBA.Server.Data;
using BOBA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BOBA.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private const string starterId = "1";

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskType>> GetTaskTypesAsync()
        {
            return await _context.TaskTypes.ToListAsync();
        }

        public async Task<BOBA.Server.Data.Task> GetTaskAsync(string taskId)
        {
            return await _context.Tasks
                .Include(t => t.CurrentState)
                .Include(t => t.TaskType)
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .SingleAsync(t => t.Id == taskId);
        }

        public async Task<TaskFlow> GetTaskFlowAsync(string taskId)
        {
            var task = await _context.Tasks
                                     .Include(t => t.CurrentState)
                                     .SingleAsync(t => t.Id == taskId);

            return await _context.TaskFlows
                .SingleAsync(tf => tf.CurrentStateId == task.CurrentStateId &&
                                   tf.TaskTypeId == task.TaskTypeId);
        }

        public async Task<List<Choice>> GetChoicesAsync(List<string> ids)
        {
            var choices = new List<Choice>();
            foreach (var id in ids)
            {
                var choice = await _context.Choices.FindAsync(id);
                if (choice != null)
                {
                    choices.Add(choice);
                }
            }
            return choices;
        }

        public async Task<string> GetTaskStateNameAsync(string stateId)
        {
            return await _context.TaskStates
                .Where(s => s.Id == stateId)
                .Select(s => s.Name)
                .FirstAsync();
        }

        public async Task<List<BOBA.Server.Data.Task>> GetUserTasksAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.AssigneeId == userId)
                .Include(t => t.CurrentState)
                .Include(t => t.TaskType)
                .ToListAsync();
        }

        public async Task<List<BOBA.Server.Data.Task>> GetClosedTasksAsync()
        {
            return await _context.Tasks
                .Where(t => t.CurrentState.IsFinal)
                .Include(t => t.CurrentState)
                .Include(t => t.TaskType)
                .ToListAsync();
        }

        public async Task<BOBA.Server.Data.Task> CreateTaskAsync(CreateTaskRequest request, string creatorId)
        {
            var taskType = await _context.TaskTypes.SingleAsync(tt => tt.Id == request.TaskTypeId);
            var starterState = await _context.TaskStates.SingleAsync(s => s.Id == starterId);

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
            return task;
        }

        public async Task<BOBA.Server.Data.Task> MoveTaskAsync(MoveTaskRequest request)
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
            return task;
        }
    }
}
