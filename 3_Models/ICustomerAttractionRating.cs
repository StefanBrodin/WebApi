namespace Models;

public interface ICustomerAttractionRating
{
    public Guid CustomerId { get; set; }
    public Guid AttractionId { get; set; }

    public byte? RatingScore { get; set; }
    public string RatingReview { get; set; }
    public DateTime RatingTimestamp { get; set; }

    // Model relationships
    // Belongs to one Customer
    public ICustomer Customer { get; set; }

    // Belongs to one Attraction
    public IAttraction Attraction { get; set; }

    public bool Seeded { get; set; }
}