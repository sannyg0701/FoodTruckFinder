using FoodTruckFinder.Repository;
using FoodTruckFinder.Repository.Impl;
using FoodTruckFinder.Service;
using FoodTruckFinder.Service.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodTruckFinder
{
  /// <summary>
  /// Class that builds the IoC container for the service.
  /// </summary>
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup()
    {
      Configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpClient();

      services.AddSingleton(Configuration.GetSection(nameof(FoodTruckFinderConfig)).Get<FoodTruckFinderConfig>());
      services.AddSingleton<IFoodTruckInfoRepository, FoodTruckInfoHttpRepository>();
      services.AddSingleton<IFoodTruckInfoService, FoodTruckInfoService>();
    }
  }
}
