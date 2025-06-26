import React, { useState, useEffect, useCallback, ReactNode } from "react";
import { IDENTITY_SERVER_URL } from "../config/api";
import { AuthContext } from "../hooks/useAuth";

interface LoginCredentials {
  email: string;
  password: string;
}

interface RegisterCredentials {
  email: string;
  password: string;
}

interface AuthToken {
  accessToken: string;
  expiresIn: number;
}

interface AuthContextType {
  isAuthenticated: boolean;
  isLoading: boolean;
  token: AuthToken | null;
  login: (credentials: LoginCredentials) => Promise<void>;
  register: (credentials: RegisterCredentials) => Promise<void>;
  logout: () => void;
}

const STORAGE_KEY = "auth_token";

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [token, setToken] = useState<AuthToken | null>(null);

  useEffect(() => {
    const storedTokens = localStorage.getItem(STORAGE_KEY);
    if (storedTokens) {
      try {
        const parsedToken: AuthToken = JSON.parse(storedTokens);
        // Batch all state updates together
        setToken(parsedToken);
        setIsAuthenticated(true);
        setIsLoading(false);
      } catch {
        localStorage.removeItem(STORAGE_KEY);
        setIsLoading(false);
      }
    } else {
      // No token found
      setIsLoading(false);
    }
  }, []);

  const login = useCallback(async (credentials: LoginCredentials) => {
    setIsLoading(true);

    try {
      const response = await fetch(`${IDENTITY_SERVER_URL}/connect/token`, {
        method: "POST",
        headers: {
          "Content-Type": "application/x-www-form-urlencoded",
        },
        body: new URLSearchParams({
          grant_type: "password",
          username: credentials.email,
          password: credentials.password,
          client_id: "company-user-client",
          client_secret: "user-secret",
          scope: "company-api openid profile",
        }),
      });

      if (!response.ok) {
        throw new Error("Login failed");
      }

      const tokenData = await response.json();

      const newToken: AuthToken = {
        accessToken: tokenData.access_token,
        expiresIn: tokenData.expires_in,
      };

      setToken(newToken);
      setIsAuthenticated(true);
      localStorage.setItem(STORAGE_KEY, JSON.stringify(newToken));
    } finally {
      setIsLoading(false);
    }
  }, []);

  const register = useCallback(
    async (credentials: RegisterCredentials) => {
      setIsLoading(true);

      try {
        const response = await fetch(
          `${IDENTITY_SERVER_URL}/api/auth/register`,
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              email: credentials.email,
              password: credentials.password,
            }),
          }
        );

        if (!response.ok) {
          throw new Error("Registration failed");
        }

        await login(credentials);
      } finally {
        setIsLoading(false);
      }
    },
    [login]
  );

  const logout = useCallback(() => {
    setToken(null);
    setIsAuthenticated(false);
    localStorage.removeItem(STORAGE_KEY);
  }, []);

  const value: AuthContextType = {
    isAuthenticated,
    isLoading,
    token,
    login,
    register,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
