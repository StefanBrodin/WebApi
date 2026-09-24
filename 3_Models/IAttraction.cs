namespace Models;

public interface IAttraction
{
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }

    public Guid AddressId { get; set; }

    // Model relationships
    // Many Attractions belong to one Address
    public IAddress Address { get; set; }

    // One Attraction may have many Ratings/Reviews
    public List<ICustomerAttractionRating> CustomerAttractionRatings { get; set; }

    // One Attraction may have many Categories (via AttractionCategory)
    public List<IAttractionCategory> AttractionCategories { get; set; }

    public bool Seeded { get; set; }
}