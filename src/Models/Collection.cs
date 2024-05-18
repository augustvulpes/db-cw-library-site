using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Collection
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public List<CollectionBook> CollectionBooks { get; set; }
    }
}
