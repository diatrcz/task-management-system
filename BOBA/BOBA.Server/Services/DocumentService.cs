using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BOBA.Server.Services;

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;

    public DocumentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDocTypeDto>> GetDocTypesByTaskId(string taskId)
    {
        var task = await _context.Tasks.Where(t => t.Id == taskId).SingleAsync();
        var doctypes = await _context.TaskDocTypes
            .Where(dt => dt.TaskTypes.Any(tt => tt.Id == task.TaskTypeId))
            .Select(dt => new TaskDocTypeDto
            {
                Id = dt.Id,
                Name = dt.Name,
            })
            .ToListAsync();

        return doctypes; 
    }

    public async Task<List<string>> SaveDocumentsForTask(List<FormDocumentDto> documentInfos, List<FormDocumentFileDto> files, string userId, string taskId)
    {
        if (documentInfos.Count != files.Count)
            throw new ArgumentException("Document info and file lists must have the same length.");

        var savedIds = new List<string>();

        for (int i = 0; i < documentInfos.Count; i++)
        {
            var info = documentInfos[i];
            var file = files[i];

            var document = new FormDocument
            {
                Id = info.Id ?? Guid.NewGuid().ToString(),
                TaskId = taskId,
                DocTypeId = info.DocTypeId,
                FileName = file.FileName,
                ContentType = file.ContentType, 
                Data = file.Data,
                UploadeddAt = DateTime.UtcNow,    
                UploaderId = userId
            };

            _context.FormDocuments.Add(document);
            savedIds.Add(document.Id);
        }

        await _context.SaveChangesAsync();

        return savedIds;
    }

    public async Task<FormDocumentFileDto> GetFileById(string fileId)
    {
        var file = await _context.FormDocuments
            .Where(f => f.Id == fileId && !f.IsDeleted)
            .Select(f => new FormDocumentFileDto
            {
                Id = f.Id,
                FileName = f.FileName,
                ContentType = f.ContentType,  
                Data = f.Data
            })
            .SingleAsync();

        return file;
    }

    public async Task<string> SetDeleteFile(string fileId, string userId)
    {
        var file = await _context.FormDocuments
        .SingleAsync(f => f.Id == fileId);

        file.IsDeleted = true;
        file.UploaderId = userId;

        await _context.SaveChangesAsync();

        return file.Id;
    }

    public async Task<List<FormDocumentDto>> GetDocumentsForTask(string taskId)
    {
        var documents = await _context.FormDocuments
            .Where (f => f.TaskId == taskId && !f.IsDeleted)
            .ToListAsync();

        var userIds = documents.Select(d => d.UploaderId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.FirstName + ' ' + u.LastName);

        var documentDtos = documents.Select(document => new FormDocumentDto 
        {
            Id = document.Id,
            DocTypeId = document.DocTypeId,
            FileName = document.FileName,
            UploadedAt = document.UploadeddAt,
            UploaderName = users.ContainsKey(document.UploaderId) ? users[document.UploaderId] : "Unknown"
        }).ToList();

        return documentDtos;
    }
}
