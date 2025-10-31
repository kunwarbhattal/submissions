import React, { useEffect, useState } from "react";
import "./App.css";

const apiUrl = import.meta.env.API_BASE;

interface Task {
  id: string;
  description: string;
  isCompleted: boolean;
}

const App: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [newTask, setNewTask] = useState("");
  const [filter, setFilter] = useState<"all" | "active" | "completed">("all");
  const [searchTerm, setSearchTerm] = useState("");

  const apiBase = apiUrl || "http://localhost:5013/api/tasks";

  useEffect(() => {
    fetch(apiBase)
      .then((res) => res.json())
      .then((data) => setTasks(data))
      .catch((err) => console.error("Error fetching tasks:", err));
  }, []);

  const addTask = async () => {
    if (!newTask.trim()) return;
    const newItem: Omit<Task, "id"> = {
      description: newTask.trim(),
      isCompleted: false,
    };

    const res = await fetch(apiBase, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newItem),
    });

    const data = await res.json();
    setTasks([...tasks, data]);
    setNewTask("");
  };

  const toggleComplete = async (id: string) => {
    const updatedTasks = tasks.map((t) =>
      t.id === id ? { ...t, isCompleted: !t.isCompleted } : t
    );
    setTasks(updatedTasks);

    const taskToUpdate = updatedTasks.find((t) => t.id === id);
    await fetch(`${apiBase}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(taskToUpdate),
    });
  };

  const deleteTask = async (id: string) => {
    await fetch(`${apiBase}/${id}`, { method: "DELETE" });
    setTasks(tasks.filter((t) => t.id !== id));
  };

  const clearTasks = async () => {
    for (const t of tasks) {
      await fetch(`${apiBase}/${t.id}`, { method: "DELETE" });
    }
    setTasks([]);
  };

  const filteredTasks = tasks.filter((task) => {
    if (filter === "active") return !task.isCompleted;
    if (filter === "completed") return task.isCompleted;
    return true;
  });

  const searchedTasks = filteredTasks.filter((task) =>
    task.description.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="container">
      <h1 className="title">Task Manager</h1>

      <div className="input-section">
        <input
          type="text"
          value={newTask}
          onChange={(e) => setNewTask(e.target.value)}
          placeholder="Enter a new task..."
        />
        <button onClick={addTask}>Add Task</button>
      </div>

      <div className="controls">
        <input
          type="text"
          placeholder="🔍 Search tasks..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <div className="filters">
          <button
            className={filter === "all" ? "active" : ""}
            onClick={() => setFilter("all")}
          >
            All
          </button>
          <button
            className={filter === "active" ? "active" : ""}
            onClick={() => setFilter("active")}
          >
            Active
          </button>
          <button
            className={filter === "completed" ? "active" : ""}
            onClick={() => setFilter("completed")}
          >
            Completed
          </button>
        </div>
        <button className="clear" onClick={clearTasks}>
          Clear All
        </button>
      </div>

      <ul className="task-list">
        {searchedTasks.map((task) => (
          <li key={task.id} className={task.isCompleted ? "completed" : ""}>
            <span onClick={() => toggleComplete(task.id)}>
              {task.description}
            </span>
            <button className="delete" onClick={() => deleteTask(task.id)}>
              ✖
            </button>
          </li>
        ))}
        {searchedTasks.length === 0 && (
          <p className="empty-msg">No tasks to display</p>
        )}
      </ul>
    </div>
  );
};

export default App;
