using Microsoft.EntityFrameworkCore;

using Sample.Core.Model;

namespace Sample.Infrastructure.EntityFramework;

internal sealed class CategoriesDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    
    public CategoriesDbContext(DbContextOptions<CategoriesDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Name).HasMaxLength(200);
            entity.HasMany(x => x.SubCategories).WithOne(x => x.ParentCategory);
        });
        base.OnModelCreating(modelBuilder);
    }
}