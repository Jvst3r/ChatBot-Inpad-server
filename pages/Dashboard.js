import React from 'react';

const Dashboard = () => {
  return (
    <div>
      <h1>Дашборд</h1>
      <p>Добро пожаловать в админ-панель!</p>
      
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '20px', marginTop: '30px' }}>
  
        
        <div style={{ background: 'white', padding: '20px', borderRadius: '8px', boxShadow: '0 2px 4px rgba(0,0,0,0.1)' }}>
          <h3>Ответы на вопросы</h3>
          <p style={{ fontSize: '2rem', margin: '10px 0', color: '#e74c3c' }}>567</p>
        </div>
        
        <div style={{ background: 'white', padding: '20px', borderRadius: '8px', boxShadow: '0 2px 4px rgba(0,0,0,0.1)' }}>
          <h3>Вопросы</h3>
          <p style={{ fontSize: '2rem', margin: '10px 0', color: '#f39c12' }}>89</p>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;