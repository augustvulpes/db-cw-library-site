using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Author
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Country { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
    }
}
