using Microsoft.EntityFrameworkCore;
using MyWorkout.Api.Models;

namespace MyWorkout.Api.Data;

// Most mezi C# třídami a tabulkami v PostgreSQL.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Každý DbSet = jedna tabulka. Název vlastnosti = název tabulky.
    public DbSet<User> Users => Set<User>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<WorkoutSet> Sets => Set<WorkoutSet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(254).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.DisplayName).HasMaxLength(50).IsRequired();
            e.Property(u => u.Role).HasMaxLength(20).HasDefaultValue("user");
            e.Property(u => u.WeightKg).HasPrecision(5, 2);
            e.Property(u => u.Level).HasMaxLength(30);
            e.Property(u => u.Goal).HasMaxLength(30);
        });

        modelBuilder.Entity<Exercise>(e =>
        {
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.MuscleGroup).HasMaxLength(50).IsRequired();
            e.Property(x => x.Equipment).HasMaxLength(50);
            e.Property(x => x.Difficulty).HasMaxLength(30);
            e.Property(x => x.Source).HasMaxLength(200);
            e.HasData(ExerciseSeed.All);
        });

        modelBuilder.Entity<Workout>(e =>
        {
            e.Property(w => w.Name).HasMaxLength(100);
            e.Property(w => w.Note).HasMaxLength(1000);
            // Smazání uživatele smaže i jeho tréninky.
            e.HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkoutExercise>(e =>
        {
            // Smazání tréninku smaže i jeho cviky v tréninku.
            e.HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);
            // Cvik z databáze nejde smazat, dokud je použitý v nějakém tréninku.
            e.HasOne(we => we.Exercise)
                .WithMany()
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WorkoutSet>(e =>
        {
            e.Property(s => s.WeightKg).HasPrecision(6, 2);
            // Smazání cviku v tréninku smaže i jeho série.
            e.HasOne(s => s.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(s => s.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
