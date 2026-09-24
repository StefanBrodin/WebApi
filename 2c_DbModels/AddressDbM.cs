using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Address", Schema = "dbo")]
public class AddressDbM : Address, ISeed<AddressDbM>, IEquatable<AddressDbM>
{
    [Key]
    public override Guid AddressId { get; set; }

    [Required]
    [MaxLength(50)]
    public override string StreetName { get; set; }

    [MaxLength(10)]
    public override string StreetNumber { get; set; }

    [Required]
    public override Guid PostalCodeId { get; set; }

    #region implementing IEquatable
    public bool Equals(AddressDbM other) =>
        (other != null) && ((StreetName?.ToLower().Trim(), StreetNumber?.ToLower().Trim(), PostalCodeId) == 
                            (other.StreetName?.ToLower().Trim(), other.StreetNumber?.ToLower().Trim(), other.PostalCodeId));

    public override bool Equals(object obj) => Equals(obj as AddressDbM);
    public override int GetHashCode() => (StreetName?.ToLower().Trim(), StreetNumber?.ToLower().Trim(), PostalCodeId).GetHashCode();
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override IPostalCode PostalCode 
    { 
        get => PostalCodeDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(PostalCodeId))]
    public virtual PostalCodeDbM PostalCodeDbM { get; set; }

    [NotMapped]
    public override List<ICustomer> Customers 
    { 
        get => CustomersDbM?.ToList<ICustomer>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<CustomerDbM> CustomersDbM { get; set; } = new();

    [NotMapped]
    public override List<IAttraction> Attractions 
    { 
        get => AttractionsDbM?.ToList<IAttraction>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<AttractionDbM> AttractionsDbM { get; set; } = new();
    #endregion

    #region constructors
    public AddressDbM() : base() { }
    public AddressDbM(Address org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new AddressDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}