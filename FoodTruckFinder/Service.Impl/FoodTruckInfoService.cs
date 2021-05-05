using FoodTruckFinder.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckFinder.Service.Impl
{
    /// <summary>
    /// An implementation of <see cref="IFoodTruckInfoService"/> that interacts with <see cref="IFoodTruckInfoRepository"/>
    /// </summary>
    public class FoodTruckInfoService : IFoodTruckInfoService
    {
        private IFoodTruckInfoRepository _foodTruckInfoHttpRepository;

        public FoodTruckInfoService(IFoodTruckInfoRepository foodTruckHttpRepository)
        {
            _foodTruckInfoHttpRepository = foodTruckHttpRepository ?? throw new ArgumentNullException(nameof(foodTruckHttpRepository));
        }

        /// <summary>
        /// Retrieves a <see cref="List{FoodTruckInfo}"/>.
        /// </summary>
        /// <param name="latitude">The latitude of the location to retrieve the <see cref="List{FoodTruckInfo}"/> for.</param>
        /// <param name="longitude">The longitude of the location to retrieve the <see cref="List{FoodTruckInfo}"/> for.</param>
        /// <returns>
        /// A <see cref="List{FoodTruckInfo}"/> for the given <paramref name="latitude"/> and <paramref name="longitude"/>
        /// </returns>
        public async Task<List<FoodTruckInfo>> GetFoodTruckInfos(double latitude, double longitude)
        {
            var foodTruckInfos = new List<FoodTruckInfo>();
            var foodTruckInfoJsonResponses = await _foodTruckInfoHttpRepository.GetFoodTruckInfoJson(latitude, longitude);

            if (foodTruckInfoJsonResponses == null || !foodTruckInfoJsonResponses.Any())
            {
                Console.WriteLine("No food trucks found in the specified area. Please try expanding your search.");
                return foodTruckInfos;
            }

            foreach (var foodTruckInfoJsonResponse in foodTruckInfoJsonResponses)
            {
                foodTruckInfos.Add(new FoodTruckInfo
                {
                    Items = foodTruckInfoJsonResponse.FoodItems.Split(":").ToList(),
                    Name = foodTruckInfoJsonResponse.Applicant,
                    Location = new Location
                    {
                        Address = foodTruckInfoJsonResponse.Address,
                        Description = foodTruckInfoJsonResponse.LocationDescription
                    }
                });
            }

            return foodTruckInfos;
        }
    }
}
