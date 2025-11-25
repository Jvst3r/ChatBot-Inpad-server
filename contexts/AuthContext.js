import React, { createContext, useState, useContext, useEffect } from 'react';

const AuthContext = createContext();

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('admin_token');
    const userData = localStorage.getItem('admin_user');
    
    if (token && userData) {
      setUser(JSON.parse(userData));
    }
    setLoading(false);
  }, []);

  const login = async (email, password) => {
    if (email === 'admin@example.com' && password === 'admin') {
      const userData = { 
        id: 1, 
        name: 'Администратор', 
        email: 'admin@example.com',
        role: 'admin' 
      };
      
      setUser(userData);
      localStorage.setItem('admin_token', 'fake-jwt-token');
      localStorage.setItem('admin_user', JSON.stringify(userData));
      return { success: true };
    } else {
      return { success: false, error: 'Неверные учетные данные' };
    }
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('admin_token');
    localStorage.removeItem('admin_user');
  };

  const value = {
    user,
    login,
    logout,
    loading,
    isAuthenticated: !!user
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};