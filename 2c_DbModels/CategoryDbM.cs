using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Category", Schema = "dbo")]
public class CategoryDbM : Category, ISeed<CategoryDbM>, IEquatable<CategoryDbM>
{
    [Key]
    public override Guid CategoryId { get; set; }

    [Required]
    [MaxLength(30)]
    public override string CategoryName { get; set; }

    #region implementing IEquatable
    public bool Equals(CategoryDbM other) =>
        (other != null) && (CategoryName?.ToLower().Trim() == other.CategoryName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as CategoryDbM);
    public override int GetHashCode() => CategoryName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override List<IAttractionCategory> AttractionCategories 
    { 
        get => AttractionCategoriesDbM?.ToList<IAttractionCategory>(); 
        set => throw new NotImplementedException(); 
    }

    [JsonIgnore]
    public virtual List<AttractionCategoryDbM> AttractionCategoriesDbM { get; set; } = new();
    #endregion

    #region constructors
    public CategoryDbM() : base() { }
    public CategoryDbM(Category org) : base(org) { }
    #endregion

    #region randomly seed this instance
    public new CategoryDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    #endregion
}