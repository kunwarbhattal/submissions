import { useEffect, useState } from "react";
import api from "../api/axios";
import { useAuth } from "../hooks/useAuth";

import ProjectCard from "../components/ProjectCard";

// ...



interface Project {
  id: number;
  title: string;
  description?: string;
  createdAt: string;
}

export default function Dashboard() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const { logout } = useAuth();

  async function loadProjects() {
    const res = await api.get("/projects");
    setProjects(res.data);
  }

  async function addProject(e: React.FormEvent) {
    e.preventDefault();
    if (!title.trim()) return;
    await api.post("/projects", { title, description });
    setTitle("");
    setDescription("");
    await loadProjects();
  }

  async function deleteProject(id: number) {
    await api.delete(`/projects/${id}`);
    await loadProjects();
  }

  useEffect(() => {
    loadProjects();
  }, []);

  return (
    <div className="p-6 max-w-3xl mx-auto">
      <div className="flex justify-between mb-4">
        <h1 className="text-2xl font-semibold">Your Projects</h1>
        <button
          onClick={logout}
          className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600"
        >
          Logout
        </button>
      </div>

      <form onSubmit={addProject} className="mb-6 flex flex-col gap-2">
        <input
          className="border p-2 rounded"
          placeholder="Project title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <textarea
          className="border p-2 rounded"
          placeholder="Description (optional)"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <button className="bg-blue-600 text-white p-2 rounded hover:bg-blue-700">
          Add Project
        </button>
      </form>

      <div className="space-y-3">
        {projects.map((p) => (
          <ProjectCard key={p.id} {...p} onDelete={deleteProject} />
        ))}
      </div>
    </div>
  );
}
