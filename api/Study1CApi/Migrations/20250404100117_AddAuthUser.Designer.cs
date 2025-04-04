﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Study1CApi.Models;

#nullable disable

namespace Study1CApi.Migrations
{
    [DbContext(typeof(Study1cDbContext))]
    [Migration("20250404100117_AddAuthUser")]
    partial class AddAuthUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Study1CApi.Models.AuthUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UserDataCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserHashPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserLogin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.Property<int>("UserRoleNavigationRoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("UserRoleNavigationRoleId");

                    b.ToTable("AuthUsers");
                });

            modelBuilder.Entity("Study1CApi.Models.BlocksMaterial", b =>
                {
                    b.Property<Guid>("BmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("bm_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("Block")
                        .HasColumnType("uuid")
                        .HasColumnName("block");

                    b.Property<DateTime>("BmDateCreate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("bm_date_create")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("Duration")
                        .HasColumnType("integer")
                        .HasColumnName("duration");

                    b.Property<Guid>("Material")
                        .HasColumnType("uuid")
                        .HasColumnName("material");

                    b.Property<string>("Note")
                        .HasColumnType("text")
                        .HasColumnName("note");

                    b.HasKey("BmId")
                        .HasName("pk_blocks_materials");

                    b.HasIndex("Block");

                    b.HasIndex("Material");

                    b.ToTable("blocks_materials", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.BlocksTask", b =>
                {
                    b.Property<Guid>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("task_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("Block")
                        .HasColumnType("uuid")
                        .HasColumnName("block");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Duration")
                        .HasColumnType("integer")
                        .HasColumnName("duration");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<DateTime>("TaskDateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("task_date_created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("task_name");

                    b.Property<int>("TaskNumberOfBlock")
                        .HasColumnType("integer")
                        .HasColumnName("task_number_of_block");

                    b.HasKey("TaskId")
                        .HasName("pk_task");

                    b.HasIndex("Block");

                    b.ToTable("blocks_tasks", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.Course", b =>
                {
                    b.Property<Guid>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("course_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("Author")
                        .HasColumnType("uuid")
                        .HasColumnName("author");

                    b.Property<DateTime>("CourseDataCreate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("course_data_create")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("course_name");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.HasKey("CourseId")
                        .HasName("pk_course");

                    b.HasIndex("Author");

                    b.ToTable("courses", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.CoursesBlock", b =>
                {
                    b.Property<Guid>("BlockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("block_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("BlockDateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("block_date_created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("BlockName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("block_name");

                    b.Property<int?>("BlockNumberOfCourse")
                        .HasColumnType("integer")
                        .HasColumnName("block_number_of_course");

                    b.Property<Guid>("Course")
                        .HasColumnType("uuid")
                        .HasColumnName("course");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.HasKey("BlockId")
                        .HasName("pk_block");

                    b.HasIndex("Course");

                    b.ToTable("courses_blocks", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.Material", b =>
                {
                    b.Property<Guid>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("material_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<DateTime>("MaterialDateCreate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("material_date_create")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("material_name");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("MaterialId")
                        .HasName("pk_material");

                    b.HasIndex("Type");

                    b.ToTable("materials", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.MaterialType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("type_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TypeId"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type_name");

                    b.HasKey("TypeId")
                        .HasName("pk_material_type");

                    b.ToTable("material_type", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId")
                        .HasName("pk_role");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.StudyState", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("state_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StateId"));

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state_name");

                    b.HasKey("StateId")
                        .HasName("pk_state_type");

                    b.ToTable("study_states", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.TasksPractice", b =>
                {
                    b.Property<Guid>("PracticeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("practice_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("Duration")
                        .HasColumnType("integer")
                        .HasColumnName("duration");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<int?>("NumberPracticeOfTask")
                        .HasColumnType("integer")
                        .HasColumnName("number_practice_of_task");

                    b.Property<DateTime>("PracticeDateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("practice_date_created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("PracticeName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("practice_name");

                    b.Property<Guid>("Task")
                        .HasColumnType("uuid")
                        .HasColumnName("task");

                    b.HasKey("PracticeId")
                        .HasName("pk_practice");

                    b.HasIndex("Task");

                    b.ToTable("tasks_practice", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<bool>("IsFirst")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_first");

                    b.Property<DateTime>("UserDataCreate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("user_data_create")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("UserHashPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_hash_password");

                    b.Property<string>("UserLogin")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_login");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.Property<string>("UserPatronymic")
                        .HasColumnType("text")
                        .HasColumnName("user_patronymic");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer")
                        .HasColumnName("user_role");

                    b.Property<string>("UserSurname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_surname");

                    b.HasKey("UserId")
                        .HasName("pk_user");

                    b.HasIndex("UserRole");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.UsersTask", b =>
                {
                    b.Property<Guid>("UtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("ut_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("AuthUser")
                        .HasColumnType("uuid")
                        .HasColumnName("auth_user");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_start");

                    b.Property<int>("DurationMaterial")
                        .HasColumnType("integer")
                        .HasColumnName("duration_material");

                    b.Property<int>("DurationPractice")
                        .HasColumnType("integer")
                        .HasColumnName("duration_practice");

                    b.Property<int>("DurationTask")
                        .HasColumnType("integer")
                        .HasColumnName("duration_task");

                    b.Property<Guid?>("Material")
                        .HasColumnType("uuid")
                        .HasColumnName("material");

                    b.Property<Guid?>("Practice")
                        .HasColumnType("uuid")
                        .HasColumnName("practice");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid?>("Task")
                        .HasColumnType("uuid")
                        .HasColumnName("task");

                    b.HasKey("UtId")
                        .HasName("pk_ut");

                    b.HasIndex("AuthUser");

                    b.HasIndex("Material");

                    b.HasIndex("Practice");

                    b.HasIndex("Status");

                    b.HasIndex("Task");

                    b.ToTable("users_tasks", (string)null);
                });

            modelBuilder.Entity("Study1CApi.Models.AuthUser", b =>
                {
                    b.HasOne("Study1CApi.Models.Role", "UserRoleNavigation")
                        .WithMany()
                        .HasForeignKey("UserRoleNavigationRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRoleNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.BlocksMaterial", b =>
                {
                    b.HasOne("Study1CApi.Models.CoursesBlock", "BlockNavigation")
                        .WithMany("BlocksMaterials")
                        .HasForeignKey("Block")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bm_blocks");

                    b.HasOne("Study1CApi.Models.Material", "MaterialNavigation")
                        .WithMany("BlocksMaterials")
                        .HasForeignKey("Material")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bm_materials");

                    b.Navigation("BlockNavigation");

                    b.Navigation("MaterialNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.BlocksTask", b =>
                {
                    b.HasOne("Study1CApi.Models.CoursesBlock", "BlockNavigation")
                        .WithMany("BlocksTasks")
                        .HasForeignKey("Block")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bm_blocks");

                    b.Navigation("BlockNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.Course", b =>
                {
                    b.HasOne("Study1CApi.Models.User", "AuthorNavigation")
                        .WithMany("Courses")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_course");

                    b.Navigation("AuthorNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.CoursesBlock", b =>
                {
                    b.HasOne("Study1CApi.Models.Course", "CourseNavigation")
                        .WithMany("CoursesBlocks")
                        .HasForeignKey("Course")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_courses_blocks");

                    b.Navigation("CourseNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.Material", b =>
                {
                    b.HasOne("Study1CApi.Models.MaterialType", "TypeNavigation")
                        .WithMany("Materials")
                        .HasForeignKey("Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_materials_types");

                    b.Navigation("TypeNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.TasksPractice", b =>
                {
                    b.HasOne("Study1CApi.Models.BlocksTask", "TaskNavigation")
                        .WithMany("TasksPractices")
                        .HasForeignKey("Task")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_practice_task");

                    b.Navigation("TaskNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.User", b =>
                {
                    b.HasOne("Study1CApi.Models.Role", "UserRoleNavigation")
                        .WithMany("Users")
                        .HasForeignKey("UserRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_role");

                    b.Navigation("UserRoleNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.UsersTask", b =>
                {
                    b.HasOne("Study1CApi.Models.User", "AuthUserNavigation")
                        .WithMany("UsersTasks")
                        .HasForeignKey("AuthUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_tasks_user");

                    b.HasOne("Study1CApi.Models.BlocksMaterial", "MaterialNavigation")
                        .WithMany("UsersTasks")
                        .HasForeignKey("Material")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_users_tasks_material");

                    b.HasOne("Study1CApi.Models.TasksPractice", "PracticeNavigation")
                        .WithMany("UsersTasks")
                        .HasForeignKey("Practice")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_users_tasks_practice");

                    b.HasOne("Study1CApi.Models.StudyState", "StatusNavigation")
                        .WithMany("UsersTasks")
                        .HasForeignKey("Status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_tasks_status");

                    b.HasOne("Study1CApi.Models.BlocksTask", "TaskNavigation")
                        .WithMany("UsersTasks")
                        .HasForeignKey("Task")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_users_tasks_task");

                    b.Navigation("AuthUserNavigation");

                    b.Navigation("MaterialNavigation");

                    b.Navigation("PracticeNavigation");

                    b.Navigation("StatusNavigation");

                    b.Navigation("TaskNavigation");
                });

            modelBuilder.Entity("Study1CApi.Models.BlocksMaterial", b =>
                {
                    b.Navigation("UsersTasks");
                });

            modelBuilder.Entity("Study1CApi.Models.BlocksTask", b =>
                {
                    b.Navigation("TasksPractices");

                    b.Navigation("UsersTasks");
                });

            modelBuilder.Entity("Study1CApi.Models.Course", b =>
                {
                    b.Navigation("CoursesBlocks");
                });

            modelBuilder.Entity("Study1CApi.Models.CoursesBlock", b =>
                {
                    b.Navigation("BlocksMaterials");

                    b.Navigation("BlocksTasks");
                });

            modelBuilder.Entity("Study1CApi.Models.Material", b =>
                {
                    b.Navigation("BlocksMaterials");
                });

            modelBuilder.Entity("Study1CApi.Models.MaterialType", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("Study1CApi.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Study1CApi.Models.StudyState", b =>
                {
                    b.Navigation("UsersTasks");
                });

            modelBuilder.Entity("Study1CApi.Models.TasksPractice", b =>
                {
                    b.Navigation("UsersTasks");
                });

            modelBuilder.Entity("Study1CApi.Models.User", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("UsersTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
