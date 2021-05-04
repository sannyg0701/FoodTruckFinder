namespace FoodTruckFinder.Repository.Impl
{
  /// <summary>
  /// POCO that represents the JSON response retrieved from  https://data.sfgov.org.
  /// </summary>
  public class FoodTruckInfoJsonResponse
  {
    public string ObjectId { get; set; }

    public string Applicant { get; set; }

    public string Address { get; set; }

    public string FoodItems { get; set; }

    public string LocationDescription { get; set; }
  }
}
