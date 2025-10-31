import { Link } from "react-router-dom";

interface Props {
  id: number;
  title: string;
  description?: string;
  onDelete: (id: number) => void;
}

export default function ProjectCard({ id, title, description, onDelete }: Props) {
  return (
    <div className="flex justify-between items-center bg-white shadow p-3 rounded hover:shadow-md transition">
      <div>
        <Link
          to={`/projects/${id}`}
          className="font-semibold text-blue-600 hover:underline"
        >
          {title}
        </Link>
        {description && (
          <p className="text-sm text-gray-600">{description}</p>
        )}
      </div>
      <button
        onClick={() => onDelete(id)}
        className="bg-red-500 text-white px-2 py-1 rounded hover:bg-red-600"
      >
        Delete
      </button>
    </div>
  );
}
