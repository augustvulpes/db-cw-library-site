﻿using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Dto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required] 
        public string Password { get; set; }
    }
}
