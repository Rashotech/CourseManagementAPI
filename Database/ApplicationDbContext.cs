using CourseManagement.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Database
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Course>()
        .HasOne(c => c.Instructor)
        .WithMany()
        .HasForeignKey(c => c.InstructorId);
    }

    public override int SaveChanges()
    {
      UpdateTimestamps();
      return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      UpdateTimestamps();
      return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
      var entries = ChangeTracker
          .Entries()
          .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

      foreach (var entityEntry in entries)
      {
        if (entityEntry.Entity is not ApplicationUser)
        {
          ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

          if (entityEntry.State == EntityState.Added)
          {
            ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
          }
        }
      }
    }
  }
}
