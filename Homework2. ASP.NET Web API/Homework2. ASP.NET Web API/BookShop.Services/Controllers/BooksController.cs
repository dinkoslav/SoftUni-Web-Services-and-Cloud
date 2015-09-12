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
using System.Xml.Linq;
using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Models.BindingModels;
using BookShop.Services.Models.ViewModels;

namespace BookShop.Services.Controllers
{
    public class BooksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET	/api/books/{id}
        public IHttpActionResult GetBooksById(int id)
        {
            var book = db.Books.Where(b => b.Id == id).Select(b => new BookViewModel()
            {
                Title = b.Title,
                //Categories = b.Categories.Select(c=>c.Name),
                Categories = b.Categories.Select(c => new CategorieViewModelNames()
                {
                   Name = c.Name
                }),
                Edition = b.Edition,
                Price = b.Price,
                Description = b.Description ?? null,
                Restriction = b.AgeRestriction,
                Copies = b.Copies,
                ReleaseDate = b.ReleaseDate,
                AuthorData = new AuthorViewModel()
                {
                    Id = b.Author.Id,
                    LastName = b.Author.LastName
                }
            });
            if (!book.Any())
            {
                return this.NotFound();
            }
            return this.Ok(book);
        }

        // GET	/api/books?search={word}
        public IHttpActionResult GetBooksBySearchWord(string searchWord)
        {
            var books = db.Books.Where(b => b.Title.Contains(searchWord)).Select(b => new BookViewModelTitleAndId()
            {
                Id = b.Id,
                Title = b.Title
            });
            if (!books.Any())
            {
                return this.NotFound();
            }
            return this.Ok(books);  
        }

        //PUT	/api/books/{id}
        [HttpPut]
        public IHttpActionResult ChangeBook(int id ,Book changedBook)
        {
            var book = db.Books.First(b => b.Id == id);
            if (book ==null)
            {
                return this.NotFound();
            }
            book.Id=changedBook.Id == 0 ? book.Id : changedBook.Id;
            book.Copies = changedBook.Copies == 0 ? book.Copies : changedBook.Copies;
            book.Price = changedBook.Price == 0 ? book.Price : changedBook.Price;
            book.ReleaseDate = changedBook.ReleaseDate ?? book.ReleaseDate;
            book.Title = changedBook.Title == String.Empty ? book.Title : changedBook.Title;
            book.AgeRestriction = changedBook.AgeRestriction;
            book.Edition = changedBook.Edition;
            book.Author = changedBook.Author ?? book.Author;
            book.Categories = changedBook.Categories ?? book.Categories;
            book.Description = changedBook.Description == String.Empty ? book.Description : changedBook.Description;
            book.RelatedBooks = changedBook.RelatedBooks ?? book.RelatedBooks;
            
            
            db.SaveChanges();
            return this.Ok("changes has been made");
        }

        //DELETE	/api/books/{id}
        [HttpDelete]
        public IHttpActionResult DeleteBook(int id)
        {
            try
            {
                var book = db.Books.First(b => b.Id == id);
                db.Books.Remove(book);
                db.SaveChanges();
                return this.Ok("resource deleted successfully ");
            }
            catch (Exception e )
            {
                return this.NotFound();
            }
            
        }

        // POST	/api/books 
        [HttpPost]
        public IHttpActionResult AddBook(AddBooksBindingModels  model)
        {

            if (model == null)
            {
                this.ModelState.AddModelError("model", "the model is empty");
            }
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var categories = model.Categories.Split(' ') ;
            var categoryList = new List<Category>();

            if (categories.Count() != 0)
            {
                
                foreach (var categoryName in categories)
                {
                    Category cat = new Category()
                    {
                        Name = categoryName
                    };
                    categoryList.Add(cat);
                }
            }
            
            var book = new Book()
            {
                Title = model.Title,
                Description = model.Description ?? null,
                Edition = model.Edition,
                Price = model.Price,
                AgeRestriction = model.Restriction,
                Copies = model.Copies,
                ReleaseDate = model.ReleaseDate,
                Author = db.Authors.First(a => a.Id==model.AuthorId) ?? null,
                RelatedBooks = db.Books.Where(b => b.Id == model.BookId).ToList().Count == 0 ? null : db.Books.Where(b => b.Id == model.BookId).ToList(),
                Categories = categoryList ?? null

            };
            db.Books.Add(book);
            db.SaveChanges();
            return this.Ok("created book  with id = "+ book.Id);
        }
    }
}