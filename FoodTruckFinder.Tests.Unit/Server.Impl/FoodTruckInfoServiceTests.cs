using FluentAssertions;
using FoodTruckFinder.Repository;
using FoodTruckFinder.Repository.Impl;
using FoodTruckFinder.Service.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckFinder.Tests.Unit.Server.Impl
{
  [TestClass]
  public class FoodTruckInfoServiceTests
  {
    private Mock<IFoodTruckInfoRepository> _foodTruckRepositoryMock = new Mock<IFoodTruckInfoRepository>();

    [TestMethod]
    public void ConstructorThrowsArgumentNullExceptionWhenFoodTruckInfoRepositoryIsNull()
    {
      FluentActions.Invoking(() => new FoodTruckInfoService(null)).Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public async Task GetFoodTruckInfoReturnsEmptyListWhenFoodTruckInfoJsonResponseIsNull()
    {
      _foodTruckRepositoryMock.Setup(_ => _.GetFoodTruckInfoJson(It.IsAny<double>(), It.IsAny<double>()))
        .ReturnsAsync((List<FoodTruckInfoJsonResponse>)null);

      var service = new FoodTruckInfoService(_foodTruckRepositoryMock.Object);
      var actual = await service.GetFoodTruckInfos(1, 2);

      actual.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFoodTruckInfoReturnsEmptyListWhenFoodTruckInfoJsonResponseIsEmpty()
    {
      _foodTruckRepositoryMock.Setup(_ => _.GetFoodTruckInfoJson(It.IsAny<double>(), It.IsAny<double>()))
        .ReturnsAsync(new List<FoodTruckInfoJsonResponse>());

      var service = new FoodTruckInfoService(_foodTruckRepositoryMock.Object);
      var actual = await service.GetFoodTruckInfos(1, 2);

      actual.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFoodTruckInfoReturnsFoodTruckInfos()
    {
      var foodTruckInfoJsonResponses = new List<FoodTruckInfoJsonResponse>
      {
        new FoodTruckInfoJsonResponse
        {
          ObjectId = "1",
          Address = "123 Fake Street",
          Applicant = "ABC Foods",
          FoodItems = "Pizza, Fries and Burgers.",
          LocationDescription = "Right across from 123 tower."
        },
        new FoodTruckInfoJsonResponse
        {
          ObjectId = "2",
          Address = "456 Fake Street",
          Applicant = "DEF Foods",
          FoodItems = "Gyros, Fries and Burgers.",
          LocationDescription = "Right across from 456 tower."
        }
      };

      var expected = new List<FoodTruckInfo>();
      foreach (var foodTruckInfoJsonResponse in foodTruckInfoJsonResponses)
      {
        expected.Add(new FoodTruckInfo
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

      _foodTruckRepositoryMock.Setup(_ => _.GetFoodTruckInfoJson(It.IsAny<double>(), It.IsAny<double>()))
        .ReturnsAsync(foodTruckInfoJsonResponses);

      var service = new FoodTruckInfoService(_foodTruckRepositoryMock.Object);
      var actual = await service.GetFoodTruckInfos(1, 2);

      actual.Should().BeEquivalentTo(expected);
    }
  }
}
