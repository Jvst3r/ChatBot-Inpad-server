import React, { useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { 
  FaUser,           // Профиль
  FaChartBar,       // Дашборд
  FaQuestionCircle, // Вопросы
  FaComments,       // Ответы
  FaSignOutAlt      // Выйти (опционально)
} from 'react-icons/fa';

const Layout = ({ children }) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const location = useLocation();
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const menuItems = [
    { path: '/profile', label: 'Профиль', icon: <FaUser size={20} /> },
    { path: '/', label: 'Дашборд', icon: <FaChartBar size={20} /> },
    { path: '/questions', label: 'Вопросы', icon: <FaQuestionCircle size={20} /> },
    { path: '/answers', label: 'Ответы', icon: <FaComments size={20} /> },
  ];

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div style={{ display: 'flex', height: '100vh', backgroundColor: '#f5f6fa' }}>
      {/* Sidebar */}
      <aside style={{
        width: isSidebarOpen ? '300px' : '60px',
        background: '#187ce1ff',
        color: '#2c3e50',
        transition: 'all 0.3s ease',
        display: 'flex',
        flexDirection: 'column',
        boxShadow: '2px 0 15px rgba(0,0,0,0.08)',
        borderRight: '1px solid #e1e8ed'
      }}>
        <div style={{
          padding: '25px 20px',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          borderBottom: '2px solid rgba(255,255,255,0.3)',
          background: '#187ce1ff'
        }}>
          <h2 style={{ 
            fontSize: '1.5rem',
            fontWeight: '700',
            whiteSpace: 'nowrap',
            overflow: 'hidden',
            color: '#ffffff'
          }}>
            {isSidebarOpen && 'BIM ASSISTANT'}
          </h2>
        </div>
        
        <nav style={{ padding: '20px 0', flex: 1 }}>
          {menuItems.map(item => (
            <Link
              key={item.path}
              to={item.path}
              style={{
                display: 'flex',
                alignItems: 'center',
                padding: '15px 25px',
                color: location.pathname === item.path ? '#ffffff' : 'rgba(255,255,255,0.8)',
                textDecoration: 'none',
                transition: 'all 0.3s',
                borderLeft: '4px solid',
                borderLeftColor: location.pathname === item.path ? '#ffffff' : 'transparent',
                background: location.pathname === item.path ? 'rgba(255,255,255,0.15)' : 'transparent',
                fontSize: '1rem',
                fontWeight: location.pathname === item.path ? '600' : '400',
                margin: '5px 15px',
                borderRadius: '6px'
              }}
            >
              <span style={{ 
                marginRight: '15px', 
                fontSize: '20px',
                color: location.pathname === item.path ? '#ffffff' : 'rgba(255,255,255,0.8)',
                display: 'flex',
                alignItems: 'center'
              }}>
                {item.icon}
              </span>
              {isSidebarOpen && <span>{item.label}</span>}
            </Link>
          ))}
        </nav>

        {isSidebarOpen && (
          <div style={{
            padding: '25px 20px',
            borderTop: '2px solid rgba(255,255,255,0.3)',
            background: '#187ce1ff'
          }}>
            <div style={{ marginBottom: '15px' }}>
              <strong style={{ 
                display: 'block', 
                fontSize: '1rem',
                color: '#ffffff',
                marginBottom: '5px'
              }}>
                {user?.name}
              </strong>
              <span style={{ 
                fontSize: '0.85rem', 
                color: 'rgba(255,255,255,0.7)',
                display: 'block'
              }}>
                {user?.email}
              </span>
            </div>
            <button onClick={handleLogout} style={{
              width: '100%',
              padding: '10px',
              background: 'rgba(255,255,255,0.9)',
              border: 'none',
              color: '#187ce1ff',
              borderRadius: '6px',
              cursor: 'pointer',
              fontWeight: '600',
              transition: 'all 0.3s',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '8px'
            }}>
              <FaSignOutAlt /> Выйти
            </button>
          </div>
        )}
      </aside>

      {/* Main Content */}
      <main style={{ flex: 1, display: 'flex', flexDirection: 'column', overflow: 'hidden' }}>
        <div style={{ flex: 1, padding: '30px', background: '#f5f6fa', overflowY: 'auto' }}>
          {children}
        </div>
      </main>
    </div>
  );
};

export default Layout;
