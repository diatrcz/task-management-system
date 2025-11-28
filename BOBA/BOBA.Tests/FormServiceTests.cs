using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BOBA.Tests;

public class FormServiceAdditionalTests
{
    private ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    #region GetTaskTypesByName Tests

    [Fact]
    public async System.Threading.Tasks.Task GetTaskTypesByName_ReturnsEmptyList_WhenEmptyInput()
    {
        using var context = CreateInMemoryDbContext();
        var service = new FormService(context);

        var result = await service.GetTaskTypesByName(new List<string>());

        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskTypesByName_ReturnsFieldsInOrder()
    {
        using var context = CreateInMemoryDbContext();

        var field1 = new TaskField { Id = "f1", Name = "Alpha", Type = "text", Label = "Alpha Field" };
        var field2 = new TaskField { Id = "f2", Name = "Beta", Type = "text", Label = "Beta Field" };
        var field3 = new TaskField { Id = "f3", Name = "Gamma", Type = "text", Label = "Gamma Field" };

        context.TaskFields.AddRange(field1, field2, field3);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        var result = await service.GetTaskTypesByName(new List<string> { "Gamma", "Alpha", "Beta" });

        Assert.Equal(3, result.Count);
        Assert.Equal("Gamma", result[0].Name);
        Assert.Equal("Alpha", result[1].Name);
        Assert.Equal("Beta", result[2].Name);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskTypesByName_MapsAllFieldProperties()
    {
        using var context = CreateInMemoryDbContext();

        var field = new TaskField
        {
            Id = "f1",
            Name = "Email",
            Type = "email",
            Label = "Email Address",
            Placeholder = "user@example.com",
            Validation = @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ValidationErrorMessage = "Invalid email format",
            Options = "option1,option2"
        };

        context.TaskFields.Add(field);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        var result = await service.GetTaskTypesByName(new List<string> { "Email" });

        Assert.Single(result);
        var dto = result[0];
        Assert.Equal("f1", dto.Id);
        Assert.Equal("Email", dto.Name);
        Assert.Equal("email", dto.Type);
        Assert.Equal("Email Address", dto.Label);
        Assert.Equal("user@example.com", dto.Placeholder);
        Assert.Equal(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", dto.Validation);
        Assert.Equal("Invalid email format", dto.ValidationErrorMessage);
        Assert.Equal("option1,option2", dto.Options);
    }

    #endregion

    #region SaveFormFields Tests

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_ReturnsEmptyList_WhenEmptyInput()
    {
        using var context = CreateInMemoryDbContext();
        var service = new FormService(context);

        var result = await service.SaveFormFields(new List<FormFieldDto>(), "user1");

        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_PassesValidation_WhenValueMatchesPattern()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField
        {
            Id = "tf1",
            Name = "Email",
            Type = "text",
            Label = "Email",
            Validation = @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ValidationErrorMessage = "Invalid email."
        };
        context.TaskFields.Add(tf);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Email", Value = "valid@example.com" };

        var result = await service.SaveFormFields(new List<FormFieldDto> { dto }, "u1");

        Assert.Single(result);
        var saved = await context.FormFields.FirstAsync();
        Assert.Equal("valid@example.com", saved.Value);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_SkipsValidation_WhenValidationIsEmpty()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField
        {
            Id = "tf1",
            Name = "Comment",
            Type = "text",
            Label = "Comment",
            Validation = ""
        };
        context.TaskFields.Add(tf);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Comment", Value = "any value works" };

        var result = await service.SaveFormFields(new List<FormFieldDto> { dto }, "u1");

        Assert.Single(result);
        var saved = await context.FormFields.FirstAsync();
        Assert.Equal("any value works", saved.Value);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_SkipsValidation_WhenValidationIsNull()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField
        {
            Id = "tf1",
            Name = "Comment",
            Type = "text",
            Label = "Comment",
            Validation = null
        };
        context.TaskFields.Add(tf);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Comment", Value = "any value works" };

        var result = await service.SaveFormFields(new List<FormFieldDto> { dto }, "u1");

        Assert.Single(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_HandlesMultipleFields()
    {
        using var context = CreateInMemoryDbContext();

        var tf1 = new TaskField { Id = "tf1", Name = "Title", Type = "text", Label = "Title" };
        var tf2 = new TaskField { Id = "tf2", Name = "Description", Type = "text", Label = "Description" };
        var tf3 = new TaskField { Id = "tf3", Name = "Priority", Type = "text", Label = "Priority" };

        context.TaskFields.AddRange(tf1, tf2, tf3);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dtos = new List<FormFieldDto>
        {
            new FormFieldDto { TaskId = "t1", ModelId = "Title", Value = "Bug Report" },
            new FormFieldDto { TaskId = "t1", ModelId = "Description", Value = "System crash" },
            new FormFieldDto { TaskId = "t1", ModelId = "Priority", Value = "High" }
        };

        var result = await service.SaveFormFields(dtos, "user1");

        Assert.Equal(3, result.Count);
        var allFields = await context.FormFields.ToListAsync();
        Assert.Equal(3, allFields.Count);
        Assert.Contains(allFields, f => f.ModelId == "tf1" && f.Value == "Bug Report");
        Assert.Contains(allFields, f => f.ModelId == "tf2" && f.Value == "System crash");
        Assert.Contains(allFields, f => f.ModelId == "tf3" && f.Value == "High");
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_DoesNotUpdate_WhenValueAndModifierUnchanged()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField { Id = "tf1", Name = "Title", Type = "text", Label = "Title" };
        var existing = new FormField
        {
            Id = "oldid",
            TaskId = "t1",
            ModelId = "tf1",
            Value = "SameValue",
            ModifierId = "user1"
        };

        context.TaskFields.Add(tf);
        context.FormFields.Add(existing);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Title", Value = "SameValue" };

        var result = await service.SaveFormFields(new List<FormFieldDto> { dto }, "user1");

        var updated = await context.FormFields.FindAsync("oldid");
        Assert.Equal("SameValue", updated!.Value);
        Assert.Equal("user1", updated.ModifierId);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_UpdatesModifier_WhenOnlyModifierChanges()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField { Id = "tf1", Name = "Title", Type = "text", Label = "Title" };
        var existing = new FormField
        {
            Id = "oldid",
            TaskId = "t1",
            ModelId = "tf1",
            Value = "SameValue",
            ModifierId = "user1"
        };

        context.TaskFields.Add(tf);
        context.FormFields.Add(existing);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Title", Value = "SameValue" };

        var result = await service.SaveFormFields(new List<FormFieldDto> { dto }, "user2");

        var updated = await context.FormFields.FindAsync("oldid");
        Assert.Equal("SameValue", updated!.Value);
        Assert.Equal("user2", updated.ModifierId);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_RemovesAllDuplicates_WhenMultipleExist()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField { Id = "tf1", Name = "Title", Type = "text", Label = "Title" };
        context.TaskFields.Add(tf);

        var f1 = new FormField { Id = "1", TaskId = "t1", ModelId = "tf1", Value = "A", ModifierId = "u1" };
        var f2 = new FormField { Id = "2", TaskId = "t1", ModelId = "tf1", Value = "B", ModifierId = "u1" };
        var f3 = new FormField { Id = "3", TaskId = "t1", ModelId = "tf1", Value = "C", ModifierId = "u1" };
        var f4 = new FormField { Id = "4", TaskId = "t1", ModelId = "tf1", Value = "D", ModifierId = "u1" };

        context.FormFields.AddRange(f1, f2, f3, f4);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Title", Value = "NewVal" };

        await service.SaveFormFields(new List<FormFieldDto> { dto }, "u2");

        var all = await context.FormFields.Where(f => f.TaskId == "t1").ToListAsync();
        Assert.Single(all);
        Assert.Equal("NewVal", all.First().Value);
    }

    [Fact]
    public async System.Threading.Tasks.Task SaveFormFields_PreservesDifferentTaskFields()
    {
        using var context = CreateInMemoryDbContext();

        var tf = new TaskField { Id = "tf1", Name = "Title", Type = "text", Label = "Title" };
        context.TaskFields.Add(tf);

        var otherTaskField = new FormField
        {
            Id = "other",
            TaskId = "t2",
            ModelId = "tf1",
            Value = "Other Task Value",
            ModifierId = "u1"
        };

        context.FormFields.Add(otherTaskField);
        await context.SaveChangesAsync();

        var service = new FormService(context);
        var dto = new FormFieldDto { TaskId = "t1", ModelId = "Title", Value = "Current Task Value" };

        await service.SaveFormFields(new List<FormFieldDto> { dto }, "u2");

        var allFields = await context.FormFields.ToListAsync();
        Assert.Equal(2, allFields.Count);
        Assert.Contains(allFields, f => f.TaskId == "t1" && f.Value == "Current Task Value");
        Assert.Contains(allFields, f => f.TaskId == "t2" && f.Value == "Other Task Value");
    }

    #endregion

    #region GetSavedFieldsForTask Tests

    [Fact]
    public async System.Threading.Tasks.Task GetSavedFieldsForTask_ReturnsEmptyList_WhenNoMatchingFields()
    {
        using var context = CreateInMemoryDbContext();

        var f1 = new FormField { Id = "1", TaskId = "t2", ModelId = "m1", Value = "val1", ModifierId = "u1" };
        context.FormFields.Add(f1);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        var result = await service.GetSavedFieldsForTask(new List<string> { "m1" }, "t1");

        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetSavedFieldsForTask_ReturnsEmptyList_WhenEmptyInput()
    {
        using var context = CreateInMemoryDbContext();
        var service = new FormService(context);

        var result = await service.GetSavedFieldsForTask(new List<string>(), "t1");

        Assert.Empty(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetSavedFieldsForTask_MapsAllProperties()
    {
        using var context = CreateInMemoryDbContext();

        var field = new FormField
        {
            Id = "field123",
            TaskId = "task456",
            ModelId = "model789",
            Value = "testValue",
            ModifierId = "user999"
        };

        context.FormFields.Add(field);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        var result = await service.GetSavedFieldsForTask(new List<string> { "model789" }, "task456");

        Assert.Single(result);
        var dto = result[0];
        Assert.Equal("field123", dto.Id);
        Assert.Equal("task456", dto.TaskId);
        Assert.Equal("model789", dto.ModelId);
        Assert.Equal("testValue", dto.Value);
        Assert.Equal("user999", dto.ModifierId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetSavedFieldsForTask_FiltersCorrectlyByTaskAndModel()
    {
        using var context = CreateInMemoryDbContext();

        var f1 = new FormField { Id = "1", TaskId = "t1", ModelId = "m1", Value = "match", ModifierId = "u1" };
        var f2 = new FormField { Id = "2", TaskId = "t1", ModelId = "m2", Value = "nomatch-wrongmodel", ModifierId = "u1" };
        var f3 = new FormField { Id = "3", TaskId = "t2", ModelId = "m1", Value = "nomatch-wrongtask", ModifierId = "u1" };

        context.FormFields.AddRange(f1, f2, f3);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        var result = await service.GetSavedFieldsForTask(new List<string> { "m1" }, "t1");

        Assert.Single(result);
        Assert.Equal("match", result[0].Value);
        Assert.Equal("m1", result[0].ModelId);
        Assert.Equal("t1", result[0].TaskId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetSavedFieldsForTask_ReturnsMultipleMatchingFields()
    {
        using var context = CreateInMemoryDbContext();

        var f1 = new FormField { Id = "1", TaskId = "t1", ModelId = "m1", Value = "val1", ModifierId = "u1" };
        var f2 = new FormField { Id = "2", TaskId = "t1", ModelId = "m2", Value = "val2", ModifierId = "u2" };
        var f3 = new FormField { Id = "3", TaskId = "t1", ModelId = "m3", Value = "val3", ModifierId = "u3" };

        context.FormFields.AddRange(f1, f2, f3);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        var result = await service.GetSavedFieldsForTask(new List<string> { "m1", "m2", "m3" }, "t1");

        Assert.Equal(3, result.Count);
        Assert.Contains(result, r => r.ModelId == "m1" && r.Value == "val1");
        Assert.Contains(result, r => r.ModelId == "m2" && r.Value == "val2");
        Assert.Contains(result, r => r.ModelId == "m3" && r.Value == "val3");
    }

    [Fact]
    public async System.Threading.Tasks.Task GetSavedFieldsForTask_ThrowsException_WhenDuplicateFieldsExist()
    {
        using var context = CreateInMemoryDbContext();

        var f1 = new FormField { Id = "1", TaskId = "t1", ModelId = "m1", Value = "val1", ModifierId = "u1" };
        var f2 = new FormField { Id = "2", TaskId = "t1", ModelId = "m1", Value = "duplicate", ModifierId = "u1" };

        context.FormFields.AddRange(f1, f2);
        await context.SaveChangesAsync();

        var service = new FormService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.GetSavedFieldsForTask(new List<string> { "m1" }, "t1");
        });
    }

    #endregion
}