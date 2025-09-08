using BOBA.Server.Data;
using BOBA.Server.Models.Dto;
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

        public async Task<TaskFlowSummaryDto> GetTaskFlowById(string taskId)
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

        public async Task<string> GetTaskStateNameById(string stateId)
        {
            return await _context.TaskStates
                .Where(s => s.Id == stateId)
                .Select(s => s.Name)
                .FirstAsync();
        }
    }
}
