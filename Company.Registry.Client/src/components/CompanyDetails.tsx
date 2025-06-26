import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useApi } from "../hooks";
import { API_ENDPOINTS } from "../config/api";
import type { Company } from "../types/Company";

const CompanyDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [company, setCompany] = useState<Company | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const api = useApi();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCompany = async () => {
      if (!id) return;

      setLoading(true);
      setError(null);

      try {
        const response = await api.get<Company>(
          API_ENDPOINTS.COMPANY_BY_ID(id)
        );
        setCompany(response.data);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Failed to fetch company"
        );
      } finally {
        setLoading(false);
      }
    };

    fetchCompany();
  }, [id, api]);

  if (loading) {
    return <div className="loading">Loading company details...</div>;
  }

  if (error) {
    return <div className="error">Error: {error}</div>;
  }

  if (!company) {
    return <div className="error">Company not found</div>;
  }

  return (
    <div className="company-details-container">
      <div className="card">
        <h1>{company.name}</h1>

        <div className="info-field">
          <span className="info-label">Company ID:</span>
          <span className="info-value company-id">{company.id}</span>
        </div>

        <div className="info-field">
          <span className="info-label">Company Name:</span>
          <span className="info-value">
            {company.name || <span className="empty-value">Not specified</span>}
          </span>
        </div>

        {company.ticker && (
          <div className="info-field">
            <span className="info-label">Ticker:</span>
            <span className="info-value ticker-badge">{company.ticker}</span>
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
            {company.isin || <span className="empty-value">Not specified</span>}
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
            onClick={() => navigate(`/edit/${company.id}`)}
            className="btn-primary"
          >
            Edit Company
          </button>
          <button onClick={() => navigate("/")} className="btn-outline">
            Back to List
          </button>
        </div>
      </div>
    </div>
  );
};

export default CompanyDetails;
