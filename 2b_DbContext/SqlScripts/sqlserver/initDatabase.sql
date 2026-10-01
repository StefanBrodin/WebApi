-- View: Attractions without any reviews/ratings
CREATE OR ALTER VIEW supusr.vw_Attractions_Without_Reviews
AS
SELECT 
    a.AttractionId,
    a.AttractionName,
    a.AttractionDescription,
    a.Seeded,
    c.CountryName,
    ci.CityName,
    CONCAT(ad.StreetName, ' ', ISNULL(ad.StreetNumber, '')) AS FullStreetAddress
FROM supusr.Attraction a
INNER JOIN supusr.Address ad ON ad.AddressId = a.AddressId
INNER JOIN supusr.PostalCode pc ON pc.PostalCodeId = ad.PostalCodeId
INNER JOIN supusr.City ci ON ci.CityId = pc.CityId
INNER JOIN supusr.Country c ON c.CountryId = ci.CountryId
WHERE NOT EXISTS (
    SELECT 1 
    FROM supusr.CustomerAttractionRating r 
    WHERE r.AttractionId = a.AttractionId
);
GO


-- View: Database overview (Number of Users, Cities, and Attractions)
CREATE OR ALTER VIEW supusr.vw_Database_Overview
AS
SELECT 
    (SELECT COUNT(*) FROM supusr.Customer) AS NrCustomers,
    (SELECT COUNT(*) FROM supusr.City) AS NrCities,
    (SELECT COUNT(*) FROM supusr.Attraction) AS NrAttractions;
GO


-- Stored Procedure: Remove seeded (or non-seeded) data
CREATE OR ALTER PROCEDURE supusr.sp_RemoveSeed
    @seeded BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- Raderar i bakvänd beroendeordning för att undvika FK-konflikter
    DELETE FROM supusr.CustomerAttractionRating WHERE Seeded = @seeded;
    DELETE FROM supusr.AttractionCategory WHERE Seeded = @seeded;
    DELETE FROM supusr.Attraction WHERE Seeded = @seeded;
    DELETE FROM supusr.Customer WHERE Seeded = @seeded;
    DELETE FROM supusr.Category WHERE Seeded = @seeded;
    DELETE FROM supusr.Address WHERE Seeded = @seeded;
    DELETE FROM supusr.PostalCode WHERE Seeded = @seeded;
    DELETE FROM supusr.City WHERE Seeded = @seeded;
    DELETE FROM supusr.Country WHERE Seeded = @seeded;
END;
GO

