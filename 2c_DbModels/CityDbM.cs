using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("City", Schema = "dbo")]
public class CityDbM : City, ISeed<CityDbM>, IEquatable<CityDbM>
{
    [Key]
    public override Guid CityId { get; set; }

    [Required]
    [MaxLength(30)]
    public override string CityName { get; set; }

    [Required]
    public override Guid CountryId { get; set; }

    #region implementing IEquatable
    public bool Equals(CityDbM other) => 
        (other != null) && ((CityName?.ToLower().Trim(), CountryId) == 
                            (other.CityName?.ToLower().Trim(), other.CountryId));

    public override bool Equals(object obj) => Equals(obj as CityDbM);
    public override int GetHashCode() => (CityName?.ToLower().Trim(), CountryId).GetHashCode();
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override ICountry Country 
    { 
        get => CountryDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(CountryId))]
    public virtual CountryDbM CountryDbM { get; set; }

    [NotMapped]
    public override List<IPostalCode> PostalCodes 
    { 
        get => PostalCodesDbM?.ToList<IPostalCode>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<PostalCodeDbM> PostalCodesDbM { get; set; } = new();
    #endregion

    #region constructors
    public CityDbM() : base() { }
    public CityDbM(City org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new CityDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}