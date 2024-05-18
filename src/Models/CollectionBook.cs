using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class CollectionBook
    {
        [Required]
        public int CollectionId { get; set; }
        [Required]
        public int BookId { get; set; }
        public Collection Collection { get; set; }
        public Book Book { get; set; }
    }
}
