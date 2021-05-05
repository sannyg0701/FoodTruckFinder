using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using System;
using FoodTruckFinder.Repository.Impl;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace FoodTruckFinder.Tests.Unit.Repository.Impl
{
    [TestClass]
    public class FoodTruckInfoHttpRepositoryTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<DelegatingHandler> _clientHandlerMock = new Mock<DelegatingHandler>();

        [TestMethod]
        public void ConstructorThrowsArgumentNullExceptionWhenConfigurationIsNull()
        {
            FluentActions.Invoking(() => new FoodTruckInfoHttpRepository(null, _httpClientFactoryMock.Object)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorThrowsArgumentNullExceptionWhenClientFactoryIsNull()
        {
            FluentActions.Invoking(() => new FoodTruckInfoHttpRepository(new FoodTruckFinderConfig(), null)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetFoodTruckInfoJsonReturnsFoodTruckInfoJsonResponse()
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
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(foodTruckInfoJsonResponses), Encoding.UTF8, "application/json")
            };

            SetupHttpClient(response);

            var repository = new FoodTruckInfoHttpRepository(new FoodTruckFinderConfig { Url = "https://www.xyz.com/" }, _httpClientFactoryMock.Object);

            var actual = await repository.GetFoodTruckInfoJson(1, 2);

            actual.Should().BeEquivalentTo(foodTruckInfoJsonResponses);

            var queryParams = new Dictionary<string, string>()
      {
        {"latitude", "1" },
        {"longitude", "2"}
      };

            var requestUri = QueryHelpers.AddQueryString("https://www.xyz.com/", queryParams);

            _clientHandlerMock.Protected().Verify(
              "SendAsync",
              Times.Exactly(1),
              ItExpr.Is<HttpRequestMessage>(x =>
              x.Headers.GetValues("Accept").FirstOrDefault() == "application/json" &&
              x.Method == HttpMethod.Get &&
              x.RequestUri.ToString() == requestUri
           ), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task GetFoodTruckInfoJsonReturnsNull()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject("malformed_json"), Encoding.UTF8, "application/json")
            };

            SetupHttpClient(response);

            var repository = new FoodTruckInfoHttpRepository(new FoodTruckFinderConfig { Url = "https://www.xyz.com/" }, _httpClientFactoryMock.Object);

            var actual = await repository.GetFoodTruckInfoJson(1, 2);

            actual.Should().BeNull();

            var queryParams = new Dictionary<string, string>()
      {
        {"latitude", "1" },
        {"longitude", "2"}
      };

            var requestUri = QueryHelpers.AddQueryString("https://www.xyz.com/", queryParams);

            _clientHandlerMock.Protected().Verify(
              "SendAsync",
              Times.Exactly(1),
              ItExpr.Is<HttpRequestMessage>(x =>
              x.Headers.GetValues("Accept").FirstOrDefault() == "application/json" &&
              x.Method == HttpMethod.Get &&
              x.RequestUri.ToString() == requestUri
           ), ItExpr.IsAny<CancellationToken>());
        }

        private void SetupHttpClient(HttpResponseMessage response)
        {
            _clientHandlerMock
             .Protected()
             .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
             .ReturnsAsync(response);

            var httpClient = new HttpClient(_clientHandlerMock.Object);

            _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }
    }
}
