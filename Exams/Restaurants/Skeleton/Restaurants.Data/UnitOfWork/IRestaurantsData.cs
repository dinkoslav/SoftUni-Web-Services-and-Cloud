using Microsoft.AspNet.Identity;
using Restauranteur.Models;
using Restaurants.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Data.UnitOfWork
{
    public interface IRestaurantsData
    {
        Repositories.IRepository<Restaurant> Restaurants { get; }
        Repositories.IRepository<ApplicationUser> Users { get; }
        Repositories.IRepository<MealType> MealTypes { get; }
        Repositories.IRepository<Town> Towns { get; }
        Repositories.IRepository<Meal> Meals { get; }
        Repositories.IRepository<Order> Orders { get; }
        Repositories.IRepository<Rating> Ratings { get; }
        IUserStore<ApplicationUser> UserStore { get; }

        void SaveChanges();
    }
}
