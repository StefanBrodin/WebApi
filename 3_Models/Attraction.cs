using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>, IEquatable<Attraction>
{
    public virtual Guid AttractionId { get; set; }
    public virtual string AttractionName { get; set; }
    public virtual string AttractionDescription { get; set; }

    public virtual Guid AddressId { get; set; }

    // Model relationships
    // Many Attractions belong to one Address
    public virtual IAddress Address { get; set; } = null;

    // One Attraction may have many Ratings/Reviews
    public virtual List<ICustomerAttractionRating> CustomerAttractionRatings { get; set; } = null;

    // One Attraction may have many Categories (via AttractionCategory)
    public virtual List<IAttractionCategory> AttractionCategories { get; set; } = null;

    #region constructors
    public Attraction() { }

    public Attraction(Attraction org)
    {
        this.AttractionId = org.AttractionId;
        this.AttractionName = org.AttractionName;
        this.AttractionDescription = org.AttractionDescription;
        this.AddressId = org.AddressId;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(Attraction other) =>
        (other != null) && (this.AttractionName?.ToLower().Trim() == other.AttractionName?.ToLower().Trim());

    public override bool Equals(object obj) => Equals(obj as Attraction);
    public override int GetHashCode() => AttractionName?.ToLower().Trim().GetHashCode() ?? 0;
    #endregion

    #region Seeder
    public bool Seeded { get; set; } = false;

    private static readonly (string Name, string Description)[] _sampleAttractions = new[]
    {
        ("Eiffel Tower", "Iconic iron lattice tower and symbol of Paris."),
        ("Louvre Museum", "World's largest art museum, home of the Mona Lisa."),
        ("Promenade des Anglais", "Famous scenic waterfront promenade in Nice."),
        ("Vasamuseet", "Museum housing the preserved 17th-century warship Vasa."),
        ("Kungliga Slottet", "Official residence of the Swedish royal family."),
        ("Gävlebocken", "Famous giant straw goat in Gävle, a well-known Christmas tradition."),
        ("Colosseum", "Iconic ancient Roman amphitheatre."),
        ("St. Mark's Basilica", "Iconic Byzantine cathedral in Venice."),
        ("Florence Cathedral", "Famous Renaissance cathedral with Brunelleschi's dome."),
        ("Pantheon", "Best-preserved ancient Roman building with massive dome."),
        ("La Sagrada Família", "Antoni Gaudí's iconic unfinished basilica."),
        ("Park Güell", "Whimsical Gaudí park with colorful mosaics and city views."),
        ("Prado Museum", "One of the world's greatest art museums."),
        ("British Museum", "World-famous museum of history and human culture."),
        ("Tower of London", "Historic castle, fortress and former royal prison."),
        ("London Eye", "Giant Ferris wheel with panoramic views of London."),
        ("Edinburgh Castle", "Historic fortress overlooking the city of Edinburgh."),
        ("Statue of Liberty", "Iconic symbol of freedom on Liberty Island."),
        ("Empire State Building", "Legendary Art Deco skyscraper in New York."),
        ("Golden Gate Bridge", "Famous suspension bridge and symbol of San Francisco."),
        ("The Strip", "World-famous boulevard packed with casinos and entertainment."),
        ("Brandenburg Gate", "Iconic neoclassical monument in Berlin."),
        ("Reichstag Building", "Home of the German Parliament with its famous glass dome."),
        ("Marienplatz", "Central square in Munich famous for the Glockenspiel."),
        ("Acropolis", "Ancient citadel and symbol of classical Greece."),
        ("Parthenon", "Famous ancient Greek temple on the Acropolis."),
        ("Oia Village", "Picturesque white village famous for stunning sunsets."),
        ("Senso-ji Temple", "Tokyo's oldest and most visited Buddhist temple."),
        ("Tokyo Skytree", "Tallest tower in Japan with observation decks."),
        ("Hiroshima Peace Memorial", "Symbol of the atomic bombing and peace."),
        ("Helsinki Cathedral", "Iconic white neoclassical cathedral in Helsinki.")
    };

    public Attraction Seed(SeedGenerator seeder)
    {
        Seeded = true;
        AttractionId = Guid.NewGuid();

        var sample = _sampleAttractions[seeder.Next(0, _sampleAttractions.Length)];
        AttractionName = sample.Name;
        AttractionDescription = sample.Description;

        return this;
    }
    #endregion
}