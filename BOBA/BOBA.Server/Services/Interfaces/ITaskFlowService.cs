using BOBA.Server.Models.Dto;

namespace BOBA.Server.Services.Interfaces
{
    public interface ITaskFlowService
    {
        Task<string> GetEditRoleId(string taskTypeId, string currentStateId);
        Task<TaskFlowSummaryDto> GetTaskFlowById(string taskId);
        Task<List<ChoiceSummaryDto>> GetChoices(List<string> ids);
        Task<string> GetTaskStateNameById(string stateId);
    }
}
