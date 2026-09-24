using Seido.Utilities.SeedGenerator;

namespace Models;

public class AttractionCategory : IAttractionCategory, ISeed<AttractionCategory>, IEquatable<AttractionCategory>
{
    public virtual Guid AttractionId { get; set; }
    public virtual Guid CategoryId { get; set; }

    // Model relationships
    // Belongs to one Attraction
    public virtual IAttraction Attraction { get; set; } = null;

    // Belongs to one Category
    public virtual ICategory Category { get; set; } = null;

    #region constructors
    public AttractionCategory() { }

    public AttractionCategory(AttractionCategory org)
    {
        this.AttractionId = org.AttractionId;
        this.CategoryId = org.CategoryId;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(AttractionCategory other) =>
        (other != null) && ((this.AttractionId, this.CategoryId) == (other.AttractionId, other.CategoryId));

    public override bool Equals(object obj) => Equals(obj as AttractionCategory);
    public override int GetHashCode() => (AttractionId, CategoryId).GetHashCode();
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    public AttractionCategory Seed(SeedGenerator seeder)
    {
        Seeded = true;

        // foreign keys will be assigned later

        return this;
    }
    #endregion
}