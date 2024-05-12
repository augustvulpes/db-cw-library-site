using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}
