﻿// <auto-generated />
using System;
using BOBA.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BOBA.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250326224943_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BOBA.Server.Data.Choice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("BOBA.Server.Data.Task", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssigneeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrentStateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TaskTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("CurrentStateId");

                    b.HasIndex("TaskTypeId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("BOBA.Server.Data.TaskField", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Validation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("TaskFields");
                });

            modelBuilder.Entity("BOBA.Server.Data.TaskState", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsFinal")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("TaskStates");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Description = "Define campaign goals and target audience.",
                            IsFinal = false,
                            Name = "Planning"
                        },
                        new
                        {
                            Id = "2",
                            Description = "Write copy and design assets.",
                            IsFinal = false,
                            Name = "Content Creation"
                        },
                        new
                        {
                            Id = "3",
                            Description = "Content is reviewed internally by the marketing team.",
                            IsFinal = false,
                            Name = "Internal Review"
                        },
                        new
                        {
                            Id = "4",
                            Description = "Waiting for client approval.",
                            IsFinal = false,
                            Name = "Client Review"
                        },
                        new
                        {
                            Id = "5",
                            Description = "Client requested changes, content needs updating.",
                            IsFinal = false,
                            Name = "Revisions"
                        },
                        new
                        {
                            Id = "6",
                            Description = "Content is finalized and ready for scheduling.",
                            IsFinal = false,
                            Name = "Approved"
                        },
                        new
                        {
                            Id = "7",
                            Description = "Campaign is scheduled for publishing.",
                            IsFinal = false,
                            Name = "Scheduled"
                        },
                        new
                        {
                            Id = "8",
                            Description = "Content has been posted on selected platforms.",
                            IsFinal = true,
                            Name = "Published"
                        },
                        new
                        {
                            Id = "9",
                            Description = "The task is completed and archived.",
                            IsFinal = true,
                            Name = "Cancelled"
                        });
                });

            modelBuilder.Entity("BOBA.Server.Data.TaskType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("TaskTypes");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Description = "Plan, create, and schedule posts for social media platforms.",
                            Name = "Social Media Campaign"
                        },
                        new
                        {
                            Id = "2",
                            Description = "Create and manage an advertising campaign across different channels.",
                            Name = "Ad Campaign"
                        },
                        new
                        {
                            Id = "3",
                            Description = "Design and send promotional emails to targeted audiences.",
                            Name = "Email Marketing"
                        },
                        new
                        {
                            Id = "4",
                            Description = "Improve website SEO through keyword research and content updates.",
                            Name = "SEO Optimization"
                        },
                        new
                        {
                            Id = "5",
                            Description = "Analyze competitors, customer behavior, and industry trends.",
                            Name = "Market Research"
                        });
                });

            modelBuilder.Entity("BOBA.Server.Data.Taskflow", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrentStateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EditRoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NextStateJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentStateId");

                    b.HasIndex("EditRoleId");

                    b.HasIndex("TaskTypeId");

                    b.ToTable("TaskFlows");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            CurrentStateId = "1",
                            NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"2\"}, {\"choiceId\": \"2\", \"nextStateId\": \"3\"}, {\"choiceId\": \"3\", \"nextStateId\": \"4\"}]",
                            TaskTypeId = "2"
                        },
                        new
                        {
                            Id = "2",
                            CurrentStateId = "2",
                            NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"3\"}, {\"choiceId\": \"2\", \"nextStateId\": \"4\"}, {\"choiceId\": \"3\", \"nextStateId\": \"5\"}]",
                            TaskTypeId = "2"
                        },
                        new
                        {
                            Id = "3",
                            CurrentStateId = "3",
                            NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"5\"}, {\"choiceId\": \"2\", \"nextStateId\": \"6\"}]",
                            TaskTypeId = "2"
                        },
                        new
                        {
                            Id = "4",
                            CurrentStateId = "4",
                            NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"6\"}, {\"choiceId\": \"2\", \"nextStateId\": \"7\"}]",
                            TaskTypeId = "2"
                        },
                        new
                        {
                            Id = "5",
                            CurrentStateId = "5",
                            NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"7\"}, {\"choiceId\": \"2\", \"nextStateId\": \"8\"}]",
                            TaskTypeId = "2"
                        },
                        new
                        {
                            Id = "6",
                            CurrentStateId = "6",
                            NextStateJson = "[{\"choiceId\": \"1\", \"nextStateId\": \"8\"}, {\"choiceId\": \"2\", \"nextStateId\": \"9\"}]",
                            TaskTypeId = "2"
                        });
                });

            modelBuilder.Entity("BOBA.Server.Data.Team", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Content Team"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Design Team"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Advertising Team"
                        },
                        new
                        {
                            Id = "4",
                            Name = "SEO & Analytics Team"
                        },
                        new
                        {
                            Id = "5",
                            Name = "Client Management"
                        });
                });

            modelBuilder.Entity("BOBA.Server.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TaskflowTeam", b =>
                {
                    b.Property<string>("ReadOnlyRoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReadOnlyRoleWorkflowsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReadOnlyRoleId", "ReadOnlyRoleWorkflowsId");

                    b.HasIndex("ReadOnlyRoleWorkflowsId");

                    b.ToTable("TaskflowTeam");
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.Property<string>("TeamsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TeamsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("TeamUser");
                });

            modelBuilder.Entity("BOBA.Server.Data.Task", b =>
                {
                    b.HasOne("BOBA.Server.Data.User", "Assignee")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.User", "Creator")
                        .WithMany("CreatedTasks")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.TaskState", "CurrentState")
                        .WithMany("CurrentStateTasks")
                        .HasForeignKey("CurrentStateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.TaskType", "TaskType")
                        .WithMany()
                        .HasForeignKey("TaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Creator");

                    b.Navigation("CurrentState");

                    b.Navigation("TaskType");
                });

            modelBuilder.Entity("BOBA.Server.Data.Taskflow", b =>
                {
                    b.HasOne("BOBA.Server.Data.TaskState", "CurrentState")
                        .WithMany("CurrentStateTaskflows")
                        .HasForeignKey("CurrentStateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.Team", "EditRole")
                        .WithMany("EditRoleWorkflows")
                        .HasForeignKey("EditRoleId");

                    b.HasOne("BOBA.Server.Data.TaskType", "TaskType")
                        .WithMany()
                        .HasForeignKey("TaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentState");

                    b.Navigation("EditRole");

                    b.Navigation("TaskType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BOBA.Server.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BOBA.Server.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BOBA.Server.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskflowTeam", b =>
                {
                    b.HasOne("BOBA.Server.Data.Team", null)
                        .WithMany()
                        .HasForeignKey("ReadOnlyRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.Taskflow", null)
                        .WithMany()
                        .HasForeignKey("ReadOnlyRoleWorkflowsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.HasOne("BOBA.Server.Data.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BOBA.Server.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BOBA.Server.Data.TaskState", b =>
                {
                    b.Navigation("CurrentStateTaskflows");

                    b.Navigation("CurrentStateTasks");
                });

            modelBuilder.Entity("BOBA.Server.Data.Team", b =>
                {
                    b.Navigation("EditRoleWorkflows");
                });

            modelBuilder.Entity("BOBA.Server.Data.User", b =>
                {
                    b.Navigation("AssignedTasks");

                    b.Navigation("CreatedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
