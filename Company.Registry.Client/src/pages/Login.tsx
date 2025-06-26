import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks";

export const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const { login, isLoading } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      await login({ email, password });
      navigate("/");
    } catch {
      setError("Login failed. Please check your credentials.");
    }
  };

  return (
    <div className="company-form-container">
      <div className="card">
        <h1>Sign in to your account</h1>

        <form onSubmit={handleSubmit} className="company-form">
          <div className="form-group">
            <label htmlFor="email" className="form-label">
              Email address:
            </label>
            <input
              id="email"
              type="email"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="form-input"
              placeholder="Enter your email address"
            />
          </div>

          <div className="form-group">
            <label htmlFor="password" className="form-label">
              Password:
            </label>
            <input
              id="password"
              type="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="form-input"
              placeholder="Enter your password"
            />
          </div>

          {error && <div className="error form-alert">{error}</div>}

          <div className="form-actions">
            <button
              type="submit"
              disabled={isLoading}
              className={`btn-primary ${isLoading ? "btn-disabled" : ""}`}
            >
              {isLoading ? "🔄 Signing in..." : "Sign in"}
            </button>
          </div>
        </form>

        <div className="form-footer">
          <p>
            Don't have an account?{" "}
            <Link to="/register" className="auth-link">
              Create one here
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};
