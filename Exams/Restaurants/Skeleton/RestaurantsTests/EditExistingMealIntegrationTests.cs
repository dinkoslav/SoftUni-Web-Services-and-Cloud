using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restaurants.Data;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Testing;
using System.Net.Http;
using System.Web.Http;
using Owin;
using Restaurants.Services;
using Restaurants.Models;

namespace RestaurantsTests
{
    [TestClass]
    public class EditExistingMealIntegrationTests
    {
        private TestServer httpTestServer;
        private HttpClient httpClient;

        [TestInitialize]
        public void TestInit()
        {
            // Start OWIN testing HTTP server with Web API support
            this.httpTestServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                appBuilder.UseWebApi(config);
            });
            this.httpClient = httpTestServer.HttpClient;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.httpTestServer.Dispose();
        }

        [TestMethod]
        public void EditExistingMeal_ShouldReturn200OK()
        {
            // Arrange
            RestaurantsContext dbContext = new RestaurantsContext();
            var restaurant = dbContext.Restaurants.FirstOrDefault();

            Meal meal = new Meal()
            {
                Name = "Tarator",
                Price = (decimal)1.0,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id,
                Type = dbContext.MealTypes.Find(1),
                TypeId = 1
            };

            dbContext.Meals.Add(meal);
            dbContext.SaveChanges();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Shopska"),
                new KeyValuePair<string, string>("typeId", "3"),
                new KeyValuePair<string, string>("price", "1.1")
            });
            // Act
            var httpResponse = httpClient.PutAsync("/api/meals/" + meal.Id, content).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.AreEqual(httpResponse.Content.Headers.ContentType.MediaType, "application/json");
        }

        [TestMethod]
        public void EditNonExistingMeal_ShouldReturn404NotFound()
        {
            // Arrange
            RestaurantsContext dbContext = new RestaurantsContext();
            var restaurant = dbContext.Restaurants.FirstOrDefault();

            Meal meal = new Meal()
            {
                Name = "Tarator",
                Price = (decimal)1.0,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id,
                Type = dbContext.MealTypes.Find(1),
                TypeId = 1
            };

            dbContext.Meals.Add(meal);
            dbContext.SaveChanges();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Shopska"),
                new KeyValuePair<string, string>("typeId", "3"),
                new KeyValuePair<string, string>("price", "1.1")
            });
            // Act
            var httpResponse = httpClient.PutAsync("/api/meals/" + meal.Id + 1, content).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.AreEqual(httpResponse.Content.Headers.ContentType.MediaType, "application/json");
        }

        [TestMethod]
        public void EditExistingMealWrongParameters_ShouldReturn400BadRequest()
        {
            // Arrange
            RestaurantsContext dbContext = new RestaurantsContext();
            var restaurant = dbContext.Restaurants.FirstOrDefault();

            Meal meal = new Meal()
            {
                Name = "Tarator",
                Price = (decimal)1.0,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id,
                Type = dbContext.MealTypes.Find(1),
                TypeId = 1
            };

            dbContext.Meals.Add(meal);
            dbContext.SaveChanges();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "Shopska"),
                new KeyValuePair<string, string>("typeId", "3")
            });
            // Act
            var httpResponse = httpClient.PutAsync("/api/meals/" + meal.Id, content).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.AreEqual(httpResponse.Content.Headers.ContentType.MediaType, "application/json");
        }
    }
}
