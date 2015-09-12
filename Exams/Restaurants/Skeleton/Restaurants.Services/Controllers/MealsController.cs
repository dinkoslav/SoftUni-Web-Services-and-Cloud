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
using Restaurants.Services.BindingModels;
using Restaurants.Services.ViewModels;
using Restaurants.Data.UnitOfWork;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/meals")]
    public class MealsController : ApiController
    {
        //private RestaurantsContext db = new RestaurantsContext();

        private readonly IRestaurantsData db;

        public MealsController()
            : this(new RestaurantsData())
        {
        }

        public MealsController(RestaurantsData data)
        {
            this.db = data;
        }

        // POST: api/Meals
        [ResponseType(typeof(Meal))]
        public IHttpActionResult PostMeal(MealBindingModel mealModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!db.Restaurants.All().Any(r => r.Id == mealModel.RestaurantId))
            {
                return BadRequest();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            Meal newMeal = new Meal()
            {
                Name = mealModel.Name,
                Price = mealModel.Price,
                Restaurant = db.Restaurants.Find(mealModel.RestaurantId),
                RestaurantId = mealModel.RestaurantId,
                Type = db.MealTypes.Find(mealModel.TypeId),
                TypeId = mealModel.TypeId
            };

            db.Meals.Add(newMeal);
            db.SaveChanges();

            return CreatedAtRoute(
                "DefaultApi", 
                new { id = newMeal.Id }, 
                new MealViewModel()
                {
                    Id = newMeal.Id,
                    Name = newMeal.Name,
                    Price = newMeal.Price,
                    Type = newMeal.Type.Name
                });
        }

        // PUT: api/Meals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeal(int id, MealBindingModel mealModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.Meals.All().Any(m => m.Id == id))
            {
                return this.NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var meal = db.Meals.Find(id);

            meal.Name = mealModel.Name;
            meal.Price = mealModel.Price;
            meal.Type = db.MealTypes.Find(mealModel.TypeId);

            db.SaveChanges();

            return this.Ok(new MealViewModel()
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Type = meal.Type.Name
            });
        }

        // DELETE: api/Meals/5
        public IHttpActionResult DeleteMeal(int id)
        {
            Meal meal = db.Meals.Find(id);
            if (meal == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            db.Meals.Remove(meal);
            db.SaveChanges();

            return Ok(new
            {
                Message = "Meal #" + id + " deleted."
            });
        }

        // POST: /api/Meals/{id}/order
        [Route("{id:int}/order")]
        public IHttpActionResult PostOrder(int id, OrderBindingModel orderModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!db.Meals.All().Any(r => r.Id == id))
            {
                return NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.db.Users.Find(currentUserId);

            Order order = new Order()
            {
                Meal = db.Meals.Find(id),
                MealId = id,
                CreatedOn = DateTime.Now,
                User = currentUser,
                UserId = currentUserId,
                Quantity = orderModel.Quantity,
                OrderStatus = OrderStatus.Pending
            };

            db.Orders.Add(order);
            db.SaveChanges();

            return this.Ok();
        }
    }
}