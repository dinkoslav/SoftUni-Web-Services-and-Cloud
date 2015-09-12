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
using Restaurants.Data.UnitOfWork;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        //private RestaurantsContext db = new RestaurantsContext();

        private readonly IRestaurantsData db;

        public OrdersController()
            : this(new RestaurantsData())
        {
        }

        public OrdersController(RestaurantsData data)
        {
            this.db = data;
        }

        // GET: api/Orders
        public IHttpActionResult GetPendingOrders(int startPage, int limit, int? mealId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            var currentUserId = User.Identity.GetUserId();

            var orders = db.Orders
                .All()
                .OrderByDescending(o => o.CreatedOn)
                .Where(o => o.UserId == currentUserId && o.OrderStatus == OrderStatus.Pending)
                .Select(o => new OrderViewModel()
                {
                    Id = o.Id,
                    Meal = new MealViewModel()
                    {
                        Id = o.Meal.Id,
                        Name = o.Meal.Name,
                        Price = o.Meal.Price,
                        Type = o.Meal.Type.Name
                    },
                    Quantity = o.Quantity,
                    Status = o.OrderStatus,
                    CreatedOn = o.CreatedOn
                });

            if (mealId != null)
            {
                orders = orders
                    .Where(o => o.Meal.Id == mealId)
                    .Skip(startPage*limit)
                    .Take(limit);
            }
            else
            {
                orders = orders
                    .Skip(startPage * limit)
                    .Take(limit);
            }

            return this.Ok(orders);
        }
    }
}