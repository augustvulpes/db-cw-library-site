using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class News
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
