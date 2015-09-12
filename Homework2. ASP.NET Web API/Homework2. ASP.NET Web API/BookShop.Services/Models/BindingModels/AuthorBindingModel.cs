using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShop.Services.Models.BindingModels
{
    public class AddAuthorBindingModel
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required! :)))")]
        public string LastName { get; set; }

    }
}