using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Attraction", Schema = "dbo")]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>, IEquatable<AttractionDbM>
{
    [Key]
    public override Guid AttractionId { get; set; }

    [Required]
    [MaxLength(30)]
    public override string AttractionName { get; set; }

    [MaxLength(4000)]
    public override string AttractionDescription { get; set; }

    [Required]
    public override Guid AddressId { get; set; }

    #region implementing IEquatable
    public bool Equals(AttractionDbM other) =>
        (other != null) && (AttractionName?.ToLower().Trim() == other.AttractionName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as AttractionDbM);
    public override int GetHashCode() => AttractionName?.ToLower().Trim().GetHashCode() ?? 0;
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
    public AddressDbM AddressDbM { get; set; }

    [NotMapped]
    public override List<ICustomerAttractionRating> CustomerAttractionRatings 
    { 
        get => CustomerAttractionRatingsDbM?.ToList<ICustomerAttractionRating>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public List<CustomerAttractionRatingDbM> CustomerAttractionRatingsDbM { get; set; } = new();

    [NotMapped]
    public override List<IAttractionCategory> AttractionCategories 
    { 
        get => AttractionCategoriesDbM?.ToList<IAttractionCategory>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public List<AttractionCategoryDbM> AttractionCategoriesDbM { get; set; } = new();
    #endregion

    #region constructors
    public AttractionDbM() : base() { }
    public AttractionDbM(Attraction org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new AttractionDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}