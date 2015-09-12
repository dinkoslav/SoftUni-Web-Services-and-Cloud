using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.ViewModels;
using Restaurants.Services.BindingModels;
using Restaurants.Data.UnitOfWork;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/Restaurants")]
    public class RestaurantsController : ApiController
    {
        //private RestaurantsContext db = new RestaurantsContext();

        private readonly IRestaurantsData db;

        public RestaurantsController()
            : this(new RestaurantsData())
        {
        }

        public RestaurantsController(RestaurantsData data)
        {
            this.db = data;
        }

        // GET: api/Restaurants
        public IHttpActionResult GetRestaurantsByTown(string townId)
        {
            int town = 0;
            int.TryParse(townId, out town);

            IQueryable<Restaurant> restaurants = db.Restaurants
                .All()
                .OrderBy(r => r.Name)
                .ThenByDescending(r => r.Ratings.Average(rat => rat.Stars))
                .Where(r => r.TownId == town);

            return this.Ok(
                restaurants.Select(r => new RestaurantViewModel()
            {
                Id = r.Id,
                Name = r.Name,
                Rating = r.Ratings.Count >= 0 ? (int?)r.Ratings.Average(rat => rat.Stars) : null,
                Town = new TownViewModel()
                {
                    Id = r.Town.Id,
                    Name = r.Town.Name
                }
            }));
        }

        // GET: api/Restaurants
        [Route("{id:int}/meals")]
        public IHttpActionResult GetRestaurantMeals(int id)
        {
            if (!db.Restaurants.All().Any(r => r.Id == id))
            {
                return this.NotFound();
            }

            Restaurant restaurant = db.Restaurants.Find(id);

            return this.Ok(
                restaurant.Meals
                .OrderBy(m => m.Type.Order)
                .ThenBy(m => m.Name)
                .Select(m => new MealViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    Type = m.Type.Name
                }));
        }

        // POST: api/Restaurants
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult PostRestaurant(RestaurantBindingModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);

            Restaurant newRestaurant = new Restaurant()
            {
                Name = restaurant.Name,
                Town = db.Towns.Find(int.Parse(restaurant.TownId)),
                TownId = int.Parse(restaurant.TownId),
                Owner = currentUser,
                OwnerId = currentUserId
            };

            db.Restaurants.Add(newRestaurant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi",
                new { controller = "restaurants", id = newRestaurant.Id },
                new RestaurantViewModel()
            {
                Id = newRestaurant.Id,
                Name = newRestaurant.Name,
                Rating = null,
                Town = new TownViewModel()
                {
                    Id = newRestaurant.Town.Id,
                    Name = newRestaurant.Town.Name
                }
            });
        }

        // POST: api/Restaurants/{id}/rate
        [Route("{id:int}/rate")]
        public IHttpActionResult PostRestaurant(int id, RestaurantRateBindingModel restaurantRating)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            int stars = int.Parse(restaurantRating.Stars);

            if (stars < 0 || stars > 10)
            {
                return this.BadRequest();
            }

            if (db.Restaurants.Find(id) == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.db.Users.Find(currentUserId);

            if (db.Ratings.All().Any(r => r.RestaurantId == id && r.UserId == currentUserId))
            {
                return this.BadRequest();
            }

            if (db.Restaurants.Find(id).OwnerId == currentUserId)
            {
                return this.BadRequest();
            }

            Rating rating = new Rating()
            {
                Stars = stars,
                Restaurant = db.Restaurants.Find(id),
                RestaurantId = id,
                User = currentUser,
                UserId = currentUserId
            };

            db.Ratings.Add(rating);
            db.SaveChanges();

            return this.Ok();

        }
    }
}