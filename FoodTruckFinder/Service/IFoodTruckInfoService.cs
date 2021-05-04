using System.Collections.Generic;
using System.Threading.Tasks;
using FoodTruckFinder.Repository;

namespace FoodTruckFinder.Service
{
  /// <summary>
  /// An interface that defines all of the business logic associated with <see cref="FoodTruckInfo"/>.
  /// </summary>
  public interface IFoodTruckInfoService
  {
    /// <summary>
    /// Retrieves a <see cref="List{FoodTruckInfo}"/>.
    /// </summary>
    /// <param name="latitude">The latitude of the location to retrieve the <see cref="List{FoodTruckInfo}"/> for.</param>
    /// <param name="longitude">The longitude of the location to retrieve the <see cref="List{FoodTruckInfo}"/> for.</param>
    /// <returns>
    /// A <see cref="List{FoodTruckInfo}"/> for the given <paramref name="latitude"/> and <paramref name="longitude"/>
    /// </returns>
    /// <remarks>Calls out to <see cref="IFoodTruckInfoRepository"/> </remarks>
    Task<List<FoodTruckInfo>> GetFoodTruckInfos(double latitude, double longitude);
  }
}
