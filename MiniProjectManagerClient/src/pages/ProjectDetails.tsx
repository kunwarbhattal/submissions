import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../api/axios";

interface Task {
  id: number;
  title: string;
  dueDate?: string;
  isCompleted: boolean;
}

export default function ProjectDetails() {
  const { id } = useParams();
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");
  const [dueDate, setDueDate] = useState("");

  async function loadTasks() {
    const res = await api.get(`/projects/${id}/tasks`);
    setTasks(res.data);
  }

  async function addTask(e: React.FormEvent) {
    e.preventDefault();
    if (!title.trim()) return;
    await api.post(`/projects/${id}/tasks`, {
      title,
      dueDate: dueDate || null,
    });
    setTitle("");
    setDueDate("");
    await loadTasks();
  }

  async function toggleTask(taskId: number) {
    await api.post(`/projects/${id}/tasks/${taskId}/toggle`);
    await loadTasks();
  }

  async function deleteTask(taskId: number) {
    await api.delete(`/projects/${id}/tasks/${taskId}`);
    await loadTasks();
  }

  async function runScheduler() {
    const res = await api.post(`/projects/${id}/schedule`);
    alert(`Scheduler result:\n${JSON.stringify(res.data.plan, null, 2)}`);
  }

  useEffect(() => {
    loadTasks();
  }, [id]);

  return (
    <div className="p-6 max-w-3xl mx-auto">
      <div className="flex justify-between mb-4">
        <h1 className="text-2xl font-semibold">Project #{id}</h1>
        <Link
          to="/dashboard"
          className="text-blue-600 underline hover:text-blue-800"
        >
          Back
        </Link>
      </div>

      <form onSubmit={addTask} className="mb-4 flex flex-col gap-2">
        <input
          className="border p-2 rounded"
          placeholder="Task title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <input
          type="date"
          className="border p-2 rounded"
          value={dueDate}
          onChange={(e) => setDueDate(e.target.value)}
        />
        <button className="bg-green-600 text-white p-2 rounded hover:bg-green-700">
          Add Task
        </button>
      </form>

      <button
        onClick={runScheduler}
        className="mb-6 bg-purple-600 text-white p-2 rounded hover:bg-purple-700"
      >
        Run Smart Scheduler
      </button>

      <div className="space-y-3">
        {tasks.map((t) => (
          <div
            key={t.id}
            className={`flex justify-between items-center p-3 rounded shadow ${
              t.isCompleted ? "bg-green-100" : "bg-white"
            }`}
          >
            <div>
              <p
                className={`font-medium ${
                  t.isCompleted ? "line-through text-gray-500" : ""
                }`}
              >
                {t.title}
              </p>
              {t.dueDate && (
                <p className="text-sm text-gray-500">
                  Due: {t.dueDate.split("T")[0]}
                </p>
              )}
            </div>
            <div className="space-x-2">
              <button
                onClick={() => toggleTask(t.id)}
                className="bg-blue-500 text-white px-2 py-1 rounded"
              >
                {t.isCompleted ? "Undo" : "Done"}
              </button>
              <button
                onClick={() => deleteTask(t.id)}
                className="bg-red-500 text-white px-2 py-1 rounded"
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
