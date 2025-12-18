import React from 'react';

const Dashboard = () => {
  return (
    <div style={{ padding: '10px' }}>

      {/* Раздел Дашборд */}
      <div style={{ 
        marginBottom: '30px',
      }}>
        <h2 style={{ 
          color: '#2c3e50', 
          marginBottom: '25px',
          borderBottom: '2px solid #ecf0f1',
          paddingBottom: '20px'
        }}>
          Дашборд
        </h2>
        
        <div style={{ 
          display: 'grid', 
          gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', 
          gap: '25px' 
        }}>
          {/* Карточка 1 - С синим верхом */}
          <div style={{ 
            position: 'relative',
            background: 'white',
            color: '#187ce1ff',
            padding: '25px',
            borderRadius: '10px',
            boxShadow: '0 6px 15px rgba(102, 126, 234, 0.3)',
            overflow: 'hidden' // Важно для правильного отображения
          }}>
            {/* Синяя полоса сверху */}
            <div style={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '5px',
              background: 'linear-gradient(90deg, #1976d2, #2196F3)',
              borderTopLeftRadius: '10px',
              borderTopRightRadius: '10px'
            }} />
            
            <div style={{ fontSize: '0.9rem', opacity: 0.9, marginBottom: '5px' }}>
              ВСЕГО ВОПРОСОВ
            </div>
            <div style={{ fontSize: '3rem', fontWeight: 'bold', margin: '10px 0' }}>
              245
            </div>
            <div style={{ fontSize: '0.85rem', opacity: 0.8 }}>
              За все время работы
            </div>
          </div>
          
          {/* Карточка 2 - С синим верхом */}
          <div style={{ 
            position: 'relative',
            background: 'white',
            color: '#187ce1ff',
            padding: '25px',
            borderRadius: '10px',
            boxShadow: '0 6px 15px rgba(240, 147, 251, 0.3)',
            overflow: 'hidden'
          }}>
            {/* Синяя полоса сверху */}
            <div style={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '5px',
              background: 'linear-gradient(90deg, #1976d2, #2196F3)',
              borderTopLeftRadius: '10px',
              borderTopRightRadius: '10px'
            }} />
            
            <div style={{ fontSize: '0.9rem', opacity: 0.9, marginBottom: '5px' }}>
              НОВЫХ ВОПРОСОВ
            </div>
            <div style={{ fontSize: '3rem', fontWeight: 'bold', margin: '10px 0' }}>
              12
            </div>
            <div style={{ fontSize: '0.85rem', opacity: 0.8 }}>
              За последнюю неделю
            </div>
          </div>
          
          {/* Карточка 3 - С синим верхом */}
          <div style={{ 
            position: 'relative',
            background: 'white',
            color: '#187ce1ff',
            padding: '25px',
            borderRadius: '10px',
            boxShadow: '0 6px 15px rgba(79, 172, 254, 0.3)',
            overflow: 'hidden'
          }}>
            {/* Синяя полоса сверху */}
            <div style={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '5px',
              background: 'linear-gradient(90deg, #1976d2, #2196F3)',
              borderTopLeftRadius: '10px',
              borderTopRightRadius: '10px'
            }} />
            
            <div style={{ fontSize: '0.9rem', opacity: 0.9, marginBottom: '5px' }}>
              АРХИВИРОВАННЫХ ОТВЕТОВ
            </div>
            <div style={{ fontSize: '3rem', fontWeight: 'bold', margin: '10px 0' }}>
              89
            </div>
            <div style={{ fontSize: '0.85rem', opacity: 0.8 }}>
              Хранятся в архиве
            </div>
          </div>
        </div>
      </div>

      {/* Раздел Активность - тоже с синим верхом */}
      <div style={{ 
        position: 'relative',
        background: 'white', 
        borderRadius: '12px', 
        padding: '30px',
        marginBottom: '30px',
        boxShadow: '0 4px 12px rgba(0,0,0,0.08)',
        overflow: 'hidden'
      }}>

        
        <h2 style={{ 
          color: '#2c3e50', 
          marginBottom: '25px',
          borderBottom: '2px solid #ecf0f1',
          paddingBottom: '10px'
        }}>
          Активность
        </h2>
        
        <div style={{ marginBottom: '20px' }}>
          <div style={{ 
            color: '#7f8c8d', 
            fontSize: '0.95rem',
            marginBottom: '10px'
          }}>
            График активности за период
          </div>
          {/* Заглушка для графика */}
          <div style={{ 
            height: '200px', 
            background: 'linear-gradient(180deg, #f8f9fa 0%, #e9ecef 100%)',
            borderRadius: '8px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: '#6c757d',
            border: '2px dashed #dee2e6'
          }}>
            [Место для графика активности]
          </div>
        </div>

        <div>
          <h3 style={{ color: '#2c3e50', marginBottom: '15px' }}>Статистика</h3>
          <div style={{ 
            color: '#7f8c8d', 
            fontSize: '0.95rem',
            lineHeight: '1.6'
          }}>
            Особенно полезно: детали
            <div style={{ 
              marginTop: '15px',
              padding: '15px',
              background: '#f8f9fa',
              borderRadius: '6px',
              fontSize: '0.9rem'
            }}>
              • Просмотр детальной статистики по периодам<br/>
              • Анализ активности пользователей<br/>
              • Отслеживание ключевых метрик
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
