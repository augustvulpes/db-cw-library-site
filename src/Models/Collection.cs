namespace LibraryApp.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CollectionBook> CollectionBooks { get; set; }
    }
}
