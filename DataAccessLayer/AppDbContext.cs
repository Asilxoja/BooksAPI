using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
                    .HasMany(c => c.Books)
                    .WithOne(b => b.Category)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.ClientCascade);
                    

        base.OnModelCreating(modelBuilder);
    }
}