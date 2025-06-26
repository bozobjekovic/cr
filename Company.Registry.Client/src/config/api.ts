// API Configuration
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:8080';
export const IDENTITY_SERVER_URL = import.meta.env.VITE_IDENTITY_SERVER_URL || 'http://localhost:8081';
export const API_ENDPOINTS = {
  COMPANIES: '/api/company',
  COMPANY_BY_ID: (id: string) => `/api/company/${id}`,
  COMPANY_BY_ISIN: (isin: string) => `/api/company/isin/${isin}`,
} as const; 