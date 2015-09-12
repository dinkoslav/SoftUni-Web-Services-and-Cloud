using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;
using BookShop.Models;

namespace BookShop.Services.Models.ViewModels
{
    public class AuthorViewModelWithBookCollection
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> BookTitles { get; set; } 

    }
    public class AuthorViewModel
    {
        public int  Id { get; set; }
        public string LastName { get; set; }
    }
}