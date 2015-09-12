using Restauranteur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurants.Services.BindingModels
{
    public class MealBindingModel
    {
        public string Name { get; set; }

        public int RestaurantId { get; set; }

        public int TypeId { get; set; }

        public decimal Price { get; set; }
    }
}