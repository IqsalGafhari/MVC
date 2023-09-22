namespace LINQ;

public class RegionAndCountryVM
{
    public string CountryId { get; set; }
    public string CountryName { get; set; }
    public string RegionName { get; set; }
    public int RegionId { get; set; }
    public string City { get; set; }

    public override string ToString()
    {
        return $"{CountryId} - {CountryName} - {RegionName} - {RegionId} - {City}";
    }
}
