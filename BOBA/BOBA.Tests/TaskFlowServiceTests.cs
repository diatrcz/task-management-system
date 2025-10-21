using BOBA.Server.Data;
using BOBA.Server.Data.implementation;
using BOBA.Server.Data.model;
using BOBA.Server.Models;
using BOBA.Server.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BOBA.Tests
{
    public class TaskFlowServiceTests
    {
        private ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        #region GetEditRoleId Tests

        [Fact]
        public async System.Threading.Tasks.Task GetEditRoleId_ReturnsCorrectEditRoleId()
        {

            using var context = CreateInMemoryDbContext();
            var taskFlow = new TaskFlow
            {
                Id = "tf1",
                TaskTypeId = "type1",
                CurrentStateId = "s1",
                EditRoleId = "role1",
                NextState = new List<NextStateItem>(),
                ReadOnlyRole = new List<Team>(),
                FormField = new List<FormFieldItem>()
            };

            context.TaskFlows.Add(taskFlow);
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetEditRoleId("type1", "s1");

            Assert.Equal("role1", result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetEditRoleId_ThrowsException_WhenFlowNotFound()
        {
            using var context = CreateInMemoryDbContext();
            var service = new TaskFlowService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.GetEditRoleId("nonexistent", "nonexistent"));
        }

        #endregion

        #region GetTaskFlowById Tests

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_ReturnsCompleteTaskFlowDto()
        {

            using var context = CreateInMemoryDbContext();
            var taskType = new TaskType { Id = "type1", Name = "Bug" };
            var state = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
            var user = new User { Id = "u1", FirstName = "Alice" };
            var team = new Team { Id = "team1", Name = "Dev Team" };

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

            var taskFlow = new TaskFlow
            {
                Id = "tf1",
                TaskTypeId = "type1",
                CurrentStateId = "s1",
                EditRoleId = "role1",
                NextState = new List<NextStateItem>
                {
                    new NextStateItem { ChoiceId = "choice1", NextStateId = "s2", EditRoleId = "role2" }
                },
                ReadOnlyRole = new List<Team> { team },
                FormField = new List<FormFieldItem>
                {
                    new FormFieldItem
                    {
                        Layout = new Layout
                        {
                            Type = "grid",
                            Columns = 2,
                            GapClasses = "gap-4"
                        },
                        Fields = new List<Field>
                        {
                            new Field
                            {
                                FieldId = "field1",
                                Required = true,
                                Disabled = false,
                                Rows = 1,
                                StyleClasses = new StyleClasses
                                {
                                    Label = "label-class",
                                    Input = "input-class"
                                }
                            }
                        }
                    }
                }
            };

            context.TaskTypes.Add(taskType);
            context.TaskStates.Add(state);
            context.Users.Add(user);
            context.Teams.Add(team);
            context.Tasks.Add(task);
            context.TaskFlows.Add(taskFlow);
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetTaskFlowById("t1");

            Assert.NotNull(result);
            Assert.Equal("tf1", result.Id);
            Assert.Equal("role1", result.EditRoleId);
            Assert.Single(result.NextState);
            Assert.Equal("choice1", result.NextState[0].ChoiceId);
            Assert.Equal("s2", result.NextState[0].NextStateId);
            Assert.Single(result.ReadOnlyRole);
            Assert.Equal("team1", result.ReadOnlyRole[0]);
            Assert.Single(result.FormFields);
            Assert.Equal("grid", result.FormFields[0].Layout.Type);
            Assert.Equal(2, result.FormFields[0].Layout.Columns);
            Assert.Single(result.FormFields[0].Fields);
            Assert.Equal("field1", result.FormFields[0].Fields[0].FieldId);
            Assert.True(result.FormFields[0].Fields[0].Required);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_HandlesEmptyCollections()
        {

            using var context = CreateInMemoryDbContext();
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

            var taskFlow = new TaskFlow
            {
                Id = "tf1",
                TaskTypeId = "type1",
                CurrentStateId = "s1",
                EditRoleId = "role1",
                NextState = new List<NextStateItem>(),
                ReadOnlyRole = new List<Team>(),
                FormField = new List<FormFieldItem>()
            };

            context.TaskTypes.Add(taskType);
            context.TaskStates.Add(state);
            context.Users.Add(user);
            context.Tasks.Add(task);
            context.TaskFlows.Add(taskFlow);
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetTaskFlowById("t1");

            Assert.NotNull(result);
            Assert.Empty(result.NextState);
            Assert.Empty(result.ReadOnlyRole);
            Assert.Empty(result.FormFields);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_HandlesMultipleNextStates()
        {
   
            using var context = CreateInMemoryDbContext();
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

            var taskFlow = new TaskFlow
            {
                Id = "tf1",
                TaskTypeId = "type1",
                CurrentStateId = "s1",
                EditRoleId = "role1",
                NextState = new List<NextStateItem>
                {
                    new NextStateItem { ChoiceId = "approve", NextStateId = "s2", EditRoleId = "role2" },
                    new NextStateItem { ChoiceId = "reject", NextStateId = "s3", EditRoleId = "role3" },
                    new NextStateItem { ChoiceId = "defer", NextStateId = "s4", EditRoleId = "role4" }
                },
                ReadOnlyRole = new List<Team>(),
                FormField = new List<FormFieldItem>()
            };

            context.TaskTypes.Add(taskType);
            context.TaskStates.Add(state);
            context.Users.Add(user);
            context.Tasks.Add(task);
            context.TaskFlows.Add(taskFlow);
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetTaskFlowById("t1");

            Assert.NotNull(result);
            Assert.Equal(3, result.NextState.Count);
            Assert.Contains(result.NextState, ns => ns.ChoiceId == "approve" && ns.NextStateId == "s2");
            Assert.Contains(result.NextState, ns => ns.ChoiceId == "reject" && ns.NextStateId == "s3");
            Assert.Contains(result.NextState, ns => ns.ChoiceId == "defer" && ns.NextStateId == "s4");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_HandlesMultipleReadOnlyRoles()
        {

            using var context = CreateInMemoryDbContext();
            var taskType = new TaskType { Id = "type1", Name = "Bug" };
            var state = new TaskState { Id = "s1", Name = "Open", IsFinal = false };
            var user = new User { Id = "u1", FirstName = "Alice" };
            var team1 = new Team { Id = "team1", Name = "Team 1" };
            var team2 = new Team { Id = "team2", Name = "Team 2" };

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

            var taskFlow = new TaskFlow
            {
                Id = "tf1",
                TaskTypeId = "type1",
                CurrentStateId = "s1",
                EditRoleId = "role1",
                NextState = new List<NextStateItem>(),
                ReadOnlyRole = new List<Team> { team1, team2 },
                FormField = new List<FormFieldItem>()
            };

            context.TaskTypes.Add(taskType);
            context.TaskStates.Add(state);
            context.Users.Add(user);
            context.Teams.AddRange(team1, team2);
            context.Tasks.Add(task);
            context.TaskFlows.Add(taskFlow);
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetTaskFlowById("t1");

            Assert.NotNull(result);
            Assert.Equal(2, result.ReadOnlyRole.Count);
            Assert.Contains("team1", result.ReadOnlyRole);
            Assert.Contains("team2", result.ReadOnlyRole);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_HandlesMultipleFormFields()
        {

            using var context = CreateInMemoryDbContext();
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

            var taskFlow = new TaskFlow
            {
                Id = "tf1",
                TaskTypeId = "type1",
                CurrentStateId = "s1",
                EditRoleId = "role1",
                NextState = new List<NextStateItem>(),
                ReadOnlyRole = new List<Team>(),
                FormField = new List<FormFieldItem>
                {
                    new FormFieldItem
                    {
                        Layout = new Layout { Type = "grid", Columns = 2, GapClasses = "gap-4" },
                        Fields = new List<Field>
                        {
                            new Field
                            {
                                FieldId = "field1",
                                Required = true,
                                Disabled = false,
                                Rows = 1,
                                StyleClasses = new StyleClasses { Label = "label1", Input = "input1" }
                            }
                        }
                    },
                    new FormFieldItem
                    {
                        Layout = new Layout { Type = "flex", Columns = 1, GapClasses = "gap-2" },
                        Fields = new List<Field>
                        {
                            new Field
                            {
                                FieldId = "field2",
                                Required = false,
                                Disabled = true,
                                Rows = 3,
                                StyleClasses = new StyleClasses { Label = "label2", Input = "input2" }
                            }
                        }
                    }
                }
            };

            context.TaskTypes.Add(taskType);
            context.TaskStates.Add(state);
            context.Users.Add(user);
            context.Tasks.Add(task);
            context.TaskFlows.Add(taskFlow);
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetTaskFlowById("t1");

            Assert.NotNull(result);
            Assert.Equal(2, result.FormFields.Count);
            Assert.Equal("field1", result.FormFields[0].Fields[0].FieldId);
            Assert.Equal("field2", result.FormFields[1].Fields[0].FieldId);
            Assert.True(result.FormFields[0].Fields[0].Required);
            Assert.False(result.FormFields[1].Fields[0].Required);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_ThrowsException_WhenTaskNotFound()
        {

            using var context = CreateInMemoryDbContext();
            var service = new TaskFlowService(context);


            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.GetTaskFlowById("nonexistent"));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskFlowById_ThrowsException_WhenTaskFlowNotFound()
        {

            using var context = CreateInMemoryDbContext();
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

            var service = new TaskFlowService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.GetTaskFlowById("t1"));
        }

        #endregion

        #region GetChoices Tests

        [Fact]
        public async System.Threading.Tasks.Task GetChoices_ReturnsAllValidChoices()
        {

            using var context = CreateInMemoryDbContext();
            context.Choices.AddRange(
                new Choice { Id = "choice1", Name = "Approve", Description = "Approve action" },
                new Choice { Id = "choice2", Name = "Reject", Description = "Reject action" },
                new Choice { Id = "choice3", Name = "Defer", Description = "Defer action" }
            );
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);
            var ids = new List<string> { "choice1", "choice2", "choice3" };

            var result = await service.GetChoices(ids);

            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Id == "choice1" && c.Name == "Approve");
            Assert.Contains(result, c => c.Id == "choice2" && c.Name == "Reject");
            Assert.Contains(result, c => c.Id == "choice3" && c.Name == "Defer");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetChoices_SkipsNonExistentChoices()
        {

            using var context = CreateInMemoryDbContext();
            context.Choices.AddRange(
                new Choice { Id = "choice1", Name = "Approve", Description = "Approve action" },
                new Choice { Id = "choice2", Name = "Reject", Description = "Reject action" }
            );
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);
            var ids = new List<string> { "choice1", "nonexistent", "choice2" };

            var result = await service.GetChoices(ids);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Id == "choice1");
            Assert.Contains(result, c => c.Id == "choice2");
            Assert.DoesNotContain(result, c => c.Id == "nonexistent");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetChoices_ReturnsEmptyList_WhenNoValidChoices()
        {

            using var context = CreateInMemoryDbContext();
            var service = new TaskFlowService(context);
            var ids = new List<string> { "nonexistent1", "nonexistent2" };

            var result = await service.GetChoices(ids);

            Assert.Empty(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetChoices_ReturnsEmptyList_WhenEmptyIdList()
        {
 
            using var context = CreateInMemoryDbContext();
            var service = new TaskFlowService(context);
            var ids = new List<string>();

            var result = await service.GetChoices(ids);

            Assert.Empty(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetChoices_PreservesOrderBasedOnFoundChoices()
        {

            using var context = CreateInMemoryDbContext();
            context.Choices.AddRange(
                new Choice { Id = "c1", Name = "First", Description = "First choice" },
                new Choice { Id = "c2", Name = "Second", Description = "Second choice" },
                new Choice { Id = "c3", Name = "Third", Description = "Third choice" }
            );
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);
            var ids = new List<string> { "c1", "c2", "c3" };


            var result = await service.GetChoices(ids);

            Assert.Equal(3, result.Count);

            Assert.Equal("c1", result[0].Id);
            Assert.Equal("c2", result[1].Id);
            Assert.Equal("c3", result[2].Id);
        }

        #endregion

        #region GetTaskStateNameById Tests

        [Fact]
        public async System.Threading.Tasks.Task GetTaskStateNameById_ReturnsCorrectStateName()
        {

            using var context = CreateInMemoryDbContext();
            context.TaskStates.AddRange(
                new TaskState { Id = "s1", Name = "Open", IsFinal = false },
                new TaskState { Id = "s2", Name = "Closed", IsFinal = true }
            );
            await context.SaveChangesAsync();

            var service = new TaskFlowService(context);

            var result = await service.GetTaskStateNameById("s1");

            Assert.Equal("Open", result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskStateNameById_ThrowsException_WhenStateNotFound()
        {

            using var context = CreateInMemoryDbContext();
            var service = new TaskFlowService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.GetTaskStateNameById("nonexistent"));
        }

        #endregion
    }
}