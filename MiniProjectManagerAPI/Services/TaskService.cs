using Microsoft.EntityFrameworkCore;
using MiniProjectManagerAPI.Data;
using MiniProjectManagerAPI.DTOs;
using MiniProjectManagerAPI.Models;

namespace MiniProjectManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _db;
        public TaskService(AppDbContext db) => _db = db;

        private async Task<Project?> ValidateProjectOwnership(int userId, int projectId)
        {
            var project = await _db.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null) throw new KeyNotFoundException("Project not found.");
            return project;
        }

        public async Task<TaskDto> CreateTaskAsync(int userId, int projectId, CreateTaskDto dto)
        {
            var project = await ValidateProjectOwnership(userId, projectId);
            var task = new TaskItem
            {
                Title = dto.Title,
                DueDate = dto.DueDate,
                ProjectId = projectId
            };
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId
            };
        }

        public async Task DeleteTaskAsync(int userId, int projectId, int taskId)
        {
            await ValidateProjectOwnership(userId, projectId);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);
            if (task == null) throw new KeyNotFoundException("Task not found.");
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync(int userId, int projectId)
        {
            await ValidateProjectOwnership(userId, projectId);
            return await _db.Tasks.Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId
                }).ToListAsync();
        }

        public async Task ToggleTaskCompletionAsync(int userId, int projectId, int taskId)
        {
            await ValidateProjectOwnership(userId, projectId);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);
            if (task == null) throw new KeyNotFoundException("Task not found.");
            task.IsCompleted = !task.IsCompleted;
            await _db.SaveChangesAsync();
        }

        public async Task<TaskDto?> UpdateTaskAsync(int userId, int projectId, int taskId, CreateTaskDto dto)
        {
            await ValidateProjectOwnership(userId, projectId);
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);
            if (task == null) return null;

            task.Title = dto.Title;
            task.DueDate = dto.DueDate;
            await _db.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId
            };
        }

        // Simple Smart Scheduler: distribute tasks into days before due dates or upcoming days.
        public async Task<object> ScheduleAsync(int userId, int projectId)
        {
            var project = await ValidateProjectOwnership(userId, projectId);
            var tasks = project.Tasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue).ToList();

            // Simple heuristic: For tasks with due dates, schedule them on their due date (if not past).
            // For tasks without due date, assign sequential days starting tomorrow.
            var schedule = new List<object>();
            var dayPointer = DateTime.UtcNow.Date.AddDays(1);

            foreach (var t in tasks)
            {
                DateTime scheduledDate;
                if (t.DueDate.HasValue)
                {
                    scheduledDate = t.DueDate!.Value.Date;
                    if (scheduledDate < DateTime.UtcNow.Date)
                        scheduledDate = DateTime.UtcNow.Date; // if due is past, assign today
                }
                else
                {
                    scheduledDate = dayPointer;
                    dayPointer = dayPointer.AddDays(1);
                }

                schedule.Add(new
                {
                    TaskId = t.Id,
                    Title = t.Title,
                    ScheduledFor = scheduledDate
                });
            }

            return new { ProjectId = projectId, Plan = schedule };
        }
    }
}
