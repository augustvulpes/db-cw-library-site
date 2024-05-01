namespace LibraryApp.Models
{
    public class CollectionBook
    {
        public int CollectionId { get; set; }
        public int BookId { get; set; }
        public Collection Collection { get; set; }
        public Book Book { get; set; }
    }
}
