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
        public List<Review> Reviews { get; set; }
        public List<Order> Orders { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
        public List<CollectionBook> CollectionBooks { get; set; }
    }
}
