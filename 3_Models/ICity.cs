namespace Models;

public interface ICity
{
    public Guid CityId { get; set; }
    public string CityName { get; set; }

    public Guid CountryId { get; set; }

    // Model relationships
    // Many Cities belong to one Country
    public ICountry Country { get; set; }

    // One City may have many PostalCodes
    public List<IPostalCode> PostalCodes { get; set; }

    public bool Seeded { get; set; }
}