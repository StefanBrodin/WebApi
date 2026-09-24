using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("CustomerAttractionRating", Schema = "dbo")]
[PrimaryKey(nameof(CustomerId), nameof(AttractionId))]
public class CustomerAttractionRatingDbM : CustomerAttractionRating, ISeed<CustomerAttractionRatingDbM>, IEquatable<CustomerAttractionRatingDbM>
{
    [Required]
    public override Guid CustomerId { get; set; }

    [Required]
    public override Guid AttractionId { get; set; }

    public override byte? RatingScore { get; set; }

    [MaxLength(500)]
    public override string RatingReview { get; set; }

    [Required]
    public override DateTime RatingTimestamp { get; set; }

    #region implementing IEquatable
    public bool Equals(CustomerAttractionRatingDbM other) =>
        (other != null) && ((CustomerId, AttractionId) == (other.CustomerId, other.AttractionId));

    public override bool Equals(object obj) => Equals(obj as CustomerAttractionRatingDbM);
    public override int GetHashCode() => (CustomerId, AttractionId).GetHashCode();
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override ICustomer Customer 
    { 
        get => CustomerDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(CustomerId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public CustomerDbM CustomerDbM { get; set; }

    [NotMapped]
    public override IAttraction Attraction 
    { 
        get => AttractionDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(AttractionId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public AttractionDbM AttractionDbM { get; set; }
    #endregion

    #region constructors
    public CustomerAttractionRatingDbM() : base() { }
    public CustomerAttractionRatingDbM(CustomerAttractionRating org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new CustomerAttractionRatingDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}