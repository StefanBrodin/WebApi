namespace Models.DTO;

public class AttractionWithoutReviewsDto
{
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public bool Seeded { get; set; }
    public string CountryName { get; set; }
    public string CityName { get; set; }
    public string FullStreetAddress { get; set; }
}


