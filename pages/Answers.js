import React, { useState } from 'react';

const Answers = () => {
  const [answers, setAnswers] = useState([
    {
      id: 1,
      question: "Как настроить параметры в BIM-модели компании?",
      answer: "Перейдите в панель управления параметрами Revit, выберите стандартные шаблоны компании из библиотеки BIM. Убедитесь, что все параметры соответствуют актуальным стандартам в разделе 'Настройки'. Применяемые изменения автоматически синхронизируются со всеми проектами.",
      author: "Admin",
      date: "15 декабря, 2025",
      tags: ["Revit", "BIM", "Параметры"]
    },
    {
      id: 2,
      question: "Ошибка при экспорте модели Revit в формате IFC",
      answer: "Убедитесь, что используется последняя версия плагина экспорта IFC. Проверьте, что все компоненты модели соответствуют стандартам компании. Используйте встроенный валидатор BIM перед экспортом. Если ошибка повторяется, обратитесь в техническую поддержку с лог-файлом.",
      author: "Admin",
      date: "10 декабря, 2025",
      tags: ["Revit", "IFC", "Экспорт", "Ошибки"]
    },
    {
      id: 3,
      question: "Как работать с общими параметрами в проекте?",
      answer: "Общие параметры настраиваются через диалоговое окно 'Общие параметры' в Revit. Создайте семейство параметров или используйте существующие из библиотеки компании. Убедитесь в совместимости версий параметров между всеми участниками проекта.",
      author: "BIM Manager",
      date: "5 декабря, 2025",
      tags: ["Общие параметры", "Совместная работа"]
    }
  ]);

  const [newAnswer, setNewAnswer] = useState({
    question: '',
    answer: '',
    tags: ''
  });

  const [showAddForm, setShowAddForm] = useState(false);

  const handleAddAnswer = () => {
    if (!newAnswer.question.trim() || !newAnswer.answer.trim()) {
      alert('Заполните вопрос и ответ');
      return;
    }

    const newAnswerObj = {
      id: answers.length + 1,
      question: newAnswer.question,
      answer: newAnswer.answer,
      author: "Admin",
      date: new Date().toLocaleDateString('ru-RU', { 
        day: 'numeric', 
        month: 'long', 
        year: 'numeric' 
      }),
      tags: newAnswer.tags.split(',').map(tag => tag.trim()).filter(tag => tag)
    };

    setAnswers([newAnswerObj, ...answers]);
    setNewAnswer({ question: '', answer: '', tags: '' });
    setShowAddForm(false);
  };

  const handleEditAnswer = (id) => {
    // Реализация редактирования
    alert(`Редактирование ответа ${id}`);
  };

  const handleDeleteAnswer = (id) => {
    if (window.confirm('Удалить этот ответ?')) {
      setAnswers(answers.filter(answer => answer.id !== id));
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
        Ответы на вопросы
      </h1>

      <div style={{ 
        background: '#f8f9fa',
        padding: '20px',
        borderRadius: '10px',
        marginBottom: '30px',
        fontSize: '16px',
        lineHeight: '1.6',
        color: '#494b4dff'
      }}>
        База ответов для первичной поддержки инженеров.
      </div>

      {/* Кнопка добавления нового ответа */}
      <div style={{ marginBottom: '30px' }}>
        <button
          onClick={() => setShowAddForm(!showAddForm)}
          style={{
            padding: '12px 25px',
            backgroundColor: showAddForm ? '#6c757d' : '#187ce1ff',
            color: 'white',
            border: 'none',
            borderRadius: '8px',
            fontSize: '16px',
            fontWeight: '600',
            cursor: 'pointer',
            transition: 'all 0.3s ease',
            boxShadow: '0 3px 10px rgba(24, 124, 225, 0.2)',
            display: 'flex',
            alignItems: 'center',
            gap: '10px'
          }}
          onMouseEnter={(e) => e.target.style.opacity = '0.9'}
          onMouseLeave={(e) => e.target.style.opacity = '1'}
        >
          <span style={{ fontSize: '20px' }}>+</span>
          {showAddForm ? 'Отменить добавление' : 'Добавить новый ответ'}
        </button>
      </div>

      {/* Форма добавления нового ответа */}
      {showAddForm && (
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

          <h3 style={{ 
            color: '#2c3e50',
            marginBottom: '25px',
            fontSize: '20px'
          }}>
            Новый ответ
          </h3>

          <div style={{ marginBottom: '20px' }}>
            <label style={{
              color: '#2c3e50',
              fontWeight: '600',
              fontSize: '14px',
              marginBottom: '8px',
              display: 'block'
            }}>
              Вопрос
            </label>
            <input
              type="text"
              value={newAnswer.question}
              onChange={(e) => setNewAnswer({...newAnswer, question: e.target.value})}
              placeholder="Введите вопрос"
              style={{
                width: '100%',
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

          <div style={{ marginBottom: '20px' }}>
            <label style={{
              color: '#2c3e50',
              fontWeight: '600',
              fontSize: '14px',
              marginBottom: '8px',
              display: 'block'
            }}>
              Ответ
            </label>
            <textarea
              value={newAnswer.answer}
              onChange={(e) => setNewAnswer({...newAnswer, answer: e.target.value})}
              placeholder="Введите подробный ответ"
              rows="6"
              style={{
                width: '100%',
                padding: '12px 15px',
                border: '2px solid #e0e0e0',
                borderRadius: '8px',
                fontSize: '16px',
                color: '#2c3e50',
                backgroundColor: '#f8f9fa',
                transition: 'all 0.3s ease',
                outline: 'none',
                resize: 'vertical',
                fontFamily: 'inherit'
              }}
              onFocus={(e) => e.target.style.borderColor = '#187ce1ff'}
              onBlur={(e) => e.target.style.borderColor = '#e0e0e0'}
            />
          </div>

          <div style={{ marginBottom: '25px' }}>
            <label style={{
              color: '#2c3e50',
              fontWeight: '600',
              fontSize: '14px',
              marginBottom: '8px',
              display: 'block'
            }}>
              Теги (через запятую)
            </label>
            <input
              type="text"
              value={newAnswer.tags}
              onChange={(e) => setNewAnswer({...newAnswer, tags: e.target.value})}
              placeholder="Revit, BIM, Параметры"
              style={{
                width: '100%',
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

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '15px' }}>
            <button
              onClick={() => setShowAddForm(false)}
              style={{
                padding: '12px 25px',
                backgroundColor: '#6c757d',
                color: 'white',
                border: 'none',
                borderRadius: '8px',
                fontSize: '16px',
                fontWeight: '600',
                cursor: 'pointer',
                transition: 'all 0.3s ease'
              }}
            >
              Отмена
            </button>
            <button
              onClick={handleAddAnswer}
              style={{
                padding: '12px 30px',
                backgroundColor: '#28a745',
                color: 'white',
                border: 'none',
                borderRadius: '8px',
                fontSize: '16px',
                fontWeight: '600',
                cursor: 'pointer',
                transition: 'all 0.3s ease',
                boxShadow: '0 3px 10px rgba(40, 167, 69, 0.2)'
              }}
            >
              Добавить ответ
            </button>
          </div>
        </div>
      )}

      {/* Список ответов */}
      {answers.map((item) => (
        <div 
          key={item.id}
          style={{ 
            position: 'relative',
            background: 'white',
            borderRadius: '12px',
            padding: '30px',
            marginBottom: '25px',
            boxShadow: '0 4px 12px rgba(0,0,0,0.08)',
            overflow: 'hidden'
          }}
        >
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

          <h3 style={{ 
            color: '#2c3e50',
            marginBottom: '15px',
            fontSize: '20px',
            lineHeight: '1.4'
          }}>
            Вопрос: {item.question}
          </h3>

          <div style={{ 
            background: '#f8f9fa',
            padding: '20px',
            borderRadius: '8px',
            marginBottom: '20px',
            fontSize: '16px',
            lineHeight: '1.6',
            color: '#495057',
            borderLeft: '4px solid #187ce1ff'
          }}>
            {item.answer}
          </div>

          {/* Теги */}
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '10px', marginBottom: '20px' }}>
            {item.tags.map((tag, index) => (
              <span
                key={index}
                style={{
                  background: '#e3f2fd',
                  color: '#1976d2',
                  padding: '5px 12px',
                  borderRadius: '20px',
                  fontSize: '12px',
                  fontWeight: '600'
                }}
              >
                {tag}
              </span>
            ))}
          </div>

          <div style={{ 
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            color: '#6c757d',
            fontSize: '14px',
            borderTop: '1px solid #ecf0f1',
            paddingTop: '15px'
          }}>
            <div>
              <strong>Автор:</strong> {item.author} • <strong>Дата:</strong> {item.date}
            </div>
            
            <div style={{ display: 'flex', gap: '15px' }}>
              <button
                onClick={() => handleEditAnswer(item.id)}
                style={{
                  padding: '8px 15px',
                  backgroundColor: 'transparent',
                  color: '#187ce1ff',
                  border: '1px solid #187ce1ff',
                  borderRadius: '6px',
                  fontSize: '14px',
                  fontWeight: '600',
                  cursor: 'pointer',
                  transition: 'all 0.3s ease'
                }}
                onMouseEnter={(e) => {
                  e.target.style.backgroundColor = '#187ce1ff';
                  e.target.style.color = 'white';
                }}
                onMouseLeave={(e) => {
                  e.target.style.backgroundColor = 'transparent';
                  e.target.style.color = '#187ce1ff';
                }}
              >
                Редактировать
              </button>
              
              <button
                onClick={() => handleDeleteAnswer(item.id)}
                style={{
                  padding: '8px 15px',
                  backgroundColor: 'transparent',
                  color: '#dc3545',
                  border: '1px solid #dc3545',
                  borderRadius: '6px',
                  fontSize: '14px',
                  fontWeight: '600',
                  cursor: 'pointer',
                  transition: 'all 0.3s ease'
                }}
                onMouseEnter={(e) => {
                  e.target.style.backgroundColor = '#dc3545';
                  e.target.style.color = 'white';
                }}
                onMouseLeave={(e) => {
                  e.target.style.backgroundColor = 'transparent';
                  e.target.style.color = '#dc3545';
                }}
              >
                Удалить
              </button>
            </div>
          </div>
        </div>
      ))}

      {/* Подсказка внизу */}
      <div style={{ 
        textAlign: 'center',
        color: '#6c757d',
        fontSize: '14px',
        marginTop: '40px',
        paddingTop: '20px',
        borderTop: '1px dashed #dee2e6'
      }}>
        Всего ответов: {answers.length}
      </div>
    </div>
  );
};

export default Answers;