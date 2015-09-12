namespace OnlineShop.Model
{
    using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

    public class ApplicationUser: IdentityUser
    {
        private ICollection<Ad> ads;

        public ApplicationUser()
        {
            this.ads = new HashSet<Ad>();
        }

        public ICollection<Ad> OwnAds
        {
            get { return this.ads; }
            set { this.ads = value; }
        }
    }
}
