using FoodTruckFinder.Repository.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckFinder.Repository
{
    /// <summary>
    /// An interface that defines all of the outbound operations associated with a <see cref="FoodTruckInfo"/>. These outbound operations could represent either
    /// interacting with a persisted data store or retrieving data from a server.
    /// </summary>
    public interface IFoodTruckInfoRepository
    {
        /// <summary>
        /// Retrieves a <see cref="List{FoodTruckInfoJsonResponse}"/> from https://data.sfgov.org.
        /// </summary>
        /// <param name="latitude">The latitude of the location to retrieve the <see cref="List{FoodTruckInfoJsonResponse}"/> for.</param>
        /// <param name="longitude">The longitude of the location to retrieve the <see cref="List{FoodTruckInfoJsonResponse}"/> for.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item>A <see cref="List{FoodTruckInfoJsonResponse}"/> for the given <paramref name="latitude"/> and <paramref name="longitude"/>.</item>
        ///     <item>NULL if the server either returns a malformed JSON response or an unsuccessful status code.</item>
        ///   </list>
        /// </returns>
        Task<List<FoodTruckInfoJsonResponse>> GetFoodTruckInfoJson(double latitude, double longitude);
    }
}
