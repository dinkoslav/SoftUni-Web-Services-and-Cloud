using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShop.Services.Models.BindingModels
{
    public class AddCategorieBindingModels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }


    }
}