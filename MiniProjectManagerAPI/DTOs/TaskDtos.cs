using System.ComponentModel.DataAnnotations;

namespace MiniProjectManagerAPI.DTOs
{
    public class CreateTaskDto
    {
        [Required, MinLength(1), MaxLength(200)]
        public string Title { get; set; } = null!;

        public DateTime? DueDate { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
    }
}
