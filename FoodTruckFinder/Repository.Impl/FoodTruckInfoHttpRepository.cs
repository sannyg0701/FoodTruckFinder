using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FoodTruckFinder.Repository.Impl
{
  /// <summary>
  /// An implementation of <see cref="IFoodTruckInfoRepository"/> that interacts with https://data.sfgov.org.
  /// </summary>
  public class FoodTruckInfoHttpRepository : IFoodTruckInfoRepository
  {
    private FoodTruckFinderConfig _foodtruckFinderConfig;
    private IHttpClientFactory _httpClientFactory;

    public FoodTruckInfoHttpRepository(FoodTruckFinderConfig foodtruckFinderConfig, IHttpClientFactory httpClientFactory)
    {
      _foodtruckFinderConfig = foodtruckFinderConfig ?? throw new ArgumentNullException(nameof(httpClientFactory));
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    /// <summary>
    /// Retrieves a <see cref="List{FoodTruckInfoHttpResponse}"/> from https://data.sfgov.org.
    /// </summary>
    /// <param name="latitude">The latitude of the location to retrieve the <see cref="List{FoodTruckInfoHttpResponse}"/> for.</param>
    /// <param name="longitude">The longitude of the location to retrieve the <see cref="List{FoodTruckInfoHttpResponse}"/> for.</param>
    /// <returns>
    ///   <list type="bullet">
    ///     <item>A <see cref="List{FoodTruckInfoHttpResponse}"/> for the given <paramref name="latitude"/> and <paramref name="longitude"/>.</item>
    ///     <item>NULL if the server either returns a malformed JSON response or an unsuccessful status code.</item>
    ///   </list>
    /// </returns>
    public async Task<List<FoodTruckInfoJsonResponse>> GetFoodTruckInfoJson(double latitude, double longitude)
    {
      using (var client = _httpClientFactory.CreateClient())
      {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var queryParams = new Dictionary<string, string>()
        {
          {"latitude", latitude.ToString() },
          {"longitude", longitude.ToString() }
        };

        var requestUri = QueryHelpers.AddQueryString(_foodtruckFinderConfig.Url, queryParams);
        var response = await client.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
          try
          {
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FoodTruckInfoJsonResponse>>(json);
          }
          catch(JsonSerializationException)
          {
            Console.WriteLine("Server returned a malformed JSON response.");
            return null;
          }
        }
        else
        {
          Console.WriteLine("Server returned a non-success response code.");
          return null;
        }
      }
    }
  }
}
