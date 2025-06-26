import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks";

export const Register = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const { register, isLoading } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    if (password.length < 6) {
      setError("Password must be at least 6 characters long.");
      return;
    }

    try {
      await register({ email, password });
      navigate("/");
    } catch {
      setError("Registration failed. Please try again.");
    }
  };

  return (
    <div className="company-form-container">
      <div className="card">
        <h1>Create your account</h1>

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
              placeholder="Enter your password (min 6 characters)"
            />
          </div>

          {error && <div className="error form-alert">{error}</div>}

          <div className="form-actions">
            <button
              type="submit"
              disabled={isLoading}
              className={`btn-primary ${isLoading ? "btn-disabled" : ""}`}
            >
              {isLoading ? "Creating account..." : "Create account"}
            </button>
          </div>
        </form>

        <div className="form-footer">
          <p>
            Already have an account?{" "}
            <Link to="/login" className="auth-link">
              Sign in here
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};
