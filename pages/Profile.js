import React, { useState } from 'react';

const Profile = () => {
  const [email, setEmail] = useState('admin@example.com');
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  const handleSaveChanges = () => {
    setIsSaving(true);
    // Здесь будет логика сохранения
    setTimeout(() => {
      setIsSaving(false);
      setIsEditing(false);
      alert('Изменения сохранены!');
    }, 1000);
  };

  const handleEditToggle = () => {
    setIsEditing(!isEditing);
    if (!isEditing) {
      setCurrentPassword('');
      setNewPassword('');
      setConfirmPassword('');
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1 style={{ 
        color: '#2c3e50', 
        marginBottom: '30px',
        paddingBottom: '15px',
        fontSize: '28px'
      }}>
        Профиль администратора
      </h1>

      {/* Информация аккаунта */}
      <div style={{ 
        position: 'relative',
        background: 'white',
        borderRadius: '12px',
        padding: '30px',
        marginBottom: '30px',
        boxShadow: '0 4px 12px rgba(0,0,0,0.08)',
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
          borderTopLeftRadius: '12px',
          borderTopRightRadius: '12px'
        }} />

        <h2 style={{ 
          color: '#2c3e50',
          marginBottom: '25px',
          paddingBottom: '10px',
          borderBottom: '1px solid #ecf0f1',
          fontSize: '22px'
        }}>
          Информация аккаунта
        </h2>

        <div style={{ marginBottom: '30px' }}>
          <div style={{
            display: 'flex',
            alignItems: 'center',
            marginBottom: '15px'
          }}>
            <div style={{
              width: '12px',
              height: '12px',
              borderRadius: '50%',
              backgroundColor: '#187ce1ff',
              marginRight: '15px'
            }} />
            <label style={{
              color: '#2c3e50',
              fontWeight: '600',
              fontSize: '14px',
              letterSpacing: '0.5px',
              marginBottom: '8px',
              display: 'block',
              minWidth: '200px'
            }}>
              EMAIL АДРЕС
            </label>
          </div>
          
          {isEditing ? (
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              style={{
                width: '100%',
                maxWidth: '400px',
                padding: '12px 15px',
                border: '2px solid #e0e0e0',
                borderRadius: '8px',
                fontSize: '16px',
                color: '#2c3e50',
                backgroundColor: '#f8f9fa',
                transition: 'all 0.3s ease',
                outline: 'none'
              }}
              onFocus={(e) => e.target.style.borderColor = '#187ce1ff'}
              onBlur={(e) => e.target.style.borderColor = '#e0e0e0'}
            />
          ) : (
            <div style={{
              padding: '12px 15px',
              backgroundColor: '#f8f9fa',
              borderRadius: '8px',
              fontSize: '16px',
              color: '#2c3e50',
              border: '2px solid transparent',
              maxWidth: '400px'
            }}>
              {email}
            </div>
          )}
        </div>

        {/* Разделитель */}
        <div style={{
          height: '1px',
          backgroundColor: '#ecf0f1',
          margin: '30px 0',
          width: '100%'
        }} />

        {/* Кнопка Изменить */}
        <div style={{ marginBottom: '30px' }}>
          <button
            onClick={handleEditToggle}
            style={{
              padding: '12px 30px',
              backgroundColor: isEditing ? '#6c757d' : '#187ce1ff',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              fontSize: '16px',
              fontWeight: '600',
              cursor: 'pointer',
              transition: 'all 0.3s ease',
              boxShadow: '0 3px 10px rgba(24, 124, 225, 0.2)'
            }}
            onMouseEnter={(e) => e.target.style.opacity = '0.9'}
            onMouseLeave={(e) => e.target.style.opacity = '1'}
          >
            {isEditing ? 'Отменить редактирование' : 'Изменить'}
          </button>
        </div>

        {/* Безопасность */}
        {isEditing && (
          <>
            <div style={{
              height: '1px',
              backgroundColor: '#ecf0f1',
              margin: '30px 0',
              width: '100%'
            }} />

            <h3 style={{ 
              color: '#2c3e50',
              marginBottom: '25px',
              fontSize: '20px'
            }}>
              Безопасность
            </h3>

            {/* Текущий пароль */}
            <div style={{ marginBottom: '25px' }}>
              <div style={{
                display: 'flex',
                alignItems: 'center',
                marginBottom: '10px'
              }}>
                <div style={{
                  width: '12px',
                  height: '12px',
                  borderRadius: '50%',
                  backgroundColor: '#187ce1ff',
                  marginRight: '15px'
                }} />
                <label style={{
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  letterSpacing: '0.5px',
                  display: 'block',
                  minWidth: '200px'
                }}>
                  ТЕКУЩИЙ ПАРОЛЬ
                </label>
              </div>
              <input
                type="password"
                value={currentPassword}
                onChange={(e) => setCurrentPassword(e.target.value)}
                placeholder="Введите текущий пароль"
                style={{
                  width: '100%',
                  maxWidth: '400px',
                  padding: '12px 15px',
                  border: '2px solid #e0e0e0',
                  borderRadius: '8px',
                  fontSize: '16px',
                  color: '#2c3e50',
                  backgroundColor: '#f8f9fa',
                  transition: 'all 0.3s ease',
                  outline: 'none'
                }}
                onFocus={(e) => e.target.style.borderColor = '#187ce1ff'}
                onBlur={(e) => e.target.style.borderColor = '#e0e0e0'}
              />
            </div>

            {/* Новый пароль */}
            <div style={{ marginBottom: '25px' }}>
              <div style={{
                display: 'flex',
                alignItems: 'center',
                marginBottom: '10px'
              }}>
                <div style={{
                  width: '12px',
                  height: '12px',
                  borderRadius: '50%',
                  backgroundColor: '#187ce1ff',
                  marginRight: '15px'
                }} />
                <label style={{
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  letterSpacing: '0.5px',
                  display: 'block',
                  minWidth: '200px'
                }}>
                  НОВЫЙ ПАРОЛЬ
                </label>
              </div>
              <input
                type="password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                placeholder="Введите новый пароль"
                style={{
                  width: '100%',
                  maxWidth: '400px',
                  padding: '12px 15px',
                  border: '2px solid #e0e0e0',
                  borderRadius: '8px',
                  fontSize: '16px',
                  color: '#2c3e50',
                  backgroundColor: '#f8f9fa',
                  transition: 'all 0.3s ease',
                  outline: 'none'
                }}
                onFocus={(e) => e.target.style.borderColor = '#187ce1ff'}
                onBlur={(e) => e.target.style.borderColor = '#e0e0e0'}
              />
            </div>

            {/* Подтверждение пароля */}
            <div style={{ marginBottom: '30px' }}>
              <div style={{
                display: 'flex',
                alignItems: 'center',
                marginBottom: '10px'
              }}>
                <div style={{
                  width: '12px',
                  height: '12px',
                  borderRadius: '50%',
                  backgroundColor: '#187ce1ff',
                  marginRight: '15px'
                }} />
                <label style={{
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  letterSpacing: '0.5px',
                  display: 'block',
                  minWidth: '200px'
                }}>
                  ПОДТВЕРЖДЕНИЕ ПАРОЛЯ
                </label>
              </div>
              <input
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="Подтвердите новый пароль"
                style={{
                  width: '100%',
                  maxWidth: '400px',
                  padding: '12px 15px',
                  border: '2px solid #e0e0e0',
                  borderRadius: '8px',
                  fontSize: '16px',
                  color: '#2c3e50',
                  backgroundColor: '#f8f9fa',
                  transition: 'all 0.3s ease',
                  outline: 'none'
                }}
                onFocus={(e) => e.target.style.borderColor = '#187ce1ff'}
                onBlur={(e) => e.target.style.borderColor = '#e0e0e0'}
              />
            </div>

            {/* Разделитель */}
            <div style={{
              height: '1px',
              backgroundColor: '#ecf0f1',
              margin: '30px 0',
              width: '100%'
            }} />

            {/* Кнопка Сохранить */}
            <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
              <button
                onClick={handleSaveChanges}
                disabled={isSaving || newPassword !== confirmPassword}
                style={{
                  padding: '12px 40px',
                  backgroundColor: newPassword === confirmPassword ? '#28a745' : '#6c757d',
                  color: 'white',
                  border: 'none',
                  borderRadius: '8px',
                  fontSize: '16px',
                  fontWeight: '600',
                  cursor: newPassword === confirmPassword ? 'pointer' : 'not-allowed',
                  transition: 'all 0.3s ease',
                  boxShadow: newPassword === confirmPassword ? '0 3px 10px rgba(40, 167, 69, 0.2)' : 'none',
                  opacity: newPassword === confirmPassword ? 1 : 0.7
                }}
                onMouseEnter={(e) => {
                  if (newPassword === confirmPassword) {
                    e.target.style.opacity = '0.9';
                  }
                }}
                onMouseLeave={(e) => {
                  if (newPassword === confirmPassword) {
                    e.target.style.opacity = '1';
                  }
                }}
              >
                {isSaving ? 'Сохранение...' : 'Сохранить'}
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default Profile;