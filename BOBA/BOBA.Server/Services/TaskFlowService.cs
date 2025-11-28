using BOBA.Server.Data;
using BOBA.Server.Data.model;
using BOBA.Server.Models;
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
                ReadOnlyRole = taskflow.ReadOnlyRole.Select(team => team.Id).ToList(),
                FormFields = taskflow.FormField.Select(ff => new FormJsonDto
                {
                    Layout = new LayoutDto
                    {
                        Type = ff.Layout.Type,
                        Columns = ff.Layout.Columns,
                        GapClasses = ff.Layout.GapClasses
                    },
                    Fields = ff.Fields.Select(f => new FieldDto
                    {
                        FieldId = f.FieldId,
                        Required = f.Required,
                        Disabled = f.Disabled,
                        Rows = f.Rows,
                        StyleClasses = new StyleClassesDto
                        {
                            Label = f.StyleClasses.Label,
                            Input = f.StyleClasses.Input
                        }
                    }).ToList()
                }).ToList()
            };

            return taskFlowDto;
        }

        public async Task<List<ChoiceSummaryDto>> GetChoices(List<string> ids)
        {
            var choiceDtos = new List<ChoiceSummaryDto>();
            foreach (var id in ids)
            {
                var choice = await _context.Choices.FindAsync(id);
                if (choice != null)
                {
                    var choiceDto = new ChoiceSummaryDto
                    {
                        Id = choice.Id,
                        Name = choice.Name
                    };

                    choiceDtos.Add(choiceDto);
                }
            }

            return choiceDtos;
        }

        public async Task<TaskStateDto> GetTaskStateNameById(string stateId)
        {
            var taskState = await _context.TaskStates
                .Where(s => s.Id == stateId)
                .FirstAsync();

            return new TaskStateDto { Id = taskState.Id, Name = taskState.Name };
        }
    }
}
