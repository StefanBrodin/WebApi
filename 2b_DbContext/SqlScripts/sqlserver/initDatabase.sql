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



