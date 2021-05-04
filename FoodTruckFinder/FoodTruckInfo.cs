using System.Collections.Generic;
using FoodTruckFinder.Repository.Impl;

namespace FoodTruckFinder
{
  /// <summary>
  /// POCO that represents a human readable version of <see cref="FoodTruckInfoJsonResponse"/>
  /// </summary>
  public class FoodTruckInfo
  {
    public Location Location { get; set; }

    public string Name { get; set; }

    public List<string> Items { get; set; }
  }
}
