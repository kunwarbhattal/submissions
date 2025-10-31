import React from "react";

interface Task {
  id: string;
  description: string;
  isCompleted: boolean;
}

interface TaskListProps {
  tasks: Task[];
  onDelete: (id: string) => void;
  onToggle: (id: string) => void;
}

const TaskList: React.FC<TaskListProps> = ({ tasks, onDelete, onToggle }) => {
  if (tasks.length === 0) {
    return <p className="text-center text-muted">No tasks yet 😴</p>;
  }

  return (
    <ul className="list-group">
      {tasks.map((task) => (
        <li
          key={task.id}
          className={`list-group-item d-flex justify-content-between align-items-center ${
            task.isCompleted ? "list-group-item-success" : ""
          }`}
        >
          <span
            style={{
              textDecoration: task.isCompleted ? "line-through" : "none",
              cursor: "pointer",
            }}
            onClick={() => onToggle(task.id)}
          >
            {task.description}
          </span>
          <button
            className="btn btn-sm btn-outline-danger"
            onClick={() => onDelete(task.id)}
          >
            ✕
          </button>
        </li>
      ))}
    </ul>
  );
};

export default TaskList;
