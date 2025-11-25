import React, { useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

const Layout = ({ children }) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const location = useLocation();
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const menuItems = [
    { path: '/', label: 'Дашборд', icon: '📊' },
    { path: '/users', label: 'Ответы на вопросы', icon: '👥' },
    { path: '/products', label: 'Вопросы', icon: '📦' },
  ];

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div style={{ display: 'flex', height: '100vh', backgroundColor: '#f5f6fa' }}>
      {/* Sidebar */}
      <aside style={{
        width: isSidebarOpen ? '250px' : '60px',
        background: 'linear-gradient(180deg, #2c3e50 0%, #3498db 100%)',
        color: 'white',
        transition: 'all 0.3s ease',
        display: 'flex',
        flexDirection: 'column',
        boxShadow: '2px 0 10px rgba(0,0,0,0.1)'
      }}>
        <div style={{
          padding: '20px',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          borderBottom: '1px solid rgba(255,255,255,0.1)',
          background: 'rgba(0,0,0,0.1)'
        }}>
          <h2 style={{ 
            fontSize: '1.2rem', 
            fontWeight: '600',
            whiteSpace: 'nowrap',
            overflow: 'hidden'
          }}>
            {isSidebarOpen && 'Admin Panel'}
          </h2>
          <button 
            onClick={() => setIsSidebarOpen(!isSidebarOpen)}
            style={{
              background: 'rgba(255,255,255,0.1)',
              border: 'none',
              color: 'white',
              cursor: 'pointer',
              padding: '5px 10px',
              borderRadius: '4px'
            }}
          >
            {isSidebarOpen ? '◀' : '▶'}
          </button>
        </div>
        
        <nav style={{ padding: '20px 0', flex: 1 }}>
          {menuItems.map(item => (
            <Link
              key={item.path}
              to={item.path}
              style={{
                display: 'flex',
                alignItems: 'center',
                padding: '12px 20px',
                color: location.pathname === item.path ? 'white' : '#ecf0f1',
                textDecoration: 'none',
                transition: 'all 0.3s',
                borderLeft: '3px solid',
                borderLeftColor: location.pathname === item.path ? '#3498db' : 'transparent',
                background: location.pathname === item.path ? 'rgba(52, 152, 219, 0.2)' : 'transparent'
              }}
            >
              <span style={{ marginRight: '12px', fontSize: '18px' }}>{item.icon}</span>
              {isSidebarOpen && <span>{item.label}</span>}
            </Link>
          ))}
        </nav>

        {isSidebarOpen && (
          <div style={{
            padding: '20px',
            borderTop: '1px solid rgba(255,255,255,0.1)',
            background: 'rgba(0,0,0,0.1)'
          }}>
            <div style={{ marginBottom: '10px' }}>
              <strong style={{ display: 'block', fontSize: '0.9rem' }}>{user?.name}</strong>
              <span style={{ fontSize: '0.8rem', color: '#bdc3c7' }}>{user?.email}</span>
            </div>
            <button onClick={handleLogout} style={{
              width: '100%',
              padding: '8px',
              background: 'rgba(231, 76, 60, 0.2)',
              border: '1px solid rgba(231, 76, 60, 0.3)',
              color: '#e74c3c',
              borderRadius: '4px',
              cursor: 'pointer'
            }}>
              Выйти
            </button>
          </div>
        )}
      </aside>

      {/* Main Content */}
      <main style={{ flex: 1, display: 'flex', flexDirection: 'column', overflow: 'hidden' }}>
        <header style={{
          background: 'white',
          padding: '20px 30px',
          borderBottom: '1px solid #e1e8ed',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
        }}>
          <h1 style={{ color: '#2c3e50', fontSize: '1.5rem', fontWeight: '600' }}>
            {menuItems.find(item => item.path === location.pathname)?.label || 'Админ панель'}
          </h1>
          <div style={{ color: '#7f8c8d', fontSize: '0.9rem' }}>
            Добро пожаловать, {user?.name}
          </div>
        </header>
        <div style={{ flex: 1, padding: '30px', background: '#f5f6fa', overflowY: 'auto' }}>
          {children}
        </div>
      </main>
    </div>
  );
};

export default Layout;