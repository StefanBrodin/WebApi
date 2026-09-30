namespace Models.DTO;

public class AttractionListItemDto
{
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    
    // Location information
    public string CityName { get; set; }
    public string CountryName { get; set; }

    // Categories associated with the attraction
    public List<string> Categories { get; set; } = new();

    // Average rating and number of reviews 
    public double? AverageRating { get; set; }
    public int NumberOfRatings { get; set; }
}