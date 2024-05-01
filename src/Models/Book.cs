using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public string BBK { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }
        public ICollection<CollectionBook> CollectionBooks { get; set; }
    }
}
