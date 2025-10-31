using System.ComponentModel.DataAnnotations;

namespace MiniProjectManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
