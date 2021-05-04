using FoodTruckFinder.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FoodTruckFinder
{
  public class Program
  {
    static async Task Main(string[] args)
    {
      // Build the Ioc container.
      var services = new ServiceCollection();
      var startup = new Startup();
      startup.ConfigureServices(services);

      var serviceProvider = services.BuildServiceProvider();
      var foodTruckInfoService = serviceProvider.GetRequiredService<IFoodTruckInfoService>();

      int foodTruckInfosCount = 0;
      while (foodTruckInfosCount < 5)
      {
        Console.WriteLine("Please enter a latitude and longitude to discover food trucks in the area.");
        string latitudeStr = Console.ReadLine();
        string longitudeStr = Console.ReadLine();

        double latitude;
        double longitude;
        while (!double.TryParse(latitudeStr, out latitude) || !double.TryParse(longitudeStr, out longitude))
        {
          Console.WriteLine("Please enter a valid latitude and longitude to discover food trucks in the area.");
          latitudeStr = Console.ReadLine();
          longitudeStr = Console.ReadLine();
        }

        var foodTruckInfos = await foodTruckInfoService.GetFoodTruckInfos(latitude, longitude);
        Console.WriteLine("\n");
        foreach (var foodTruckInfo in foodTruckInfos)
        {
          Console.WriteLine($"Food truck {foodTruckInfo.Name} is located at {foodTruckInfo.Location.Address} and has {string.Join(",", foodTruckInfo.Items)} on the menu.");
        }

        foodTruckInfosCount = foodTruckInfos.Count;
        if(foodTruckInfosCount < 5)
        {
          Console.WriteLine("\n");
          Console.WriteLine("Less than 5 food trucks were found in the specified area. Would you like to expand your search?[y/n]");
          switch (Console.ReadKey(false).Key)
          {
            case ConsoleKey.Y:
              Console.WriteLine("\n");
              continue;
            case ConsoleKey.N:
              foodTruckInfosCount = int.MaxValue;
              break;
            default:
              continue;
          }
        }
      }

      Console.WriteLine("\n");
      Console.WriteLine("Enjoy....Press any key to exit.");
      Console.ReadKey();
    }
  }
}
