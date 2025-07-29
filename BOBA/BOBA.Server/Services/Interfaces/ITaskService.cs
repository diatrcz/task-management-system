using BOBA.Server.Data;
using BOBA.Server.Models;
using BOBA.Server.Models.Dto;

namespace BOBA.Server.Services.Interfaces;

public interface ITaskService
{
    Task<List<TaskTypeDto>> GetTaskTypes();
    Task<TaskSummaryDto> GetTask(string taskId);
    Task<TaskFlowSummaryDto> GetTaskFlow(string taskId);
    Task<List<ChoiceSummaryDto>> GetChoices(List<string> ids);
    Task<string> GetTaskStateName(string stateId);
    Task<List<TaskSummaryDto>> GetUserTasks(string userId);
    Task<List<TaskSummaryDto>> GetClosedTasksByTeamId(string team_id);
    Task<List<TaskSummaryDto>> GetUnassignedTasksByTeamId(string team_id);
    Task<List<TaskSummaryDto>> GetAssignedTasksForUserByTeamId(string team_id, string user_id);
    Task<List<TaskSummaryDto>> GetClosedTasks();
    Task<string> CreateTask(CreateTaskRequest request, string creatorId);
    Task<string> MoveTask(MoveTaskRequest request);
}
