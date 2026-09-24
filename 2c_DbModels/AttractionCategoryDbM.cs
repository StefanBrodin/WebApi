using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("AttractionCategory", Schema = "dbo")]
[PrimaryKey(nameof(AttractionId), nameof(CategoryId))]
public class AttractionCategoryDbM : AttractionCategory, ISeed<AttractionCategoryDbM>, IEquatable<AttractionCategoryDbM>
{
    [Required]
    public override Guid AttractionId { get; set; }

    [Required]
    public override Guid CategoryId { get; set; }

    #region implementing IEquatable
    public bool Equals(AttractionCategoryDbM other) =>
        (other != null) && ((AttractionId, CategoryId) == (other.AttractionId, other.CategoryId));

    public override bool Equals(object obj) => Equals(obj as AttractionCategoryDbM);
    public override int GetHashCode() => (AttractionId, CategoryId).GetHashCode();
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
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

    [NotMapped]
    public override ICategory Category 
    { 
        get => CategoryDbM; 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    [ForeignKey(nameof(CategoryId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public CategoryDbM CategoryDbM { get; set; }
    #endregion

    #region constructors
    public AttractionCategoryDbM() : base() { }
    public AttractionCategoryDbM(AttractionCategory org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new AttractionCategoryDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}