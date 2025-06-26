import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useApi } from "../hooks";
import { API_ENDPOINTS } from "../config/api";
import type {
  Company,
  CreateCompanyRequest,
  UpdateCompanyRequest,
} from "../types/Company";

interface CompanyFormProps {
  mode: "create" | "edit";
}

const CompanyForm: React.FC<CompanyFormProps> = ({ mode }) => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const api = useApi();

  const [formData, setFormData] = useState({
    name: "",
    exchange: "",
    ticker: "",
    isin: "",
    website: "",
  });

  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [loadingCompany, setLoadingCompany] = useState(false);

  useEffect(() => {
    const loadCompany = async (companyId: string) => {
      try {
        setLoadingCompany(true);
        const response = await api.get<Company>(
          API_ENDPOINTS.COMPANY_BY_ID(companyId)
        );
        const company = response.data;
        setFormData({
          name: company.name || "",
          exchange: company.exchange || "",
          ticker: company.ticker || "",
          isin: company.isin || "",
          website: company.website || "",
        });
      } catch {
        setError("Failed to load company data");
      } finally {
        setLoadingCompany(false);
      }
    };

    if (mode === "edit" && id) {
      loadCompany(id);
    }
  }, [mode, id, api]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      if (mode === "create") {
        const request: CreateCompanyRequest = {
          name: formData.name,
          exchange: formData.exchange,
          ticker: formData.ticker,
          isin: formData.isin,
          website: formData.website,
        };
        await api.post<Company>(API_ENDPOINTS.COMPANIES, request);
        navigate("/");
      } else if (mode === "edit" && id) {
        const request: UpdateCompanyRequest = {
          id,
          name: formData.name,
          exchange: formData.exchange,
          ticker: formData.ticker,
          isin: formData.isin,
          website: formData.website,
        };
        await api.put<Company>(API_ENDPOINTS.COMPANY_BY_ID(id), request);
        navigate(`/company/${id}`);
      }
    } catch (err) {
      let errorMessage = `Failed to ${mode} company. Please try again.`;

      if (err && typeof err === "object" && "response" in err) {
        const apiError = err as {
          response?: { data?: { errors?: Record<string, string[]> } };
        };
        if (apiError.response?.data?.errors) {
          const validationErrors = Object.values(apiError.response.data.errors)
            .flat()
            .join(", ");
          errorMessage = validationErrors || errorMessage;
        }
      }

      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  if (loadingCompany) {
    return <div className="loading">Loading company data...</div>;
  }

  return (
    <div className="company-form-container">
      <div className="card">
        <h1>{mode === "create" ? "Add New Company" : "Edit Company"}</h1>

        <form onSubmit={handleSubmit} className="company-form">
          <div className="form-group">
            <label htmlFor="name" className="form-label">
              Company Name:
            </label>
            <input
              id="name"
              name="name"
              type="text"
              required
              value={formData.name}
              onChange={handleInputChange}
              className="form-input"
              placeholder="Enter company name"
            />
          </div>

          <div className="form-group">
            <label htmlFor="exchange" className="form-label">
              Exchange:
            </label>
            <input
              id="exchange"
              name="exchange"
              type="text"
              value={formData.exchange}
              onChange={handleInputChange}
              className="form-input"
              placeholder="Enter exchange (e.g., NYSE, NASDAQ)"
            />
          </div>

          <div className="form-group">
            <label htmlFor="ticker" className="form-label">
              Ticker:
            </label>
            <input
              id="ticker"
              name="ticker"
              type="text"
              value={formData.ticker}
              onChange={handleInputChange}
              className="form-input"
              placeholder="Enter ticker symbol (e.g., AAPL)"
            />
          </div>

          <div className="form-group">
            <label htmlFor="isin" className="form-label">
              ISIN:
            </label>
            <input
              id="isin"
              name="isin"
              type="text"
              required
              value={formData.isin}
              onChange={handleInputChange}
              className="form-input"
              placeholder="Enter ISIN code"
            />
          </div>

          <div className="form-group">
            <label htmlFor="website" className="form-label">
              Website:
            </label>
            <input
              id="website"
              name="website"
              type="url"
              value={formData.website}
              onChange={handleInputChange}
              className="form-input"
              placeholder="Enter website URL"
            />
          </div>

          {error && <div className="error form-alert">{error}</div>}

          <div className="form-actions">
            <button
              type="submit"
              disabled={loading}
              className={`btn-primary ${loading ? "btn-disabled" : ""}`}
            >
              {loading
                ? mode === "create"
                  ? "Creating..."
                  : "Updating..."
                : mode === "create"
                ? "Create Company"
                : "Update Company"}
            </button>
            <button
              type="button"
              onClick={() => navigate("/")}
              className="btn-outline"
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CompanyForm;
