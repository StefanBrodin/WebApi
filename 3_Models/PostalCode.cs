using Seido.Utilities.SeedGenerator;

namespace Models;

public class PostalCode : IPostalCode, ISeed<PostalCode>, IEquatable<PostalCode>
{
    public virtual Guid PostalCodeId { get; set; }
    public virtual string PostalCodeNumber { get; set; }

    public virtual Guid CityId { get; set; }

    // Model relationships
    // Many PostalCodes belong to one City
    public virtual ICity City { get; set; } = null;

    // One PostalCode may have many Addresses
    public virtual List<IAddress> Addresses { get; set; } = null;

    #region constructors
    public PostalCode() { }

    public PostalCode(PostalCode org)
    {
        this.PostalCodeId = org.PostalCodeId;
        this.PostalCodeNumber = org.PostalCodeNumber;
        this.CityId = org.CityId;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(PostalCode other) =>
        (other != null) && ((this.PostalCodeNumber?.Trim(), this.CityId) == 
                            (other.PostalCodeNumber?.Trim(), other.CityId));

    public override bool Equals(object obj) => Equals(obj as PostalCode);
    public override int GetHashCode() => (PostalCodeNumber?.Trim(), CityId).GetHashCode();
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    public PostalCode Seed(SeedGenerator seeder)
    {
        Seeded = true;
        PostalCodeId = Guid.NewGuid();

        PostalCodeNumber = $"{seeder.ZipCode}";

        return this;
    }
    #endregion
}