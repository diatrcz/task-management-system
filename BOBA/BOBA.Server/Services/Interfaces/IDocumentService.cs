using BOBA.Server.Models.Dto;

namespace BOBA.Server.Services.Interfaces;

public interface IDocumentService
{
    Task<List<TaskDocTypeDto>> GetDocTypesByTaskId(string taskTypeId);
    Task<List<string>> SaveDocumentsForTask(List<FormDocumentDto> documentInfos, List<FormDocumentFileDto> files, string userId, string taskId);
    Task<FormDocumentFileDto> GetFileById(string fileId);
    Task<string> SetDeleteFile(string fileId, string userId);
    Task<List<FormDocumentDto>> GetDocumentsForTask(string taskId);


}
