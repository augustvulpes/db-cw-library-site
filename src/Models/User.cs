using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Email { get; set; }\
        public string Password { get; set; }
        public string Gender { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Surname { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 3)]
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Order> Orders { get; set; }
    }
}
