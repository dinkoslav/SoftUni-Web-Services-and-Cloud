using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class Book
    {
        private ICollection<Category> categories; // field
        private ICollection<Book> relatedBooks;


        public Book() /// constructor
        {
            this.categories = new HashSet<Category>();
            this.relatedBooks = new HashSet<Book>();
        }
        [Key]
        public int Id { get; set; }

        [MinLength(1)]
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public virtual Author Author { get; set; }

        [Required]
        public Edition Edition { get; set; }


        [Required]
        public decimal Price { get; set; }

        public AgeRestriction AgeRestriction { get; set; }

        [Required]
        public int Copies { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual ICollection<Book> RelatedBooks { get; set; }
        //public Author Author { get; set; } // has one author

        public virtual ICollection<Category> Categories
        {  // navigation property -has many categories
            get { return this.categories; }
            set { this.categories = value; }
        }
    }
}
