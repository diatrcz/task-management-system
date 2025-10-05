using BOBA.Server.Data.implementation;
using BOBA.Server.Models.Dto;

namespace BOBA.Server.Services.Interfaces;

public interface IFormService
{
    Task<List<TaskFieldDto>> GetTaskTypesByName(List<string> fieldNames);
    Task<List<string>> SaveFormFields(List<FormFieldDto> formFieldDtos, string userId);
    Task<Dictionary<string, FormFieldDto>> GetSavedFieldsForTask(List<string> fieldIds, string taskId);
}
