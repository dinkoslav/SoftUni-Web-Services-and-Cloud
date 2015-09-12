using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Models.BindingModels;
using BookShop.Services.Models.ViewModels;

namespace BookShop.Services.Controllers
{
    public class CategoriesController : ApiController
    {
        private  ApplicationDbContext db = new ApplicationDbContext();

        // GET	/api/categories

        public IHttpActionResult GetCategories()
        {
            var cat = db.Categories.Select(c => new CategorieViewModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            return this.Ok(cat);
        }

        //GET	/api/categories/{id}
        public IHttpActionResult GetCategorieById(int id)
        {
            var cat = db.Categories.Where(c => c.Id == id).Select(c => new CategorieViewModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            if (!cat.Any())
            {
                return this.NotFound();
            }
            return this.Ok(cat);
        }

        //PUT	/api/categories/{id}
        [HttpPut]
        public IHttpActionResult ChangeCategorieById(int id, Category cat)
        {
            var categoryToChange = db.Categories.First(c => c.Id == id);
            if (categoryToChange==null)
            {
                return this.NotFound();
            }
            categoryToChange.Name = cat.Name;
            db.SaveChanges();
            return this.Ok("changes has been made");
        }

        //DELETE	/api/categories/{id}
        [HttpDelete]
        public IHttpActionResult DeleteCategorie(int id)
        {
            try
            {
                var cat = db.Categories.First(b => b.Id == id);
                db.Categories.Remove(cat);
                db.SaveChanges();
                return this.Ok("resource deleted successfully ");
            }
            catch (Exception e)
            {
                return this.NotFound();
            }
            
        }

        //POST	/api/categories
        [HttpPost]
        public IHttpActionResult AddCategorie(AddCategorieBindingModels model)
        {
            if (model == null)
            {
                this.ModelState.AddModelError("model","the model is empty");
            }
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }
            var category = new Category()
            {
                Name = model.Name,
            };
            db.Categories.Add(category);
            db.SaveChanges();
            return this.CreatedAtRoute("DefaultApi", new { id = category.Id }, category);
        }
    }
}