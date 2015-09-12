using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShop.Services.Models.ViewModels
{
    public class BookViewModel
    {
       
        public string Title { get; set; }
        public string Description { get; set; }
        public Edition Edition { get; set; }
        public decimal Price { get; set; }
        public AgeRestriction Restriction { get; set; }
        public decimal Copies { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public IEnumerable<CategorieViewModelNames> Categories { get; set; }
        public AuthorViewModel AuthorData { get; set; }

    }

    public class BookViewModelTitles
    {
        public IEnumerable<string> Title { get; set; }
    }
    
    public class BookViewModelTitleAndId
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}