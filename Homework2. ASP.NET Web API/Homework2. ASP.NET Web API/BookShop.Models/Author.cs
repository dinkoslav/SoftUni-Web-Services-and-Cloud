using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class Author
    {
        private ICollection<Book> books; //field

        public Author() //constructor
        {
            this.books = new HashSet<Book>();
        }
        [Key]
        public int Id { get; set; } //properties
        public string FirstName { get; set; }

        [Required] // Attributes [annotations] [constrains]
        public string LastName { get; set; }

        public virtual ICollection<Book> Books // this is navigation properties
        {
            get { return this.books; }
            set { this.books = value; }
        }
    }
}
