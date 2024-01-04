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
        var categoryQ = _dbContext.Categories
            .Include(x => x.SubCategories)
            .Where(x => x.Id == id);

        return await categoryQ.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);;
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}