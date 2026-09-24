using Seido.Utilities.SeedGenerator;

namespace Models;

public class Customer : ICustomer, ISeed<Customer>, IEquatable<Customer>
{
    public virtual Guid CustomerId { get; set; }
    public virtual string CustomerFirstName { get; set; }
    public virtual string CustomerLastName { get; set; }
    public virtual string CustomerUserName { get; set; }

    public virtual Guid? AddressId { get; set; }

    // Model relationships
    // One Customer may have one Address
    public virtual IAddress Address { get; set; } = null;

    // One Customer may have many Ratings/Reviews
    public virtual List<ICustomerAttractionRating> CustomerAttractionRatings { get; set; } = null;

    #region constructors
    public Customer() { }

    public Customer(Customer org)
    {
        this.CustomerId = org.CustomerId;
        this.CustomerFirstName = org.CustomerFirstName;
        this.CustomerLastName = org.CustomerLastName;
        this.CustomerUserName = org.CustomerUserName;
        this.AddressId = org.AddressId;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(Customer other) =>
        (other != null) && (this.CustomerUserName?.ToLower().Trim() == other.CustomerUserName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as Customer);
    public override int GetHashCode() => CustomerUserName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    public Customer Seed(SeedGenerator seeder)
    {
        Seeded = true;
        CustomerId = Guid.NewGuid();

        CustomerFirstName = seeder.FirstName;
        CustomerLastName = seeder.LastName;
        CustomerUserName = seeder.Email(CustomerFirstName.ToLower(), CustomerLastName.ToLower());

        // AddressId will be linked later
        AddressId = null;

        return this;
    }
    #endregion
}