using MiniProjectManagerAPI.DTOs;

namespace MiniProjectManagerAPI.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsAsync(int userId);
        Task<ProjectDto> CreateProjectAsync(int userId, CreateProjectDto dto);
        Task DeleteProjectAsync(int userId, int projectId);
        Task<ProjectDto?> GetProjectAsync(int userId, int projectId);
    }
}
