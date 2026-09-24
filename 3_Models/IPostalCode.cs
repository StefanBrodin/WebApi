namespace Models;

public interface IPostalCode
{
    public Guid PostalCodeId { get; set; }
    public string PostalCodeNumber { get; set; }

    public Guid CityId { get; set; }

    // Model relationships
    // Many PostalCodes belong to one City
    public ICity City { get; set; }

    // One PostalCode may have many Addresses
    public List<IAddress> Addresses { get; set; }

    public bool Seeded { get; set; }
}