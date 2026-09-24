namespace Models;

public interface IAttractionCategory
{
    public Guid AttractionId { get; set; }
    public Guid CategoryId { get; set; }

    // Model relationships
    // Belongs to one Attraction
    public IAttraction Attraction { get; set; }

    // Belongs to one Category
    public ICategory Category { get; set; }

    public bool Seeded { get; set; }
}

