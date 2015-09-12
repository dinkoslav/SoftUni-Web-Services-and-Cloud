using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restaurants.Services.BindingModels
{
    public class RestaurantBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string TownId { get; set; }
    }
}