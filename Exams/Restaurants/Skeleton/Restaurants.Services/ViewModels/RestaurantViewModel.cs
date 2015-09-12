using Restaurants.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurants.Services.ViewModels
{
    public class RestaurantViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Rating { get; set; }

        public TownViewModel Town { get; set; }
    }
}