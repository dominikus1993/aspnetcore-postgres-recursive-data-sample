using Microsoft.EntityFrameworkCore;

using Sample.Core.Model;
using Sample.Core.Repositories;
using Sample.Infrastructure.EntityFramework;

namespace Sample.Infrastructure.Repositories;

internal sealed class PostgresCategoriesRepository : ICategoriesRepository
{
    private readonly CategoriesDbContext _dbContext;

    public PostgresCategoriesRepository(CategoriesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.AsNoTracking()
            .Include(x => x.SubCategories)
            .Select(x =>
                new Category()
                {
                    Id = x.Id,
                    Name = x.Name,
                    SubCategories = x.SubCategories.Select(c => new Category()
                    {
                        Id = c.Id, Name = c.Name, SubCategories = c.SubCategories, ParentCategory = null
                    }).ToList(), ParentCategory = null
                })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category?.SubCategories is { Count: > 0 })
        {
            await LoadSubCategories(category.SubCategories, 3);
        }

        return category;
    }

    private async Task LoadSubCategories(List<Category> categories, int depth)
    {
        if (depth <= 0)
            return;

        foreach (var category in categories)
        {
            category.SubCategories = await _dbContext.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategory!.Id == category.Id)
                .Select(x =>
                    new Category()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SubCategories = x.SubCategories.Select(c => new Category()
                        {
                            Id = c.Id, Name = c.Name, SubCategories = c.SubCategories, ParentCategory = null
                        }).ToList(),
                        ParentCategory = null
                    })
                .ToListAsync();

            if (category.SubCategories is { Count: > 0 })
            {
                await LoadSubCategories(category.SubCategories, depth - 1);
            }
        }
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}