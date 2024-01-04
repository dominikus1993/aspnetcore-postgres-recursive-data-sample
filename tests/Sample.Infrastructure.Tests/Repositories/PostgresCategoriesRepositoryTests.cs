using Sample.Core.Model;
using Sample.Core.Repositories;
using Sample.Infrastructure.Repositories;
using Sample.Infrastructure.Tests.Fixtures;

namespace Sample.Infrastructure.Tests.Repositories;

public sealed class PostgresCategoriesRepositoryTests : IClassFixture<PostgresFixture>, IDisposable
{
    private readonly PostgresFixture _fixture;
    private readonly ICategoriesRepository _categoriesRepository;

    public PostgresCategoriesRepositoryTests(PostgresFixture fixture)
    {
        _fixture = fixture;
        _categoriesRepository = new PostgresCategoriesRepository(_fixture.CategoriesDbContext);
    }
    

    [Fact]
    public async Task ShouldGetAllChildrenCategories()
    {
        // Arrange
        var parentCategory =
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Parent",
                SubCategories =
                [
                    new Category()
                    {
                        Name = "Child1",
                        Id = Guid.NewGuid(),
                        SubCategories =
                            [new Category() { Id = Guid.NewGuid(), Name = "Child2", SubCategories = [] }]
                    }
                ]
            };
        await _categoriesRepository.AddCategoryAsync(parentCategory);

        // Act
        var result = await _categoriesRepository.GetCategoryAsync(parentCategory.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.SubCategories);
        Assert.Equivalent(parentCategory, result);
    }

    public void Dispose()
    {
        _fixture.ClearDatabaseAsync();
    }
}