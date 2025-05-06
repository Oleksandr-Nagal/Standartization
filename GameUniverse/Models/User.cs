using System.ComponentModel.DataAnnotations;

namespace GameUniverse.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MinLength(4)]
        public required string Username { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, MinLength(8)]
        public required string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; } = false;
    }
}
