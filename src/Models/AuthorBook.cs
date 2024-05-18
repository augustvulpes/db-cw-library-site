using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class AuthorBook
    {
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int BookId { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}
