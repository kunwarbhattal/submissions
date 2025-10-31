using Microsoft.EntityFrameworkCore;
using MiniProjectManagerAPI.Data;
using MiniProjectManagerAPI.DTOs;
using MiniProjectManagerAPI.Models;

namespace MiniProjectManagerAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _db;
        public ProjectService(AppDbContext db) => _db = db;

        public async Task<ProjectDto> CreateProjectAsync(int userId, CreateProjectDto dto)
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null) throw new KeyNotFoundException("Project not found.");
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsAsync(int userId)
        {
            return await _db.Projects
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ProjectDto?> GetProjectAsync(int userId, int projectId)
        {
            var p = await _db.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.UserId == userId);
            if (p == null) return null;
            return new ProjectDto { Id = p.Id, Title = p.Title, Description = p.Description, CreatedAt = p.CreatedAt };
        }
    }
}
