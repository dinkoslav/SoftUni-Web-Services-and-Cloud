using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurants.Services.ViewModels
{
    public class MealViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }
    }
}