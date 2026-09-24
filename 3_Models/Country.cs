using Seido.Utilities.SeedGenerator;

namespace Models;

public class Country : ICountry, ISeed<Country>, IEquatable<Country>
{
    public virtual Guid CountryId { get; set; }
    public virtual string CountryName { get; set; }

    // Model relationships
    // One Country may have many Cities
    public virtual List<ICity> Cities { get; set; } = null;

    #region constructors
    public Country() { }

    public Country(Country org)
    {
        this.CountryId = org.CountryId;
        this.CountryName = org.CountryName;

    }
    #endregion

    #region implementing IEquatable
    public bool Equals(Country other) => 
        (other != null) && (CountryName?.ToLower().Trim() == other.CountryName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as Country);
    public override int GetHashCode() => CountryName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    public Country Seed(SeedGenerator seeder)
    {
        Seeded = true;
        CountryId = Guid.NewGuid();

        CountryName = seeder.Country;

        return this;
    }
    #endregion
}