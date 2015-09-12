using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Models.BindingModels;
using BookShop.Services.Models.ViewModels;
using Microsoft.Ajax.Utilities;

namespace BookShop.Services.Controllers
{
    public class AuthorsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET	/api/authors/{id}

        public IHttpActionResult GetAuthorById(int id)
        {
            var author = db.Authors.Where(a => a.Id == id).Select(a => new AuthorViewModelWithBookCollection()
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BookTitles = a.Books.Select(b=>b.Title)
            });
            
            if (!author.Any())
            {
                return this.NotFound();
            }
            return this.Ok(author);
        }

        //POST	/api/authors
        [HttpPost]
        public IHttpActionResult AddAuthor(AddAuthorBindingModel model)
        {
            if (model == null)
            {
                this.ModelState.AddModelError("model", "The model is empty");
            }
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var author = new Author()
            {
                FirstName = model.FirstName ?? null,
                LastName = model.LastName
            };
            db.Authors.Add(author);
            db.SaveChanges();
            return this.CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        // GET	/api/authors/{id}/books

        [HttpGet]
        [Route("api/authors/{id}/books")]
        public IHttpActionResult GetBooksByAuthorId(int id)
        {
            var books = db.Books.Where(b => b.Author.Id == id)
                .OrderBy(b => b.Title)
                .Take(10)
                .Select(b => new BookViewModel()
                {
                    Title = b.Title,
                    Description = b.Description,
                    Edition = b.Edition,
                    Price = b.Price,
                    Restriction = b.AgeRestriction,
                    Copies = b.Copies,
                    ReleaseDate = b.ReleaseDate,
                    AuthorData = new AuthorViewModel()
                    {
                        Id = b.Author.Id,
                        LastName = b.Author.LastName
                    },
                    Categories = b.Categories.Select(c => new CategorieViewModelNames()
                    {
                        Name = c.Name
                    })
                });
            if (!books.Any())
            {
                return this.NotFound();
            }
            return this.Ok(books);
        }
    }
}