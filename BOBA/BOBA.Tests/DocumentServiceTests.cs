using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BOBA.Tests;

public class DocumentServiceTests
{
    private ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    #region GetDocTypesByTaskId Tests

    [Fact]
    public async System.Threading.Tasks.Task GetDocTypesByTaskId_ReturnsMatchingDocTypes()
    {
        using var context = CreateInMemoryDbContext();

        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var task = new BOBA.Server.Data.implementation.Task { Id = "t1", TaskTypeId = "type1", CreatorTeamId = "team1", CreatorId = "u1", CurrentStateId = "cs1"};
        var docType1 = new TaskDocType { Id = "d1", Name = "Report", TaskTypes = new List<TaskType> { taskType }, Description = "desc", Type = "type1"};
        var docType2 = new TaskDocType { Id = "d2", Name = "Log", TaskTypes = new List<TaskType> { taskType }, Description = "desc", Type = "type2" };

        context.TaskTypes.Add(taskType);
        context.Tasks.Add(task);
        context.TaskDocTypes.AddRange(docType1, docType2);
        await context.SaveChangesAsync();

        var service = new DocumentService(context);

        var result = await service.GetDocTypesByTaskId("t1");

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == "d1");
        Assert.Contains(result, r => r.Id == "d2");
    }

    [Fact]
    public async System.Threading.Tasks.Task GetDocTypesByTaskId_ThrowsIfTaskNotFound()
    {
        using var context = CreateInMemoryDbContext();
        var service = new DocumentService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetDocTypesByTaskId("nonexistent"));
    }

    #endregion

    #region SaveDocumentsForTask

    [Fact]
    public async System.Threading.Tasks.Task SaveDocumentsForTask_CreatesDocumentsAndReturnsIds()
    {
        using var context = CreateInMemoryDbContext();
        var service = new DocumentService(context);

        var docs = new List<FormDocumentDto>
        {
            new FormDocumentDto { Id = "doc1", DocTypeId = "typeA" },
            new FormDocumentDto { Id = "doc2", DocTypeId = "typeB" }
        };
        var files = new List<FormDocumentFileDto>
        {
            new FormDocumentFileDto { FileName = "file1.txt", ContentType = "text/plain", Data = new byte[] {1,2,3} },
            new FormDocumentFileDto { FileName = "file2.txt", ContentType = "text/plain", Data = new byte[] {4,5,6} }
        };

        var result = await service.SaveDocumentsForTask(docs, files, "u1", "t1");

        Assert.Equal(2, result.Count);
        var saved = await context.FormDocuments.ToListAsync();
        Assert.Equal(2, saved.Count);
        Assert.All(saved, f => Assert.Equal("u1", f.UploaderId));
        Assert.All(saved, f => Assert.Equal("t1", f.TaskId));
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveDocumentsForTask_ThrowsIfMismatchedLengths()
    {
        using var context = CreateInMemoryDbContext();
        var service = new DocumentService(context);

        var docs = new List<FormDocumentDto> { new FormDocumentDto { Id = "1" } };
        var files = new List<FormDocumentFileDto>(); 

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.SaveDocumentsForTask(docs, files, "u1", "t1"));
    }

    #endregion

    #region GetFileById Tests

    [Fact]
    public async System.Threading.Tasks.Task GetFileById_ReturnsExpectedFile()
    {
        using var context = CreateInMemoryDbContext();

        var file = new FormDocument
        {
            Id = "f1",
            DocTypeId = "doc1",
            TaskId = "task1",
            FileName = "file.pdf",
            ContentType = "application/pdf",
            Data = new byte[] { 1, 2, 3 },
            UploadeddAt = DateTime.UtcNow,
            UploaderId = "u1",
            IsDeleted = false
        };
        context.FormDocuments.Add(file);
        await context.SaveChangesAsync();

        var service = new DocumentService(context);

        var result = await service.GetFileById("f1");

        Assert.Equal("file.pdf", result.FileName);
        Assert.Equal("application/pdf", result.ContentType);
        Assert.Equal(new byte[] { 1, 2, 3 }, result.Data);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetFileById_ThrowsIfDeleted()
    {
        using var context = CreateInMemoryDbContext();

        var file = new FormDocument {
            Id = "f1",
            DocTypeId = "doc1",
            TaskId = "task1",
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Data = new byte[] { 0x01, 0x02 },
            UploadeddAt = DateTime.UtcNow,
            UploaderId = "u1",
            IsDeleted = false
        };
        context.FormDocuments.Add(file);
        await context.SaveChangesAsync();

        var service = new DocumentService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetFileById("file1"));
    }

    #endregion

    #region SetDeleteFile Tests

    [Fact]
    public async System.Threading.Tasks.Task SetDeleteFile_MarksFileAsDeleted()
    {
        using var context = CreateInMemoryDbContext();

        var file = new FormDocument {
            Id = "f1",
            DocTypeId = "doc1",
            TaskId = "task1",
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Data = new byte[] { 0x01, 0x02 },
            UploadeddAt = DateTime.UtcNow,
            UploaderId = "u1",
            IsDeleted = false
        };
        context.FormDocuments.Add(file);
        await context.SaveChangesAsync();

        var service = new DocumentService(context);

        var result = await service.SetDeleteFile("f1", "deleter");

        var updated = await context.FormDocuments.FindAsync("f1");
        Assert.True(updated!.IsDeleted);
        Assert.Equal("deleter", updated.UploaderId);
        Assert.Equal("f1", result);
    }

    #endregion

    #region GetDocumentsForTask Tests

    [Fact]
    public async System.Threading.Tasks.Task GetDocumentsForTask_ReturnsDtosWithUploaderNames()
    {
        using var context = CreateInMemoryDbContext();

        var user1 = new User { Id = "u1", FirstName = "Alice", LastName = "Smith" };
        var user2 = new User { Id = "u2", FirstName = "Bob", LastName = "Brown" };
        var doc1 = new FormDocument
        {
            Id = "f1",
            DocTypeId = "doc1",
            TaskId = "task1",
            FileName = "file.pdf",
            ContentType = "application/pdf",
            Data = new byte[] { 1, 2, 3 },
            UploadeddAt = DateTime.UtcNow,
            UploaderId = "u1",
            IsDeleted = false
        };
        var doc2 = new FormDocument
        {
            Id = "f2",
            DocTypeId = "doc2",
            TaskId = "task1",
            FileName = "file.pdf",
            ContentType = "application/pdf",
            Data = new byte[] { 4, 5, 6 },
            UploadeddAt = DateTime.UtcNow,
            UploaderId = "u2",
            IsDeleted = false
        };
        var deleted = new FormDocument
        {
            Id = "f3",
            DocTypeId = "doc3",
            TaskId = "task3",
            FileName = "file.pdf",
            ContentType = "application/pdf",
            Data = new byte[] { 1, 2, 3 },
            UploadeddAt = DateTime.UtcNow,
            UploaderId = "u1",
            IsDeleted = true
        };

        context.Users.AddRange(user1, user2);
        context.FormDocuments.AddRange(doc1, doc2, deleted);
        await context.SaveChangesAsync();

        var service = new DocumentService(context);

        var result = await service.GetDocumentsForTask("task1");

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.UploaderName == "Alice Smith");
        Assert.Contains(result, r => r.UploaderName == "Bob Brown");
        Assert.DoesNotContain(result, r => r.Id == "d3"); // deleted ignored
    }

    #endregion
}
