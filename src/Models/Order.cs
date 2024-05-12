namespace LibraryApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string State { get; set; }
        public DateTime CreationDate { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
