using BOBA.Server.Data;
using BOBA.Server.Models;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BOBA.Server.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private const string starterId = "1";

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskTypeDto>> GetTaskTypes()
    {
        var tasktypes = await _context.TaskTypes.ToListAsync();

        var tasktypeDtos = tasktypes.Select(tasktype => new TaskTypeDto 
        {
            Id = tasktype.Id,
            Name = tasktype.Name
        }).ToList();

        return tasktypeDtos;
    }

    public async Task<TaskSummaryDto> GetTask(string taskId)
    {
        var task = await _context.Tasks
            .Where(t => t.Id == taskId)
            .Include(t => t.CurrentState)
            .Include(t => t.TaskType)
            .Include(t => t.Creator)
            .Include(t => t.Assignee)
            .SingleAsync();

        var taskdto = new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorId = task.CreatorId,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeId = task.AssigneeId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        };

        return taskdto;


    }

    public async Task<TaskFlowSummaryDto> GetTaskFlow(string taskId)
    {
        var task = await _context.Tasks
                                 .Where(t => t.Id == taskId)
                                 .Include(t => t.CurrentState)
                                 .SingleAsync();

        var taskflow = await _context.TaskFlows
            .SingleAsync(tf => tf.CurrentStateId == task.CurrentStateId &&
                               tf.TaskTypeId == task.TaskTypeId);

        var taskFlowDto = new TaskFlowSummaryDto
        {
            Id = taskflow.Id,
            NextState = taskflow.NextState.Select(ns => new NextStateDto
            {
                ChoiceId = ns.ChoiceId,
                NextStateId = ns.NextStateId
            }).ToList(),
            EditRoleId = taskflow.EditRoleId,
            ReadOnlyRole = taskflow.ReadOnlyRole.Select(team => team.Id).ToList()
        };

        return taskFlowDto;
    }

    public async Task<List<ChoiceSummaryDto>> GetChoices(List<string> ids)
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

        var choiceDtos = choices.Select(choice => new ChoiceSummaryDto
        {
            Id = choice.Id,
            Name = choice.Name
        }).ToList();

        return choiceDtos;
    }

    public async Task<string> GetTaskStateName(string stateId)
    {
        return await _context.TaskStates
            .Where(s => s.Id == stateId)
            .Select(s => s.Name)
            .FirstAsync();
    }

    public async Task<List<TaskSummaryDto>> GetUserTasks(string userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.AssigneeId == userId)
            .Include(t => t.CurrentState)
            .Include(t => t.TaskType)
            .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto 
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorId = task.CreatorId,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeId = task.AssigneeId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<List<TaskSummaryDto>> GetClosedTasks()
    {
        var tasks =  await _context.Tasks
            .Where(t => t.CurrentState.IsFinal)
            .Include(t => t.CurrentState)
            .Include(t => t.TaskType)
            .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorId = task.CreatorId,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeId = task.AssigneeId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<string> CreateTask(CreateTaskRequest request, string creatorId)
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
        return task.Id;
    }

    public async Task<string> MoveTask(MoveTaskRequest request)
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
        return task.Id;
    }
}
