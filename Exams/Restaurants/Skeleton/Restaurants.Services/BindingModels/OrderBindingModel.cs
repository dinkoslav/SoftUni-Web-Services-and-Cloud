using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restaurants.Services.BindingModels
{
    public class OrderBindingModel
    {
        [Required]
        public int Quantity { get; set; }
    }
}