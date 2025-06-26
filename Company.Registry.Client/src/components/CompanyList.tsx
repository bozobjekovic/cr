import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useApi } from "../hooks";
import { API_ENDPOINTS } from "../config/api";
import type { Company } from "../types/Company";

const CompanyList: React.FC = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const api = useApi();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCompanies = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await api.get<Company[]>(API_ENDPOINTS.COMPANIES);
        setCompanies(response.data);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Failed to fetch companies"
        );
      } finally {
        setLoading(false);
      }
    };

    fetchCompanies();
  }, [api]);

  if (loading) {
    return <div className="loading">Loading companies...</div>;
  }

  if (error) {
    return <div className="error">Error: {error}</div>;
  }

  if (companies.length === 0) {
    return (
      <div className="card empty-state">
        <h2>No companies found</h2>
        <p>There are no companies in the registry yet.</p>
        <button onClick={() => navigate("/create")} className="btn-primary">
          Add First Company
        </button>
      </div>
    );
  }

  return (
    <div className="company-list card">
      <div className="list-header">
        <h1>Companies</h1>
        <button onClick={() => navigate("/create")} className="btn-primary">
          Add New Company
        </button>
      </div>

      <div className="table-container">
        <table className="companies-table">
          <thead>
            <tr>
              <th>Company Name</th>
              <th>Ticker</th>
              <th>Exchange</th>
              <th>ISIN</th>
              <th>Website</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {companies.map((company) => (
              <tr key={company.id}>
                <td className="company-name">{company.name}</td>
                <td>
                  {company.ticker && (
                    <span className="ticker-badge">{company.ticker}</span>
                  )}
                </td>
                <td>{company.exchange || "-"}</td>
                <td className="isin-code">{company.isin}</td>
                <td>
                  {company.website ? (
                    <a
                      href={company.website}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="company-website-link"
                    >
                      {company.website}
                    </a>
                  ) : (
                    "-"
                  )}
                </td>
                <td>
                  <div className="action-buttons">
                    <button
                      onClick={() => navigate(`/company/${company.id}`)}
                      className="btn-secondary btn-small"
                    >
                      View
                    </button>
                    <button
                      onClick={() => navigate(`/edit/${company.id}`)}
                      className="btn-outline btn-small"
                    >
                      Edit
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default CompanyList;
