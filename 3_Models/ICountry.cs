namespace Models;

public interface ICountry
{
    public Guid CountryId { get; set; }
    public string CountryName { get; set; }

    // Model relationships
    // One Country may have many Cities
    public List<ICity> Cities { get; set; }

    public bool Seeded { get; set; }
}
