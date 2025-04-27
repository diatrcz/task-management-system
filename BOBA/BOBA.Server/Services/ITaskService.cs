using BOBA.Server.Data;
using BOBA.Server.Models;

namespace BOBA.Server.Services;

public interface ITaskService
{
    Task<List<TaskType>> GetTaskTypesAsync();
    Task<Data.Task> GetTaskAsync(string taskId);
    Task<TaskFlow> GetTaskFlowAsync(string taskId);
    Task<List<Choice>> GetChoicesAsync(List<string> ids);
    Task<string> GetTaskStateNameAsync(string stateId);
    Task<List<Data.Task>> GetUserTasksAsync(string userId);
    Task<List<Data.Task>> GetClosedTasksAsync();
    Task<Data.Task> CreateTaskAsync(CreateTaskRequest request, string creatorId);
    Task<Data.Task> MoveTaskAsync(MoveTaskRequest request);
}
