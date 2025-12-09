using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Core.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Username { get; set; } = null!;
        
        [Required]
        public string PasswordHash { get; set; } = null!;
    }
}
