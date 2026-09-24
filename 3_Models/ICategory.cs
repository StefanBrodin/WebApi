namespace Models;

public interface ICategory
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }

    // Model relationships
    // Many Categories can be linked to Many Attractions (via AttractionCategory)
    public List<IAttractionCategory> AttractionCategories { get; set; }

    public bool Seeded { get; set; }
}