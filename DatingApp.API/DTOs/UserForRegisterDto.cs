using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDto
    {

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "You must choose a username between 4 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Your password is too long or too short.")]
        public string Password { get; set; }
    }
}