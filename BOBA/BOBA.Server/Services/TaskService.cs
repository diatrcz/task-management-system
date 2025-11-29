using BOBA.Server.Data;
using BOBA.Server.Models;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace BOBA.Server.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly ITaskFlowService _taskFlowService;
    private const string starterId = "1";

    public TaskService(ApplicationDbContext context, ITaskFlowService taskFlowService)
    {
        _context = context;
        _taskFlowService = taskFlowService;
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
            .Include(t => t.Creator)
            .SingleAsync();

       // problem is that in the end states team accidentally set to null which results i n the 500 server error
       //var team = await _context.Teams.Where(t => t.Id == task.TeamId).SingleAsync();

        var taskdto = new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = (task.Assignee != null ? $"{task.Assignee.FirstName} {task.Assignee.LastName}" : null),
            TeamId = task.TeamId,
            //Team = team.Name,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        };

        return taskdto;
    }

    public async Task<List<TaskSummaryDto>> GetUserTasks(string userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.AssigneeId == userId)
            .Include(t => t.CurrentState)
            .Include(t => t.Assignee)
            .Include(t => t.Creator)
            .Include(t => t.TaskType)
            .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto 
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = task.Assignee.FirstName + " " + task.Assignee.LastName,
            TeamId = task.TeamId,
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
            .Include(t => t.Assignee)
            .Include(t => t.Creator)
            .Include(t => t.TaskType)
            .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = task.Assignee.FirstName + " " + task.Assignee.LastName,
            TeamId = task.TeamId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<string> CreateTask(CreateTaskRequest request, string creatorId)
    {
        var taskType = await _context.TaskTypes.SingleAsync(tt => tt.Id == request.TaskTypeId);
        var starterState = await _context.TaskStates.SingleAsync(s => s.Id == starterId);

        if (taskType.StarterTeamId == request.TeamId) 
        {
            var task = new BOBA.Server.Data.implementation.Task
            {
                Id = Guid.NewGuid().ToString(),
                TaskTypeId = request.TaskTypeId,
                CreatorId = creatorId,
                CreatorTeamId = request.TeamId,
                CurrentStateId = starterId,
                AssigneeId = creatorId,
                TeamId = taskType.StarterTeamId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task.Id;
        }

        return null;
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
        task.TeamId = nextStateItem.EditRoleId;

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task.Id;
    }

    public async Task<string> AssignTask(string taskId, string userId) 
    {
        var task = await _context.Tasks
            .Include(t => t.Assignee)
            .Where(t => t.Id == taskId)
            .FirstAsync();

        task.AssigneeId = userId;

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task.Id;
    }

    public async Task<List<TaskSummaryDto>> GetClosedTasksByTeamId(string team_id)
    {
        var tasks = await _context.Tasks
             .Where(t => (t.CreatorTeamId == team_id || t.TeamId == team_id) && t.CurrentState.IsFinal == true) 
             .Include(t => t.CurrentState)
             .Include(t => t.Creator)
             .Include(t => t.TaskType)
             .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = null,
            TeamId = task.TeamId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<List<TaskSummaryDto>> GetUnassignedTasksByTeamId(string team_id)
    {
        var tasks = await _context.Tasks
             .Where(t => t.TeamId == team_id && t.CurrentState.IsFinal == false && t.AssigneeId == null)
             .Include(t => t.CurrentState)
             .Include(t => t.Creator)
             .Include(t => t.Creator)
             .Include(t => t.TaskType)
             .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = null,
            TeamId = task.TeamId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<List<TaskSummaryDto>> GetAssignedTasksForUserByTeamId(string team_id, string user_id)
    {
        var tasks = await _context.Tasks
             .Where(t => t.TeamId == team_id && t.CurrentState.IsFinal == false && t.AssigneeId == user_id)
             .Include(t => t.CurrentState)
             .Include(t => t.Assignee)
             .Include(t => t.Creator)
             .Include(t => t.TaskType)
             .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = task.Assignee.FirstName + " " + task.Assignee.LastName,
            TeamId = task.TeamId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<List<TaskSummaryDto>> GetExternalTasksByTeamId(string team_id)
    {
        var tasks = await _context.Tasks
            .Where(t => t.CreatorTeamId == team_id && t.CurrentState.IsFinal == false)
            .Include(t => t.CurrentState)
            .Include(t => t.Assignee)
            .Include(t => t.Creator)
            .Include(t => t.TaskType)
            .ToListAsync();

        var taskDtos = tasks.Select(task => new TaskSummaryDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskType.Name,
            CreatorName = task.Creator.FirstName + " " + task.Creator.LastName,
            CurrentStateId = task.CurrentStateId,
            CurrentStateName = task.CurrentState.Name,
            CurrentStateIsFinal = task.CurrentState?.IsFinal ?? false,
            AssigneeName = (task.Assignee != null ? $"{task.Assignee.FirstName} {task.Assignee.LastName}" : null),
            TeamId = task.TeamId,
            CreatedAt = task.CreatedAt.ToString("o"),
            UpdatedAt = task.UpdatedAt.ToString("o")
        }).ToList();

        return taskDtos;
    }

    public async Task<Dictionary<string, int>> GetTasksCount(string teamId, string userId)
    {
        var counts = await _context.Tasks
            .Where(t => t.TeamId == teamId || t.CreatorTeamId == teamId)
            .GroupBy(t => 1)
            .Select(g => new
            {
                MyTasks = g.Count(t => !t.CurrentState.IsFinal && t.AssigneeId == userId && t.TeamId == teamId),
                UnassignedTasks = g.Count(t => !t.CurrentState.IsFinal && t.AssigneeId == null && t.TeamId == teamId),
                ExternalTasks = g.Count(t => !t.CurrentState.IsFinal && t.CreatorTeamId == teamId && t.TeamId != teamId),
                ClosedTasks = g.Count(t => t.CurrentState.IsFinal && (t.TeamId == teamId || t.CreatorTeamId == teamId))
            })
            .FirstOrDefaultAsync();


        return new Dictionary<string, int>
        {
            ["my-tasks"] = counts?.MyTasks ?? 0,
            ["unassigned-tasks"] = counts?.UnassignedTasks ?? 0,
            ["external-tasks"] = counts?.ExternalTasks ?? 0,
            ["closed-tasks"] = counts?.ClosedTasks ?? 0
        };
    }

}
