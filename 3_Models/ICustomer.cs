namespace Models;

public interface ICustomer
{
    public Guid CustomerId { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerUserName { get; set; }

    public Guid? AddressId { get; set; }

    // Model relationships
    // One Customer may have one Address
    public IAddress Address { get; set; }

    // One Customer may have many Ratings/Reviews
    public List<ICustomerAttractionRating> CustomerAttractionRatings { get; set; }

    public bool Seeded { get; set; }
}