namespace Models;

public interface IAddress
{
    public Guid AddressId { get; set; }
    public string StreetName { get; set; }
    public string StreetNumber { get; set; }

    public Guid PostalCodeId { get; set; }

    // Model relationships
    // Many Addresses belong to one PostalCode
    public IPostalCode PostalCode { get; set; }

    // One Address may have many Customers
    public List<ICustomer> Customers { get; set; }

    // One Address may have many Attractions
    public List<IAttraction> Attractions { get; set; }

    public bool Seeded { get; set; }
}