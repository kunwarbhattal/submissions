import { Link, useLocation } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function Navbar() {
  const { logout } = useAuth();
  const location = useLocation();

  return (
    <nav className="bg-white shadow mb-6">
      <div className="max-w-5xl mx-auto flex items-center justify-between p-4">
        <Link
          to="/dashboard"
          className="text-2xl font-bold text-blue-700 tracking-tight"
        >
          Mini Project Manager
        </Link>

        <div className="flex gap-4 items-center">
          {location.pathname.startsWith("/projects/") && (
            <Link
              to="/dashboard"
              className="text-sm text-gray-700 hover:text-blue-600"
            >
              ← Back to Dashboard
            </Link>
          )}

          <button
            onClick={logout}
            className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded"
          >
            Logout
          </button>
        </div>
      </div>
    </nav>
  );
}
