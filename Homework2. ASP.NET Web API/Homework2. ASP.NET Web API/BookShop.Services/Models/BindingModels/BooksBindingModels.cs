using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShop.Services.Models.BindingModels
{
    public class AddBooksBindingModels
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required !")]
        [MaxLength(50)]
        [MinLength(3)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Edition type is required !")]
        public Edition Edition { get; set; }

         [Required(ErrorMessage = "Price value is required !")]
        public decimal Price { get; set; }

         [Required(ErrorMessage = "Age restriction type is required !")]
        public AgeRestriction Restriction { get; set; }

         [Required(ErrorMessage = "Copies count is required !")]
        public int Copies { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? AuthorId { get; set; }

        public int? BookId { get; set; }

        public string Categories { get; set; }

    }
}