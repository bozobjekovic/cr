import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks';

const Header: React.FC = () => {
  const location = useLocation();
  const { isAuthenticated, logout } = useAuth();

  const isActive = (path: string) => {
    if (path === '/' && location.pathname === '/') return true;
    if (path !== '/' && location.pathname.startsWith(path)) return true;
    return false;
  };

  const handleLogout = () => {
    logout();
  };

  return (
    <header className="header">
      <div className="header-container">
        <div className="header-brand">
          <Link to="/" className="brand-link">
            Company Registry
          </Link>
        </div>
        
        {isAuthenticated && (
          <nav className="header-nav">
            <Link 
              to="/" 
              className={`nav-link ${isActive('/') ? 'nav-link-active' : ''}`}
            >
              📊 Companies
            </Link>
            <Link 
              to="/create" 
              className={`nav-link ${isActive('/create') ? 'nav-link-active' : ''}`}
            >
              ➕ Add Company
            </Link>
            <Link 
              to="/search" 
              className={`nav-link ${isActive('/search') ? 'nav-link-active' : ''}`}
            >
              🔍 Search ISIN
            </Link>
            
            <button 
              onClick={handleLogout}
              className="logout-btn"
              title="Logout"
            >
              ←
            </button>
          </nav>
        )}
      </div>
    </header>
  );
};

export default Header; 