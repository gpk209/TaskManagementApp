using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Core.Entities
{
    public enum Priority { Low, Medium, High }
    public enum Status { Pending, Completed }

    public class TaskItem
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public Status Status { get; set; } = Status.Pending;
    }

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
