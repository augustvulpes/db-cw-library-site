namespace LibraryApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
