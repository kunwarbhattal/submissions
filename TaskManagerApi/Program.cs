using TaskManagerApi.Models;
using TaskManagerApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure CORS (only once)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ✅ Register services (if you add DI later)
builder.Services.AddSingleton<TaskRepository>();

var app = builder.Build();

// ✅ Use CORS (only once, before endpoints)
app.UseCors("AllowAll");

// ✅ Simple repository instance
var repo = new TaskRepository();

// ✅ GET all tasks
app.MapGet("/api/tasks", () =>
{
    return Results.Ok(repo.GetAll());
});

// ✅ POST create task
app.MapPost("/api/tasks", (TaskItem task) =>
{
    repo.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

// ✅ PUT update task
app.MapPut("/api/tasks/{id}", (Guid id, TaskItem updatedTask) =>
{
    var success = repo.Update(id, updatedTask);
    return success ? Results.NoContent() : Results.NotFound();
});

// ✅ DELETE remove task
app.MapDelete("/api/tasks/{id}", (Guid id) =>
{
    var success = repo.Delete(id);
    return success ? Results.NoContent() : Results.NotFound();
});

app.Run();
