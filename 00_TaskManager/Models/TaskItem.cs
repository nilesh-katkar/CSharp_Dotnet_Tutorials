using System.ComponentModel.DataAnnotations;

namespace _00_TaskManager.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public int Priority { get; set; }
    }
}
