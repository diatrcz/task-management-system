namespace BOBA.Server.Services.Interfaces
{
    public interface ITaskFlowService
    {
        Task<string> GetEditRoleId(string taskTypeId, string currentStateId);
    }
}
