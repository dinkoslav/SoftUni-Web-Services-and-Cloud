//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using BookShop.Data;
//using BookShop.Models;

//namespace BookShop.Services.Controllers
//{
//    public class TestAuthController : ApiController
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: api/TestAuth
//        public IQueryable<Author> GetAuthors()
//        {
//            return db.Authors;
//        }

//        // GET: api/TestAuth/5
//        [ResponseType(typeof(Author))]
//        public IHttpActionResult GetAuthor(int id)
//        {
//            Author author = db.Authors.Find(id);
//            if (author == null)
//            {
//                return NotFound();
//            }

//            return Ok(author);
//        }

//        // PUT: api/TestAuth/5
//        [ResponseType(typeof(void))]
//        public IHttpActionResult PutAuthor(int id, Author author)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != author.Id)
//            {
//                return BadRequest();
//            }

//            db.Entry(author).State = EntityState.Modified;

//            try
//            {
//                db.SaveChanges();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!AuthorExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/TestAuth
//        [ResponseType(typeof(Author))]
//        public IHttpActionResult PostAuthor(Author author)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.Authors.Add(author);
//            db.SaveChanges();

//            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
//        }

//        // DELETE: api/TestAuth/5
//        [ResponseType(typeof(Author))]
//        public IHttpActionResult DeleteAuthor(int id)
//        {
//            Author author = db.Authors.Find(id);
//            if (author == null)
//            {
//                return NotFound();
//            }

//            db.Authors.Remove(author);
//            db.SaveChanges();

//            return Ok(author);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool AuthorExists(int id)
//        {
//            return db.Authors.Count(e => e.Id == id) > 0;
//        }
//    }
//}