namespace LibraryApp.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
    }
}
