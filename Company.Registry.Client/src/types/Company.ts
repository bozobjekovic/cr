export interface Company {
  id: string;
  name: string;
  exchange: string;
  ticker: string;
  isin: string;
  website?: string;
}

export interface CreateCompanyRequest {
  name: string;
  exchange: string;
  ticker: string;
  isin: string;
  website?: string;
}

export interface UpdateCompanyRequest {
  id: string;
  name: string;
  exchange: string;
  ticker: string;
  isin: string;
  website?: string;
} 