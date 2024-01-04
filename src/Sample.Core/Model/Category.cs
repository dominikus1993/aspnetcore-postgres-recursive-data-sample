namespace Sample.Core.Model;

public sealed class Category
{
    public CategoryId Id { get; set; }
    public string Name { get; set; }
    public List<Category> SubCategories { get; set; } = new();
    public Category? ParentCategory { get; set; }
}