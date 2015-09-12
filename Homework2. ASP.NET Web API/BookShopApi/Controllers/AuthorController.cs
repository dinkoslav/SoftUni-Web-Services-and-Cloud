using System.Linq;

namespace BookShopApi.Controllers
{
    using BookShopApi.Dto;
using BookShopSystem.Data;
using BookShopSystem.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

    public class AuthorController : ApiController
    {
        readonly BookShopContext _context = new BookShopContext();

        // GET api/authors/5
        [HttpGet]
        public IQueryable Get(int id)
        {
            var author = _context.Authors
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    id = a.Id,
                    firstName = a.FirstName,
                    lastName = a.LastName,
                    bookTitles = a.Books
                        .Select(b =>  b.Title)
                })
                ;

            return author;
        }

        // GET api/authors/5/books
        [HttpGet]
        public IQueryable Get(int id, string item)
        {
            if (item == "books")
            {
                var books = _context.Authors
                    .Where(a => a.Id == id)
                    .Select(a => a.Books
                        .Select(b => new
                        {
                            title = b.Title,
                            description = b.Description,
                            price = b.Price,
                            copies = b.Copies,
                            edition = b.EditionType,
                            categories = b.Categories
                                .Select(c => c.Name)
                        }));

                return books;
            }

            throw new WebException("Sorry, no suck item or author!");
        }

        // POST api/authors
        [HttpPost]
        [Route("api/authors")]
        public IHttpActionResult Post(AddAuthorBindingModel model)
        {
            Author author = new Author { FirstName = model.FirstName, LastName = model.LastName};
            _context.Authors.Add(author);
            _context.SaveChanges();
            return this.Ok(author);
        }
    }
}