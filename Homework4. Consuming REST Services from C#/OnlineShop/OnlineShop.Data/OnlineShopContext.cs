namespace OnlineShop.Data
{
    using OnlineShop.Model;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OnlineShop.Data.Migrations;
    using System.Data.Entity;

    public class OnlineShopContext : IdentityDbContext<ApplicationUser>
    {
        // Your context has been configured to use a 'OnlineShopContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'OnlineShop.Data.OnlineShopContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'OnlineShopContext' 
        // connection string in the application configuration file.
        public OnlineShopContext()
            : base("name=OnlineShopContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OnlineShopContext, Configuration>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}