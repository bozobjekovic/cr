import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import CompanyList from "./components/CompanyList";
import CompanyDetails from "./components/CompanyDetails";
import CompanyForm from "./components/CompanyForm";
import CompanySearch from "./components/CompanySearch";
import { ProtectedRoute, Layout } from "./components";
import { Login, Register } from "./pages";
import { AuthProvider } from "./contexts/AuthContext";
import "./App.css";

function App() {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            {/* Public routes */}
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            
            {/* Protected routes */}
            <Route
              path="/*"
              element={
                <ProtectedRoute>
                  <Routes>
                    <Route path="/" element={<CompanyList />} />
                    <Route path="/company/:id" element={<CompanyDetails />} />
                    <Route
                      path="/create"
                      element={<CompanyForm mode="create" />}
                    />
                    <Route
                      path="/edit/:id"
                      element={<CompanyForm mode="edit" />}
                    />
                    <Route path="/search" element={<CompanySearch />} />
                  </Routes>
                </ProtectedRoute>
              }
            />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App;
