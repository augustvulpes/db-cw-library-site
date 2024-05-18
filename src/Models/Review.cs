using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Review
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
