import { useMemo } from "react";
import axios from "axios";
import { API_BASE_URL } from "../config/api";
import { useAuth } from "./useAuth";

export const useApi = () => {
  const { token } = useAuth();

  const api = useMemo(() => {
    const apiInstance = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        "Content-Type": "application/json",
      },
    });

    apiInstance.interceptors.request.use((config) => {
      if (token?.accessToken) {
        config.headers.Authorization = `Bearer ${token.accessToken}`;
      }
      return config;
    });

    return apiInstance;
  }, [token]);

  return api;
};
