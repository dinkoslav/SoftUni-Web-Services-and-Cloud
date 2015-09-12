namespace OnlineShop.Model
{
    using System.Collections;
    using System.Collections.Generic;

    public class Ad
    {
        private ICollection<Category> categories;

        public Ad()
        {
            this.categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public AdType Type { get; set; }

        public AdStatus Status { get; set; }

        public ICollection<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }
    }
}
