import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useApi } from "../hooks";
import { API_ENDPOINTS } from "../config/api";
import type { Company } from "../types/Company";

const CompanySearch: React.FC = () => {
  const [isin, setIsin] = useState("");
  const [company, setCompany] = useState<Company | null>(null);
  const [searchPerformed, setSearchPerformed] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const api = useApi();
  const navigate = useNavigate();

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isin.trim()) return;

    setSearchPerformed(true);
    setCompany(null);
    setLoading(true);
    setError(null);

    try {
      const response = await api.get<Company>(
        API_ENDPOINTS.COMPANY_BY_ISIN(isin.trim())
      );
      setCompany(response.data);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to search company");
      setCompany(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="company-search">
      <div className="card">
        <div className="search-header">
          <h1>Search Company by ISIN</h1>
          <p>Enter an ISIN code to find a company in the registry</p>
        </div>

        <form onSubmit={handleSearch} className="search-form">
          <div className="form-group">
            <label htmlFor="isin" className="form-label">
              ISIN Code:
            </label>
            <input
              id="isin"
              type="text"
              value={isin}
              onChange={(e) => setIsin(e.target.value)}
              className="form-input"
              placeholder="e.g., US0378331005"
              required
            />
          </div>

          <div className="form-actions">
            <button
              type="submit"
              disabled={loading || !isin.trim()}
              className={`btn-primary ${loading ? "btn-disabled" : ""}`}
            >
              {loading ? "Searching..." : "Search"}
            </button>
          </div>
        </form>

        {error && searchPerformed && (
          <div className="search-results">
            <div className="error">Company not found with ISIN: {isin}</div>
          </div>
        )}

        {company && (
          <div className="search-results">
            <h2>
              {company.name}
            </h2>

            <div className="info-field">
              <span className="info-label">Company ID:</span>
              <span className="info-value company-id">{company.id}</span>
            </div>

            <div className="info-field">
              <span className="info-label">Company Name:</span>
              <span className="info-value">{company.name}</span>
            </div>

            {company.ticker && (
              <div className="info-field">
                <span className="info-label">Ticker:</span>
                <span className="info-value ticker-badge">
                  {company.ticker}
                </span>
              </div>
            )}

            {company.exchange && (
              <div className="info-field">
                <span className="info-label">Exchange:</span>
                <span className="info-value">{company.exchange}</span>
              </div>
            )}

            <div className="info-field">
              <span className="info-label">ISIN:</span>
              <span className="info-value company-id">
                {company.isin || (
                  <span className="empty-value">Not specified</span>
                )}
              </span>
            </div>

            {company.website ? (
              <div className="info-field">
                <span className="info-label">Website:</span>
                <span className="info-value">
                  <a
                    href={company.website}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="company-website-link"
                  >
                    {company.website}
                  </a>
                </span>
              </div>
            ) : (
              <div className="info-field">
                <span className="info-label">Website:</span>
                <span className="info-value empty-value">Not specified</span>
              </div>
            )}

            <div className="form-actions">
              <button
                onClick={() => navigate(`/company/${company.id}`)}
                className="btn-secondary"
              >
                View Details
              </button>
              <button
                onClick={() => navigate(`/edit/${company.id}`)}
                className="btn-outline"
              >
                Edit
              </button>
            </div>
          </div>
        )}

        {searchPerformed && !company && !loading && !error && (
          <div className="search-results">
            <div className="empty-state">
              <h3>No company found</h3>
              <p>No company was found with ISIN: {isin}</p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default CompanySearch;
