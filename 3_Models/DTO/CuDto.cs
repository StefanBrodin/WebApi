using System.Text.RegularExpressions;
using Models;

namespace Models.DTO;

// DTO is a DataTransferObject, instantiated by the controller logic
// and represents a fully instantiable subset of the models for Create and Update operations.

public class CustomerCuDto
{
    public virtual Guid? CustomerId { get; set; } = null;

    public virtual string CustomerFirstName { get; set; }
    public virtual string CustomerLastName { get; set; }
    public virtual string CustomerUserName { get; set; } // Must be a valid email

    // Navigation property as ID
    public virtual Guid? AddressId { get; set; } = null;

    public CustomerCuDto() { }

    public CustomerCuDto(ICustomer org)
    {
        CustomerId = org.CustomerId;
        CustomerFirstName = org.CustomerFirstName;
        CustomerLastName = org.CustomerLastName;
        CustomerUserName = org.CustomerUserName;

        AddressId = org.Address?.AddressId;
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrWhiteSpace(CustomerFirstName))
            throw new ArgumentException("CustomerFirstName cannot be empty.");

        if (string.IsNullOrWhiteSpace(CustomerLastName))
            throw new ArgumentException("CustomerLastName cannot be empty.");

        if (string.IsNullOrWhiteSpace(CustomerUserName) || !Regex.IsMatch(CustomerUserName, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("CustomerUserName must be a valid email address.");
    }
}

public class AttractionCuDto
{
    public virtual Guid? AttractionId { get; set; } = null;

    public virtual string AttractionName { get; set; }
    public virtual string AttractionDescription { get; set; }

    // Navigation properties as IDs
    public virtual Guid? AddressId { get; set; } = null;
    public virtual List<Guid> CategoryIds { get; set; } = new List<Guid>();

    public AttractionCuDto() { }

    public AttractionCuDto(IAttraction org)
    {
        AttractionId = org.AttractionId;
        AttractionName = org.AttractionName;
        AttractionDescription = org.AttractionDescription;

        AddressId = org.Address?.AddressId;
        CategoryIds = org.AttractionCategories?.Select(c => c.CategoryId).ToList() ?? new List<Guid>();
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrWhiteSpace(AttractionName))
            throw new ArgumentException("AttractionName cannot be empty.");

        if (string.IsNullOrWhiteSpace(AttractionDescription))
            throw new ArgumentException("AttractionDescription cannot be empty.");
    }
}

public class CustomerAttractionRatingCuDto
{
    public virtual Guid CustomerId { get; set; }
    public virtual Guid AttractionId { get; set; }

    public virtual byte? RatingScore { get; set; } = null;
    public virtual string RatingReview { get; set; } = null;

    public CustomerAttractionRatingCuDto() { }

    public CustomerAttractionRatingCuDto(ICustomerAttractionRating org)
    {
        CustomerId = org.CustomerId;
        AttractionId = org.AttractionId;
        RatingScore = org.RatingScore;
        RatingReview = org.RatingReview;
    }

    public void EnsureValidity()
    {
        if (CustomerId == Guid.Empty)
            throw new ArgumentException("CustomerId must be specified.");

        if (AttractionId == Guid.Empty)
            throw new ArgumentException("AttractionId must be specified.");

        if (RatingScore.HasValue && (RatingScore.Value < 1 || RatingScore.Value > 5))
            throw new ArgumentException("RatingScore must be between 1 and 5.");

        if (!RatingScore.HasValue && string.IsNullOrWhiteSpace(RatingReview))
            throw new ArgumentException("Either RatingScore or RatingReview must be provided.");
    }
}

