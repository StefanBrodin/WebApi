using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Customer", Schema = "dbo")]
public class CustomerDbM : Customer, ISeed<CustomerDbM>, IEquatable<CustomerDbM>
{
    [Key]
    public override Guid CustomerId { get; set; }

    [MaxLength(30)]
    public override string CustomerFirstName { get; set; }

    [MaxLength(30)]
    public override string CustomerLastName { get; set; }

    [MaxLength(30)]
    public override string CustomerUserName { get; set; }

    public override Guid? AddressId { get; set; }

    #region implementing IEquatable
    public bool Equals(CustomerDbM other) =>
        (other != null) && (CustomerUserName?.ToLower().Trim() == other.CustomerUserName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as CustomerDbM);
    public override int GetHashCode() => CustomerUserName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override IAddress Address 
    { 
        get => AddressDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(AddressId))]
    public virtual AddressDbM AddressDbM { get; set; }

    [NotMapped]
    public override List<ICustomerAttractionRating> CustomerAttractionRatings 
    { 
        get => CustomerAttractionRatingsDbM?.ToList<ICustomerAttractionRating>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<CustomerAttractionRatingDbM> CustomerAttractionRatingsDbM { get; set; } = new();
    #endregion

    #region constructors
    public CustomerDbM() : base() { }
    public CustomerDbM(Customer org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new CustomerDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}