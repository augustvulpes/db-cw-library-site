using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        public int Pages { get; set; }
        public int Year { get; set; }
        [Required]
        public string BBK { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Order> Orders { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
        public List<CollectionBook> CollectionBooks { get; set; }
    }
}
