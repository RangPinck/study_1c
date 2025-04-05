using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Study1CApi.Models;

public partial class Study1cDbContext : IdentityDbContext<AuthUser, Role, Guid>
{
    public Study1cDbContext()
    {
    }

    public Study1cDbContext(DbContextOptions<Study1cDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlocksMaterial> BlocksMaterials { get; set; }

    public virtual DbSet<BlocksTask> BlocksTasks { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CoursesBlock> CoursesBlocks { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StudyState> StudyStates { get; set; }

    public virtual DbSet<TasksPractice> TasksPractices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersTask> UsersTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=123456;Database=study_1C_db;Pooling=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.HasOne(it => it.UserNavigation).WithOne(it => it.AuthUserNavigation).HasForeignKey<User>(it => it.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        });

        modelBuilder.Entity<BlocksMaterial>(entity =>
        {
            entity.HasKey(e => e.BmId).HasName("pk_blocks_materials");

            entity.ToTable("blocks_materials");

            entity.Property(e => e.BmId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("bm_id");
            entity.Property(e => e.Block).HasColumnName("block");
            entity.Property(e => e.BmDateCreate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("bm_date_create");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Material).HasColumnName("material");
            entity.Property(e => e.Note).HasColumnName("note");

            entity.HasOne(d => d.BlockNavigation).WithMany(p => p.BlocksMaterials)
                .HasForeignKey(d => d.Block)
                .HasConstraintName("fk_bm_blocks");

            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.BlocksMaterials)
                .HasForeignKey(d => d.Material)
                .HasConstraintName("fk_bm_materials");
        });

        modelBuilder.Entity<BlocksTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("pk_task");

            entity.ToTable("blocks_tasks");

            entity.Property(e => e.TaskId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("task_id");
            entity.Property(e => e.Block).HasColumnName("block");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.TaskDateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("task_date_created");
            entity.Property(e => e.TaskName).HasColumnName("task_name");
            entity.Property(e => e.TaskNumberOfBlock).HasColumnName("task_number_of_block");

            entity.HasOne(d => d.BlockNavigation).WithMany(p => p.BlocksTasks)
                .HasForeignKey(d => d.Block)
                .HasConstraintName("fk_bm_blocks");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("pk_course");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("course_id");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.CourseDataCreate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("course_data_create");
            entity.Property(e => e.CourseName).HasColumnName("course_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Link).HasColumnName("link");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("fk_user_course");
        });

        modelBuilder.Entity<CoursesBlock>(entity =>
        {
            entity.HasKey(e => e.BlockId).HasName("pk_block");

            entity.ToTable("courses_blocks");

            entity.Property(e => e.BlockId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("block_id");
            entity.Property(e => e.BlockDateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("block_date_created");
            entity.Property(e => e.BlockName).HasColumnName("block_name");
            entity.Property(e => e.BlockNumberOfCourse).HasColumnName("block_number_of_course");
            entity.Property(e => e.Course).HasColumnName("course");
            entity.Property(e => e.Description).HasColumnName("description");

            entity.HasOne(d => d.CourseNavigation).WithMany(p => p.CoursesBlocks)
                .HasForeignKey(d => d.Course)
                .HasConstraintName("fk_courses_blocks");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("pk_material");

            entity.ToTable("materials");

            entity.Property(e => e.MaterialId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("material_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.MaterialDateCreate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("material_date_create");
            entity.Property(e => e.MaterialName).HasColumnName("material_name");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Materials)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("fk_materials_types");
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("pk_material_type");

            entity.ToTable("material_type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeName).HasColumnName("type_name");
        });

        modelBuilder.Entity<StudyState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("pk_state_type");

            entity.ToTable("study_states");

            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.StateName).HasColumnName("state_name");
        });

        modelBuilder.Entity<TasksPractice>(entity =>
        {
            entity.HasKey(e => e.PracticeId).HasName("pk_practice");

            entity.ToTable("tasks_practice");

            entity.Property(e => e.PracticeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("practice_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.NumberPracticeOfTask).HasColumnName("number_practice_of_task");
            entity.Property(e => e.PracticeDateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("practice_date_created");
            entity.Property(e => e.PracticeName).HasColumnName("practice_name");
            entity.Property(e => e.Task).HasColumnName("task");

            entity.HasOne(d => d.TaskNavigation).WithMany(p => p.TasksPractices)
                .HasForeignKey(d => d.Task)
                .HasConstraintName("fk_practice_task");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_user");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_id");
            entity.Property(e => e.IsFirst)
                .HasDefaultValue(true)
                .HasColumnName("is_first");
            entity.Property(e => e.UserName).HasColumnName("user_name");
            entity.Property(e => e.UserPatronymic).HasColumnName("user_patronymic");
            entity.Property(e => e.UserSurname).HasColumnName("user_surname");
        });

        modelBuilder.Entity<UsersTask>(entity =>
        {
            entity.HasKey(e => e.UtId).HasName("pk_ut");

            entity.ToTable("users_tasks");

            entity.Property(e => e.UtId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ut_id");
            entity.Property(e => e.AuthUser).HasColumnName("auth_user");
            entity.Property(e => e.DateStart).HasColumnName("date_start");
            entity.Property(e => e.DurationMaterial).HasColumnName("duration_material");
            entity.Property(e => e.DurationPractice).HasColumnName("duration_practice");
            entity.Property(e => e.DurationTask).HasColumnName("duration_task");
            entity.Property(e => e.Material).HasColumnName("material");
            entity.Property(e => e.Practice).HasColumnName("practice");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Task).HasColumnName("task");

            entity.HasOne(d => d.AuthUserNavigation).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.AuthUser)
                .HasConstraintName("fk_users_tasks_user");

            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.Material)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_tasks_material");

            entity.HasOne(d => d.PracticeNavigation).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.Practice)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_tasks_practice");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("fk_users_tasks_status");

            entity.HasOne(d => d.TaskNavigation).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.Task)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_tasks_task");

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens", t => t.ExcludeFromMigrations());

            modelBuilder.Entity<Role>().HasData(new List<Role>
        {
            new Role
            {
                Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                Name = "Ученик",
                NormalizedName = "УЧЕНИК",
                ConcurrencyStamp = "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                IsNoManipulate = true
            },
            new Role
            {
                Id = new Guid("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e"),
                Name = "Куратор",
                NormalizedName = "КУРАТОР",
                ConcurrencyStamp = "c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e",
                IsNoManipulate= true
            },
            new Role
            {
                Id = new Guid("f45d2396-3e72-4ec7-b892-7bd454248158"),
                Name = "Администратор",
                NormalizedName = "АДМНИСТРАТОР",
                ConcurrencyStamp = "f45d2396-3e72-4ec7-b892-7bd454248158",
                IsNoManipulate  = true
            },
        });
        });
    }
}
