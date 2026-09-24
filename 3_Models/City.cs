using Seido.Utilities.SeedGenerator;

namespace Models;

public class City : ICity, ISeed<City>, IEquatable<City>
{
    public virtual Guid CityId { get; set; }
    public virtual string CityName { get; set; }

    public virtual Guid CountryId { get; set; }

    // Model relationships
    // Many Cities belong to one Country
    public virtual ICountry Country { get; set; } = null;

    // One City may have many PostalCodes
    public virtual List<IPostalCode> PostalCodes { get; set; } = null;

    #region constructors
    public City() { }

    public City(City org)
    {
        this.CityId = org.CityId;
        this.CityName = org.CityName;
        this.CountryId = org.CountryId;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(City other) => 
        (other != null) && ((this.CityName?.ToLower().Trim(), this.CountryId) == 
                            (other.CityName?.ToLower().Trim(), other.CountryId));

    public override bool Equals(object obj) => Equals(obj as City);
    public override int GetHashCode() => (CityName?.ToLower().Trim(), CountryId).GetHashCode();
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    public City Seed(SeedGenerator seeder)
    {
        Seeded = true;
        CityId = Guid.NewGuid();

        CityName = seeder.City();

        return this;
    }
    #endregion
}