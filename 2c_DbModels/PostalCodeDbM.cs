using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("PostalCode", Schema = "dbo")]
public class PostalCodeDbM : PostalCode, ISeed<PostalCodeDbM>, IEquatable<PostalCodeDbM>
{
    [Key]
    public override Guid PostalCodeId { get; set; }

    [Required]
    [Column("PostalCode", TypeName = "nchar(10)")]
    [MaxLength(10)]
    public override string PostalCodeNumber { get; set; }

    [Required]
    public override Guid CityId { get; set; }

    #region implementing IEquatable
    public bool Equals(PostalCodeDbM other) =>
        (other != null) && ((PostalCodeNumber?.Trim(), CityId) == 
                            (other.PostalCodeNumber?.Trim(), other.CityId));

    public override bool Equals(object obj) => Equals(obj as PostalCodeDbM);
    public override int GetHashCode() => (PostalCodeNumber?.Trim(), CityId).GetHashCode();
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override ICity City 
    { 
        get => CityDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(CityId))]
    public virtual CityDbM CityDbM { get; set; }

    [NotMapped]
    public override List<IAddress> Addresses 
    { 
        get => AddressesDbM?.ToList<IAddress>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<AddressDbM> AddressesDbM { get; set; } = new();
    #endregion

    #region constructors
    public PostalCodeDbM() : base() { }
    public PostalCodeDbM(PostalCode org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new PostalCodeDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}