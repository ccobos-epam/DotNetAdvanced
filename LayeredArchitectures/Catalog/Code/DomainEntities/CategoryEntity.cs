namespace DomainEntities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }

    //A category can have one parent
    public CategoryEntity? ParentCategory { get; set; }
    public Guid? ParentCategoryId { get; set; }

    //A Category can have many children
    public IList<CategoryEntity>? PossibleChildCategories { get; set; }

    public IList<ProductEntity>? ProductsInCategory { get; set; }
}
