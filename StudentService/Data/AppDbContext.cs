using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed some initial data
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice@example.com", Course = "Software Engineering", Year = 2, CreatedAt = DateTime.UtcNow },
            new Student { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob@example.com", Course = "Computer Science", Year = 3, CreatedAt = DateTime.UtcNow }
        );
    }
}
