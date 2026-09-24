using Seido.Utilities.SeedGenerator;

namespace Models;

public class Address : IAddress, ISeed<Address>, IEquatable<Address>
{
    public virtual Guid AddressId { get; set; }
    public virtual string StreetName { get; set; }
    public virtual string StreetNumber { get; set; }

    public virtual Guid PostalCodeId { get; set; }

    // Model relationships
    // Many Addresses belong to one PostalCode
    public virtual IPostalCode PostalCode { get; set; } = null;

    // One Address may have many Customers
    public virtual List<ICustomer> Customers { get; set; } = null;

    // One Address may have many Attractions
    public virtual List<IAttraction> Attractions { get; set; } = null;

    #region constructors
    public Address() { }

    public Address(Address org)
    {
        this.AddressId = org.AddressId;
        this.StreetName = org.StreetName;
        this.StreetNumber = org.StreetNumber;
        this.PostalCodeId = org.PostalCodeId;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(Address other) =>
        (other != null) && ((this.StreetName?.ToLower().Trim(), this.StreetNumber?.ToLower().Trim(), this.PostalCodeId) == 
                            (other.StreetName?.ToLower().Trim(), other.StreetNumber?.ToLower().Trim(), other.PostalCodeId));

    public override bool Equals(object obj) => Equals(obj as Address);
    public override int GetHashCode() => (StreetName?.ToLower().Trim(), StreetNumber?.ToLower().Trim(), PostalCodeId).GetHashCode();
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    public Address Seed(SeedGenerator seeder)
    {
        Seeded = true;
        AddressId = Guid.NewGuid();

        var street = seeder.StreetAddress();

        // StreetAddress() returns e.g. "Ringvägen 42", split into name and number
        var parts = street.LastIndexOf(' ');
        if (parts > 0)
        {
            StreetName = street.Substring(0, parts);
            StreetNumber = street.Substring(parts + 1);
        }
        else
        {
            StreetName = street;
            StreetNumber = $"{seeder.Next(1, 150)}";
        }

        return this;
    }
    #endregion
}