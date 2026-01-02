using System.ComponentModel.DataAnnotations;

namespace ChatBotInpadserver.Data.DTOs.WebClientDTOs
{
    public class RegisterAdminDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginAdminDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginAdminResponse
    {
        [Required]
        public bool Success { get; set; }
        [Required]
        public string Message { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        
    }

}