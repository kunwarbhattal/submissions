using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProjectManagerAPI.DTOs;
using MiniProjectManagerAPI.Services;

namespace MiniProjectManagerAPI.Controllers
{
    [ApiController]
    [Route("api/v1/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IAuthService _auth;

        public ProjectsController(IProjectService projectService, IAuthService auth)
        {
            _projectService = projectService;
            _auth = auth;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var projects = await _projectService.GetProjectsAsync(uid);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var created = await _projectService.CreateProjectAsync(uid, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var project = await _projectService.GetProjectAsync(uid, id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            await _projectService.DeleteProjectAsync(uid, id);
            return NoContent();
        }

        // Smart Scheduler endpoint
        [HttpPost("{projectId:int}/schedule")]
        public async Task<IActionResult> Schedule(int projectId, [FromServices] ITaskService taskService)
        {
            var uid = _auth.GetUserIdFromClaims(User);
            var plan = await taskService.ScheduleAsync(uid, projectId);
            return Ok(plan);
        }
    }
}
