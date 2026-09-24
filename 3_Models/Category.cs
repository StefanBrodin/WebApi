using Seido.Utilities.SeedGenerator;

namespace Models;

public class Category : ICategory, ISeed<Category>, IEquatable<Category>
{
    public virtual Guid CategoryId { get; set; }
    public virtual string CategoryName { get; set; }

    // Model relationships
    // Many Categories can be linked to Many Attractions (via AttractionCategory)
    public virtual List<IAttractionCategory> AttractionCategories { get; set; } = null;

    #region constructors
    public Category() { }

    public Category(Category org)
    {
        this.CategoryId = org.CategoryId;
        this.CategoryName = org.CategoryName;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(Category other) =>
        (other != null) && (this.CategoryName?.ToLower().Trim() == other.CategoryName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as Category);
    public override int GetHashCode() => CategoryName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    private const string _categories = 
        "Museum, Landmark, Historical Site, Religious Site, Cathedral, Temple, Castle, " +
        "Bridge, Observation Tower, Architectural Wonder, Memorial, Park, Square, Iconic Viewpoint";

    public Category Seed(SeedGenerator seeder)
    {
        Seeded = true;
        CategoryId = Guid.NewGuid();

        CategoryName = seeder.FromString(_categories);

        return this;
    }
    #endregion
}