using BOBA.Server.Data;
using BOBA.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BOBA.Server.Services
{
    public class TaskFlowService : ITaskFlowService
    {
        private readonly ApplicationDbContext _context;

        public TaskFlowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetEditRoleId(string taskTypeId, string currenStateId)
        {
            var editRoleId = await _context.TaskFlows
                .Where(f => f.TaskTypeId == taskTypeId && f.CurrentStateId == currenStateId)
                .Select(f => f.EditRoleId)
                .SingleAsync();

            return editRoleId;
        }
    }
}
