using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Country", Schema = "dbo")]
public class CountryDbM : Country, ISeed<CountryDbM>, IEquatable<CountryDbM>
{
    [Key]
    public override Guid CountryId { get; set; }

    [Required]
    [MaxLength(30)]
    public override string CountryName { get; set; }

    #region implementing IEquatable
    public bool Equals(CountryDbM other) => 
        (other != null) && (CountryName?.ToLower().Trim() == other.CountryName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as CountryDbM);
    public override int GetHashCode() => CountryName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override List<ICity> Cities 
    { 
        get => CitiesDbM?.ToList<ICity>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<CityDbM> CitiesDbM { get; set; } = new();
    #endregion

    #region constructors
    public CountryDbM() : base() { }
    public CountryDbM(Country org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new CountryDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}