using BookShopApi.Dto;
using BookShopSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace BookShopApi.Controllers
{
    public class BooksController
    {
        readonly BookShopContext _context = new BookShopContext();

        // GET api/books/{id}
        [HttpGet]
        public IQueryable GetBook(int id)
        {
            var books = _context.Books
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    id = b.Id,
                    title = b.Title,
                    author = b.Author.FirstName + " " + b.Author.LastName,
                    description = b.Description,
                    copies = b.Copies,
                    releaseDate = b.ReleaseDate,
                    ageRestriction = b.AgeRestriction,
                    categories = b.Categories
                        .Select(c => c.Name)
                });

            return books;
        }

        // GET api/authors/5/books
        [HttpGet]
        public IQueryable GetBooks([FromUri]string word)
        {
            var books = _context.Books
                .Where(b => b.Title.Contains(word))
                .OrderBy(b => b.Title)
                .Select(b => new
                {
                    id = b.Id,
                    title = b.Title
                })
                .Take(10);
        }

        // POST api/authors
        [HttpPost]
        [Route("api/authors")]
        public IHttpActionResult Post(AddAuthorBindingModel model)
        {
            
            _context.Authors.Add(author);
            _context.SaveChanges();
            return this.Ok(author);
        }
    }
}