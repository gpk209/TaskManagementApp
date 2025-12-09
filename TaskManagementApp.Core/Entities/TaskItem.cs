using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Core.Entities
{
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
}
