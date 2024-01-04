using Sample.Core.Model;

namespace Sample.Core.Repositories;

public interface ICategoriesRepository
{
    Task<Category?>GetCategoryAsync(CategoryId id, CancellationToken cancellationToken = default);
    Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default);
}