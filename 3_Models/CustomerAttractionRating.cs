using Seido.Utilities.SeedGenerator;

namespace Models;

public class CustomerAttractionRating : ICustomerAttractionRating, ISeed<CustomerAttractionRating>, IEquatable<CustomerAttractionRating>
{
    public virtual Guid CustomerId { get; set; }
    public virtual Guid AttractionId { get; set; }

    public virtual byte? RatingScore { get; set; }
    public virtual string RatingReview { get; set; }
    public virtual DateTime RatingTimestamp { get; set; } = DateTime.UtcNow;

    // Model relationships
    // Belongs to one Customer
    public virtual ICustomer Customer { get; set; } = null;

    // Belongs to one Attraction
    public virtual IAttraction Attraction { get; set; } = null;

    #region constructors
    public CustomerAttractionRating() { }

    public CustomerAttractionRating(CustomerAttractionRating org)
    {
        this.CustomerId = org.CustomerId;
        this.AttractionId = org.AttractionId;
        this.RatingScore = org.RatingScore;
        this.RatingReview = org.RatingReview;
        this.RatingTimestamp = org.RatingTimestamp;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(CustomerAttractionRating other) =>
        (other != null) && ((this.CustomerId, this.AttractionId) == (other.CustomerId, other.AttractionId));

    public override bool Equals(object obj) => Equals(obj as CustomerAttractionRating);
    public override int GetHashCode() => (CustomerId, AttractionId).GetHashCode();
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    private static readonly string[] _sampleReviews = new[]
    {
        "Absolutely breathtaking, especially at night!",
        "Wonderful place, but very crowded.",
        "Nice view but felt a bit overrated.",
        "Incredible piece of history. A must-see!",
        "Beautiful architecture with great atmosphere.",
        "Very cool! Have never seen anything like it.",
        "Mind-blowing ancient architecture!",
        "Stunning landmark with incredible details.",
        "Engineering masterpiece and beautiful scenery.",
        "Lovely landmark, especially when lit up at night.",
        "Peaceful and spiritual atmosphere.",
        "Great experience, highly recommended."
    };

    public CustomerAttractionRating Seed(SeedGenerator seeder)
    {
        Seeded = true;
        
        // Random rating score between 1 and 5, or null (like in SQL where some only leave reviews)
        RatingScore = (seeder.Next(0, 10) > 2) ? (byte)seeder.Next(1, 6) : null;
        
        RatingReview = _sampleReviews[seeder.Next(0, _sampleReviews.Length)];
        
        // Timestamp within the last 1-2 years
        RatingTimestamp = seeder.DateAndTime(2025, 2026);

        return this;
    }
    #endregion
}