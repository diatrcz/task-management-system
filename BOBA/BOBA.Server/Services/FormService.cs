using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BOBA.Server.Services;

public class FormService : IFormService
{
    private readonly ApplicationDbContext _context;

    public FormService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskFieldDto>> GetTaskTypesByName(List<string> fieldNames)
    {
        var taskFields = new List<TaskField>();

        foreach (var name in fieldNames) 
        {
            var taskfield = await _context.TaskFields.SingleAsync(t => t.Name == name);

            if (taskfield != null)
            {
                taskFields.Add(taskfield);
            }
        }

        var taskFieldDtos = taskFields.Select(taskField => new TaskFieldDto
        {
            Id = taskField.Id,
            Name = taskField.Name,
            Type = taskField.Type,
            Label = taskField.Label,
            Placeholder = taskField.Placeholder,
            Validation = taskField.Validation,
            ValidationErrorMessage = taskField.ValidationErrorMessage,
            Options = taskField.Options
        }).ToList();

        return taskFieldDtos;

    }

    public async Task<List<string>> SaveFormFields(List<FormFieldDto> formFieldDtos, string userId)
    {
        var savedIds = new List<string>();

        foreach (var field in formFieldDtos)
        {

            var taskField = await _context.TaskFields.FindAsync(field.ModelId);

            if (!string.IsNullOrWhiteSpace(taskField.Validation))
            {
                if (!Regex.IsMatch(field.Value, taskField.Validation))
                {
                    throw new Exception(
                        $"Field '{taskField.Name}' failed validation: {taskField.ValidationErrorMessage ?? "Invalid value."}"
                    );
                }
            }

            var existing = await _context.FormFields
                .SingleOrDefaultAsync(f => f.TaskId == field.TaskId && f.ModelId == field.ModelId);

            if (existing != null)
            {
                if (existing.Value != field.Value || existing.ModifierId != field.ModifierId)
                {
                    existing.Value = field.Value;
                    existing.ModifierId = userId;
                    _context.FormFields.Update(existing);
                }
            }
            else
            {
                var newField = new FormField
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskId = field.TaskId,
                    ModelId = field.ModelId,
                    Value = field.Value,
                    ModifierId = userId,
                };

                await _context.FormFields.AddAsync(newField);
            }

            savedIds.Add(field.Id); ;
        }

        await _context.SaveChangesAsync();

        return savedIds;
    }

    public async Task<Dictionary<string, FormFieldDto>> GetSavedFieldsForTask(List<string> fieldIds, string taskId)
    {
        var formFields = new Dictionary<string, FormField>();
        foreach (var fieldId in fieldIds)
        {
            var field = await _context.FormFields
                .Where(f => f.TaskId == taskId && f.ModelId == fieldId)
                .SingleAsync();

            formFields.Add(fieldId, field);
        }

        var formFieldDtos = formFields.Select(f => new FormFieldDto
        {
            Id = f.Value.Id,
            ModelId = f.Value.ModelId,
            TaskId = f.Value.TaskId,
            Value = f.Value.Value,
            ModifierId = f.Value.ModifierId
        }).ToDictionary(f => f.ModelId, f => f);

        return formFieldDtos;

    }

}
