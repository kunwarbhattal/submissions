using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProjectManagerAPI.DTOs;
using MiniProjectManagerAPI.Services;

namespace MiniProjectManagerAPI.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId:int}/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _auth;

        public TasksController(ITaskService taskService, IAuthService auth)
        {
            _taskService = taskService;
            _auth = auth;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int projectId)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var tasks = await _taskService.GetTasksAsync(uid, projectId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int projectId, [FromBody] CreateTaskDto dto)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var created = await _taskService.CreateTaskAsync(uid, projectId, dto);
            return CreatedAtAction(nameof(GetAll), new { projectId }, created);
        }

        [HttpPut("{taskId:int}")]
        public async Task<IActionResult> Update(int projectId, int taskId, [FromBody] CreateTaskDto dto)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var updated = await _taskService.UpdateTaskAsync(uid, projectId, taskId, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPost("{taskId:int}/toggle")]
        public async Task<IActionResult> Toggle(int projectId, int taskId)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            await _taskService.ToggleTaskCompletionAsync(uid, projectId, taskId);
            return NoContent();
        }

        [HttpDelete("{taskId:int}")]
        public async Task<IActionResult> Delete(int projectId, int taskId)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            await _taskService.DeleteTaskAsync(uid, projectId, taskId);
            return NoContent();
        }
    }
}
