using MiniProjectManagerAPI.DTOs;

namespace MiniProjectManagerAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksAsync(int userId, int projectId);
        Task<TaskDto> CreateTaskAsync(int userId, int projectId, CreateTaskDto dto);
        Task ToggleTaskCompletionAsync(int userId, int projectId, int taskId);
        Task DeleteTaskAsync(int userId, int projectId, int taskId);
        Task<TaskDto?> UpdateTaskAsync(int userId, int projectId, int taskId, CreateTaskDto dto);
        Task<object> ScheduleAsync(int userId, int projectId); // Smart scheduler output: return object (e.g., plan)
    }
}
