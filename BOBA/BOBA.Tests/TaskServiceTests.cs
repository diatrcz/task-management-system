using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using BOBA.Server.Models;
using BOBA.Server.Models.Dto;
using BOBA.Server.Services;
using BOBA.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace BOBA.Tests;

public class TaskServiceTests
{
    private ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    #region GetTaskTypes Tests

    [Fact]
    public async System.Threading.Tasks.Task GetTaskTypes_ReturnsAllTaskTypes()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        context.TaskTypes.AddRange(
            new TaskType { Id = "1", Name = "Bug" },
            new TaskType { Id = "2", Name = "Feature" }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTaskTypes();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Name == "Bug");
        Assert.Contains(result, t => t.Name == "Feature");
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskTypes_ReturnsEmptyList_WhenNoTaskTypes()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTaskTypes();

        Assert.Empty(result);
    }

    #endregion

    #region GetTask Tests

    [Fact]
    public async System.Threading.Tasks.Task GetTask_ReturnsExpectedTaskSummaryDto()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var state = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var user = new User { Id = "u1", FirstName = "Alice" };
        var task = new BOBA.Server.Data.implementation.Task
        {
            Id = "t1",
            TaskTypeId = taskType.Id,
            TaskType = taskType,
            CreatorId = user.Id,
            Creator = user,
            CurrentStateId = state.Id,
            CurrentState = state,
            AssigneeId = user.Id,
            Assignee = user,
            TeamId = "team1",
            CreatorTeamId = "team1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(state);
        context.Users.Add(user);
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTask("t1");

        Assert.NotNull(result);
        Assert.Equal("t1", result.Id);
        Assert.Equal("Bug", result.TaskTypeName);
        Assert.Equal("Open", result.CurrentStateName);
        Assert.False(result.CurrentStateIsFinal);
        Assert.Equal("Alice ", result.AssigneeName);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTask_ReturnsFinalState_WhenTaskIsClosed()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var state = new TaskState { Id = "s1", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };
        var task = new BOBA.Server.Data.implementation.Task
        {
            Id = "t1",
            TaskTypeId = taskType.Id,
            TaskType = taskType,
            CreatorId = user.Id,
            Creator = user,
            CurrentStateId = state.Id,
            CurrentState = state,
            AssigneeId = user.Id,
            Assignee = user,
            TeamId = "team1",
            CreatorTeamId = "team1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(state);
        context.Users.Add(user);
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTask("t1");

        Assert.True(result.CurrentStateIsFinal);
        Assert.Equal("Closed", result.CurrentStateName);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTask_ThrowsException_WhenTaskNotFound()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var service = new TaskService(context, mockFlowService.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetTask("nonexistent"));
    }

    #endregion

    #region GetUserTasks Tests

    [Fact]
    public async System.Threading.Tasks.Task GetUserTasks_ReturnsOnlyTasksAssignedToUser()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var state = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var user1 = new User { Id = "u1", FirstName = "Alice" };
        var user2 = new User { Id = "u2", FirstName = "Bob" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(state);
        context.Users.AddRange(user1, user2);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user1.Id,
                Creator = user1,
                CurrentStateId = state.Id,
                CurrentState = state,
                AssigneeId = user1.Id,
                Assignee = user1,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user2.Id,
                Creator = user2,
                CurrentStateId = state.Id,
                CurrentState = state,
                AssigneeId = user2.Id,
                Assignee = user2,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetUserTasks("u1");

        Assert.Single(result);
        Assert.Equal("t1", result[0].Id);
        Assert.Equal("Alice ", result[0].AssigneeName);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetUserTasks_ReturnsEmptyList_WhenNoTasksAssigned()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetUserTasks("u1");

        Assert.Empty(result);
    }

    #endregion

    #region GetClosedTasks Tests

    [Fact]
    public async System.Threading.Tasks.Task GetClosedTasks_ReturnsOnlyFinalTasks()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetClosedTasks();

        Assert.Single(result);
        Assert.Equal("t1", result[0].Id);
        Assert.True(result[0].CurrentStateIsFinal);
    }

    #endregion

    #region CreateTask Tests

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_CreatesTaskSuccessfully_WhenTeamMatches()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug", StarterTeamId = "team1" };
        var starterState = new TaskState { Id = "1", Name = "New", IsFinal = false };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(starterState);
        await context.SaveChangesAsync();

        var service = new TaskService(context, mockFlowService.Object);
        var request = new CreateTaskRequest { TaskTypeId = "type1", TeamId = "team1" };

        var result = await service.CreateTask(request, "u1");

        Assert.NotNull(result);
        var createdTask = await context.Tasks.FindAsync(result);
        Assert.NotNull(createdTask);
        Assert.Equal("type1", createdTask.TaskTypeId);
        Assert.Equal("u1", createdTask.CreatorId);
        Assert.Equal("u1", createdTask.AssigneeId);
        Assert.Equal("1", createdTask.CurrentStateId);
        Assert.Equal("team1", createdTask.TeamId);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_ReturnsNull_WhenTeamDoesNotMatch()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug", StarterTeamId = "team1" };
        var starterState = new TaskState { Id = "1", Name = "New", IsFinal = false };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(starterState);
        await context.SaveChangesAsync();

        var service = new TaskService(context, mockFlowService.Object);
        var request = new CreateTaskRequest { TaskTypeId = "type1", TeamId = "team2" };

        var result = await service.CreateTask(request, "u1");

        Assert.Null(result);
        Assert.Empty(context.Tasks);
    }

    #endregion

    #region MoveTask Tests

    [Fact]
    public async System.Threading.Tasks.Task MoveTask_UpdatesTaskState_Successfully()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var state1 = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var state2 = new TaskState { Id = "s2", Name = "In Progress", IsFinal = false };
        var user = new User { Id = "u1", FirstName = "Alice" };

        var task = new BOBA.Server.Data.implementation.Task
        {
            Id = "t1",
            TaskTypeId = taskType.Id,
            TaskType = taskType,
            CreatorId = user.Id,
            Creator = user,
            CurrentStateId = state1.Id,
            CurrentState = state1,
            AssigneeId = user.Id,
            Assignee = user,
            TeamId = "team1",
            CreatorTeamId = "team1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var taskFlow = new TaskFlow
        {
            Id = "tf1",
            TaskTypeId = "type1",
            CurrentStateId = "s1",
            NextState = new List<NextStateItem>
            {
                new NextStateItem
                {
                    ChoiceId = "choice1",
                    NextStateId = "s2",
                    EditRoleId = "team2"
                }
            }
        };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(state1, state2);
        context.Users.Add(user);
        context.Tasks.Add(task);
        context.TaskFlows.Add(taskFlow);
        await context.SaveChangesAsync();

        var service = new TaskService(context, mockFlowService.Object);
        var request = new MoveTaskRequest { TaskId = "t1", ChoiceId = "choice1" };

        var result = await service.MoveTask(request);

        Assert.Equal("t1", result);
        var updatedTask = await context.Tasks.FindAsync("t1");
        Assert.Equal("s2", updatedTask!.CurrentStateId);
        Assert.Null(updatedTask.AssigneeId);
        Assert.Equal("team2", updatedTask.TeamId);
    }

    #endregion

    #region GetClosedTasksByTeamId Tests

    [Fact]
    public async System.Threading.Tasks.Task GetClosedTasksByTeamId_ReturnsClosedTasksForTeam()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t3",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team2",
                CreatorTeamId = "team2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetClosedTasksByTeamId("team1");

        Assert.Single(result);
        Assert.Equal("t1", result[0].Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetClosedTasksByTeamId_IncludesTasksWhereTeamIsCreator()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(closedState);
        context.Users.Add(user);
        context.Tasks.Add(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team2",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetClosedTasksByTeamId("team1");

        Assert.Single(result);
        Assert.Equal("t1", result[0].Id);
    }

    #endregion

    #region GetUnassignedTasksByTeamId Tests

    [Fact]
    public async System.Threading.Tasks.Task GetUnassignedTasksByTeamId_ReturnsOnlyUnassignedTasks()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(openState);
        context.Users.Add(user);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = null,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetUnassignedTasksByTeamId("team1");

        Assert.Single(result);
        Assert.Equal("t1", result[0].Id);
        Assert.Null(result[0].AssigneeName);
    }

    #endregion

    #region GetAssignedTasksForUserByTeamId Tests

    [Fact]
    public async System.Threading.Tasks.Task GetAssignedTasksForUserByTeamId_ReturnsUserTasksInTeam()
    {
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var user1 = new User { Id = "u1", FirstName = "Alice" };
        var user2 = new User { Id = "u2", FirstName = "Bob" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(openState);
        context.Users.AddRange(user1, user2);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user1.Id,
                Creator = user1,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user1.Id,
                Assignee = user1,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user1.Id,
                Creator = user1,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user2.Id,
                Assignee = user2,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t3",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user1.Id,
                Creator = user1,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user1.Id,
                Assignee = user1,
                TeamId = "team2",
                CreatorTeamId = "team2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetAssignedTasksForUserByTeamId("team1", "u1");

        Assert.Single(result);
        Assert.Equal("t1", result[0].Id);
    }

    #endregion

    #region GetExternalTasksByTeamId Tests

    [Fact]
    public async System.Threading.Tasks.Task GetExternalTasksByTeamId_ReturnsTasksCreatedByTeam()
    {
   
        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t3",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetExternalTasksByTeamId("team1");

        Assert.Single(result);
        Assert.Equal("t2", result[0].Id);
        Assert.False(result[0].CurrentStateIsFinal);
    }

    #endregion

    #region GetTasksCount Tests

    [Fact]
    public async System.Threading.Tasks.Task GetTasksCount_ReturnsCorrectCounts()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);
        context.Tasks.AddRange(

            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = null,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            new BOBA.Server.Data.implementation.Task
            {
                Id = "t3",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team2",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            new BOBA.Server.Data.implementation.Task
            {
                Id = "t4",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTasksCount("team1", "u1");

        Assert.Equal(1, result["my-tasks"]);
        Assert.Equal(1, result["unassigned-tasks"]);
        Assert.Equal(1, result["external-tasks"]);
        Assert.Equal(1, result["closed-tasks"]);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTasksCount_ReturnsZeros_WhenNoTasks()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTasksCount("team1", "u1");

        Assert.Equal(0, result["my-tasks"]);
        Assert.Equal(0, result["unassigned-tasks"]);
        Assert.Equal(0, result["external-tasks"]);
        Assert.Equal(0, result["closed-tasks"]);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTasksCount_CountsMultipleTasks_InEachCategory()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);

        for (int i = 0; i < 3; i++)
        {
            context.Tasks.Add(new BOBA.Server.Data.implementation.Task
            {
                Id = $"my{i}",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        for (int i = 0; i < 2; i++)
        {
            context.Tasks.Add(new BOBA.Server.Data.implementation.Task
            {
                Id = $"unassigned{i}",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = null,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        for (int i = 0; i < 2; i++)
        {
            context.Tasks.Add(new BOBA.Server.Data.implementation.Task
            {
                Id = $"external{i}",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team2",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        for (int i = 0; i < 2; i++)
        {
            context.Tasks.Add(new BOBA.Server.Data.implementation.Task
            {
                Id = $"closed{i}",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetTasksCount("team1", "u1");

        Assert.Equal(3, result["my-tasks"]);
        Assert.Equal(2, result["unassigned-tasks"]);
        Assert.Equal(2, result["external-tasks"]);
        Assert.Equal(2, result["closed-tasks"]);
    }

    #endregion

    #region Edge Cases and Additional Tests

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_SetsCorrectTimestamps()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug", StarterTeamId = "team1" };
        var starterState = new TaskState { Id = "1", Name = "New", IsFinal = false };

        context.TaskTypes.Add(taskType);
        context.TaskStates.Add(starterState);
        await context.SaveChangesAsync();

        var service = new TaskService(context, mockFlowService.Object);
        var request = new CreateTaskRequest { TaskTypeId = "type1", TeamId = "team1" };
        var beforeCreation = DateTime.UtcNow;

        var result = await service.CreateTask(request, "u1");
        var afterCreation = DateTime.UtcNow;

        var createdTask = await context.Tasks.FindAsync(result);
        Assert.InRange(createdTask!.CreatedAt, beforeCreation.AddSeconds(-1), afterCreation.AddSeconds(1));
        Assert.InRange(createdTask!.UpdatedAt, beforeCreation.AddSeconds(-1), afterCreation.AddSeconds(1));
        Assert.True(
            Math.Abs((createdTask.CreatedAt - createdTask.UpdatedAt).TotalMilliseconds) < 1,
            $"CreatedAt and UpdatedAt differ by more than 1ms: {createdTask.CreatedAt:o} vs {createdTask.UpdatedAt:o}"
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task MoveTask_UpdatesTimestamp()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var state1 = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var state2 = new TaskState { Id = "s2", Name = "In Progress", IsFinal = false };
        var user = new User { Id = "u1", FirstName = "Alice" };

        var originalTime = DateTime.UtcNow.AddHours(-1);
        var task = new BOBA.Server.Data.implementation.Task
        {
            Id = "t1",
            TaskTypeId = taskType.Id,
            TaskType = taskType,
            CreatorId = user.Id,
            Creator = user,
            CurrentStateId = state1.Id,
            CurrentState = state1,
            AssigneeId = user.Id,
            Assignee = user,
            TeamId = "team1",
            CreatorTeamId = "team1",
            CreatedAt = originalTime,
            UpdatedAt = originalTime
        };

        var taskFlow = new TaskFlow
        {
            Id = "tf1",
            TaskTypeId = "type1",
            CurrentStateId = "s1",
            NextState = new List<NextStateItem>
            {
                new NextStateItem
                {
                    ChoiceId = "choice1",
                    NextStateId = "s2",
                    EditRoleId = "team2"
                }
            }
        };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(state1, state2);
        context.Users.Add(user);
        context.Tasks.Add(task);
        context.TaskFlows.Add(taskFlow);
        await context.SaveChangesAsync();

        var service = new TaskService(context, mockFlowService.Object);
        var request = new MoveTaskRequest { TaskId = "t1", ChoiceId = "choice1" };
        var beforeMove = DateTime.UtcNow;

        await service.MoveTask(request);
        var afterMove = DateTime.UtcNow;

        var updatedTask = await context.Tasks.FindAsync("t1");
        Assert.Equal(originalTime, updatedTask!.CreatedAt);
        Assert.InRange(updatedTask.UpdatedAt, beforeMove.AddSeconds(-1), afterMove.AddSeconds(1));
        Assert.True(updatedTask.UpdatedAt > updatedTask.CreatedAt);
    }

    [Fact]
    public async System.Threading.Tasks.Task MoveTask_ClearsAssignee()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var state1 = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var state2 = new TaskState { Id = "s2", Name = "In Progress", IsFinal = false };
        var user = new User { Id = "u1", FirstName = "Alice" };

        var task = new BOBA.Server.Data.implementation.Task
        {
            Id = "t1",
            TaskTypeId = taskType.Id,
            TaskType = taskType,
            CreatorId = user.Id,
            Creator = user,
            CurrentStateId = state1.Id,
            CurrentState = state1,
            AssigneeId = user.Id,
            Assignee = user,
            TeamId = "team1",
            CreatorTeamId = "team1",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var taskFlow = new TaskFlow
        {
            Id = "tf1",
            TaskTypeId = "type1",
            CurrentStateId = "s1",
            NextState = new List<NextStateItem>
            {
                new NextStateItem
                {
                    ChoiceId = "choice1",
                    NextStateId = "s2",
                    EditRoleId = "team2"
                }
            }
        };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(state1, state2);
        context.Users.Add(user);
        context.Tasks.Add(task);
        context.TaskFlows.Add(taskFlow);
        await context.SaveChangesAsync();

        var service = new TaskService(context, mockFlowService.Object);
        var request = new MoveTaskRequest { TaskId = "t1", ChoiceId = "choice1" };

        await service.MoveTask(request);

        var updatedTask = await context.Tasks.FindAsync("t1");
        Assert.Null(updatedTask!.AssigneeId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetUnassignedTasksByTeamId_ExcludesClosedTasks()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = null,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = null,
                TeamId = "team1",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetUnassignedTasksByTeamId("team1");

        Assert.Single(result);
        Assert.Equal("t2", result[0].Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetExternalTasksByTeamId_ExcludesClosedTasks()
    {

        using var context = CreateInMemoryDbContext();
        var mockFlowService = new Mock<ITaskFlowService>();
        var taskType = new TaskType { Id = "type1", Name = "Bug" };
        var openState = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
        var closedState = new TaskState { Id = "s2", Name = "Closed", IsFinal = true };
        var user = new User { Id = "u1", FirstName = "Alice" };

        context.TaskTypes.Add(taskType);
        context.TaskStates.AddRange(openState, closedState);
        context.Users.Add(user);
        context.Tasks.AddRange(
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t1",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = closedState.Id,
                CurrentState = closedState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team2",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new BOBA.Server.Data.implementation.Task
            {
                Id = "t2",
                TaskTypeId = taskType.Id,
                TaskType = taskType,
                CreatorId = user.Id,
                Creator = user,
                CurrentStateId = openState.Id,
                CurrentState = openState,
                AssigneeId = user.Id,
                Assignee = user,
                TeamId = "team2",
                CreatorTeamId = "team1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        await context.SaveChangesAsync();
        var service = new TaskService(context, mockFlowService.Object);

        var result = await service.GetExternalTasksByTeamId("team1");

        Assert.Single(result);
        Assert.Equal("t2", result[0].Id);
    }

    #endregion
}