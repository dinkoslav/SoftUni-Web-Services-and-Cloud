using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data
{
    class OnlineShopTest
    {
        public static void Main()
        {
            var context = new OnlineShopContext();
            context.Users.Count();
        }
    }
}
