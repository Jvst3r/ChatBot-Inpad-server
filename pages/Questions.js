import React, { useState } from 'react';
import { Link } from 'react-router-dom';

const Questions = () => {
  const [questions, setQuestions] = useState([
    {
      id: 1,
      question: "Как настроить параметры в BIM-модели компании?",
      status: "Ожидает ответ",
      date: "15 декабря 2025",
      priority: "Высокий",
      tags: ["BIM", "Revit", "Параметры"],
      author: "Инженер А. Петров"
    },
    {
      id: 2,
      question: "Ошибка при экспорте модели Revit в формате IFC",
      status: "Есть ответ",
      date: "10 декабря 2025",
      priority: "Средний",
      tags: ["Revit", "IFC", "Экспорт"],
      author: "Проектировщик И. Сидоров"
    },
    {
      id: 3,
      question: "Какие стандарты соответствия необходимо применять?",
      status: "Ожидает ответ",
      date: "8 декабря 2025",
      priority: "Высокий",
      tags: ["Стандарты", "Соответствие"],
      author: "Инженер С. Иванов"
    },
    {
      id: 4,
      question: "Как избежать типовых ошибок при моделировании конструкций?",
      status: "Ожидает ответ",
      date: "5 декабря 2025",
      priority: "Низкий",
      tags: ["Моделирование", "Ошибки", "Конструкции"],
      author: "Проектировщик П. Васильев"
    }
  ]);

  const [filter, setFilter] = useState("Все статусы");
  const [newQuestion, setNewQuestion] = useState("");
  const [showAddForm, setShowAddForm] = useState(false);

  const statusOptions = ["Все статусы", "Ожидает ответ", "Есть ответ", "В работе", "Завершено"];
  
  const filteredQuestions = filter === "Все статусы" 
    ? questions 
    : questions.filter(q => q.status === filter);

  const handleAddQuestion = () => {
    if (!newQuestion.trim()) {
      alert('Введите вопрос');
      return;
    }

    const newQuestionObj = {
      id: questions.length + 1,
      question: newQuestion,
      status: "Ожидает ответ",
      date: new Date().toLocaleDateString('ru-RU', { 
        day: 'numeric', 
        month: 'long', 
        year: 'numeric' 
      }),
      priority: "Средний",
      tags: ["Новый"],
      author: "Автор неизвестен"
    };

    setQuestions([...questions, newQuestionObj]);
    setNewQuestion("");
    setShowAddForm(false);
  };

  const handleStatusChange = (id, newStatus) => {
    setQuestions(questions.map(q => 
      q.id === id ? { ...q, status: newStatus } : q
    ));
  };

  const getStatusColor = (status) => {
    switch(status) {
      case "Ожидает ответ": return "#ff6b6b";
      case "Есть ответ": return "#51cf66";
      case "В работе": return "#339af0";
      case "Завершено": return "#868e96";
      default: return "#adb5bd";
    }
  };

  const getPriorityColor = (priority) => {
    switch(priority) {
      case "Высокий": return "#ff6b6b";
      case "Средний": return "#fcc419";
      case "Низкий": return "#51cf66";
      default: return "#adb5bd";
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1 style={{ 
        color: '#2c3e50', 
        marginBottom: '20px',
        paddingBottom: '15px',
        fontSize: '28px'
      }}>
        БАЗА ВОПРОСОВ
      </h1>

      <div style={{ 
        background: '#f8f9fa',
        padding: '25px',
        borderRadius: '12px',
        marginBottom: '30px',
        fontSize: '16px',
        lineHeight: '1.6',
        color: '#495057'
      }}>
        <h2 style={{ 
          color: '#2c3e50',
          marginBottom: '15px',
          fontSize: '20px'
        }}>
          Проблемы
        </h2>
        <div style={{ display: 'flex', gap: '30px', marginBottom: '20px' }}>
          <div>
            <div style={{ color: '#6c757d', fontSize: '14px', marginBottom: '5px' }}>
              Дата/срок
            </div>
            <div style={{ color: '#2c3e50', fontWeight: '600' }}>
              Последние 30 дней
            </div>
          </div>
          <div>
            <div style={{ color: '#6c757d', fontSize: '14px', marginBottom: '5px' }}>
              Завершено
            </div>
            <div style={{ color: '#2c3e50', fontWeight: '600' }}>
              {questions.filter(q => q.status === "Завершено").length} из {questions.length}
            </div>
          </div>
        </div>

        <div style={{ marginBottom: '20px' }}>
          <div style={{ color: '#6c757d', fontSize: '14px', marginBottom: '10px' }}>
            Отметки
          </div>
          <div style={{ display: 'flex', gap: '10px', flexWrap: 'wrap' }}>
            <span style={{
              background: '#e3f2fd',
              color: '#1976d2',
              padding: '5px 15px',
              borderRadius: '20px',
              fontSize: '12px',
              fontWeight: '600'
            }}>
              BIM
            </span>
            <span style={{
              background: '#f3e5f5',
              color: '#7b1fa2',
              padding: '5px 15px',
              borderRadius: '20px',
              fontSize: '12px',
              fontWeight: '600'
            }}>
              Revit
            </span>
            <span style={{
              background: '#e8f5e9',
              color: '#388e3c',
              padding: '5px 15px',
              borderRadius: '20px',
              fontSize: '12px',
              fontWeight: '600'
            }}>
              Стандарты
            </span>
          </div>
        </div>
      </div>

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
          fontSize: '22px'
        }}>
          Вопросы
        </h2>

        <div style={{ 
          color: '#6c757d',
          fontSize: '16px',
          lineHeight: '1.6',
          marginBottom: '25px'
        }}>
          Управление вопросами от инженеров и проектировщиков, связанными с BIM-стандартами компании, технологиями проектирования, работой в Revit и устранением типовых ошибок.
        </div>

        {/* Фильтры и кнопки */}
        <div style={{ 
          display: 'flex', 
          justifyContent: 'space-between',
          alignItems: 'center',
          marginBottom: '25px',
          flexWrap: 'wrap',
          gap: '15px'
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '15px' }}>
            <span style={{ color: '#2c3e50', fontWeight: '600' }}>Статус:</span>
            <select
              value={filter}
              onChange={(e) => setFilter(e.target.value)}
              style={{
                padding: '8px 15px',
                border: '2px solid #e0e0e0',
                borderRadius: '6px',
                fontSize: '14px',
                color: '#2c3e50',
                backgroundColor: 'white',
                cursor: 'pointer',
                outline: 'none',
                minWidth: '150px'
              }}
            >
              {statusOptions.map(option => (
                <option key={option} value={option}>{option}</option>
              ))}
            </select>
          </div>

          <button
            onClick={() => setShowAddForm(!showAddForm)}
            style={{
              padding: '10px 20px',
              backgroundColor: showAddForm ? '#6c757d' : '#187ce1ff',
              color: 'white',
              border: 'none',
              borderRadius: '6px',
              fontSize: '14px',
              fontWeight: '600',
              cursor: 'pointer',
              transition: 'all 0.3s ease'
            }}
          >
            {showAddForm ? 'Отменить' : '+ Новый вопрос'}
          </button>
        </div>

        {/* Форма добавления вопроса */}
        {showAddForm && (
          <div style={{ 
            background: '#f8f9fa',
            padding: '20px',
            borderRadius: '8px',
            marginBottom: '25px',
            border: '2px dashed #dee2e6'
          }}>
            <h3 style={{ color: '#2c3e50', marginBottom: '15px', fontSize: '18px' }}>
              Новый вопрос
            </h3>
            <textarea
              value={newQuestion}
              onChange={(e) => setNewQuestion(e.target.value)}
              placeholder="Введите новый вопрос..."
              rows="3"
              style={{
                width: '100%',
                padding: '12px 15px',
                border: '2px solid #e0e0e0',
                borderRadius: '6px',
                fontSize: '14px',
                color: '#2c3e50',
                backgroundColor: 'white',
                marginBottom: '15px',
                resize: 'vertical',
                outline: 'none'
              }}
            />
            <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
              <button
                onClick={() => setShowAddForm(false)}
                style={{
                  padding: '8px 20px',
                  backgroundColor: '#6c757d',
                  color: 'white',
                  border: 'none',
                  borderRadius: '6px',
                  fontSize: '14px',
                  fontWeight: '600',
                  cursor: 'pointer'
                }}
              >
                Отмена
              </button>
              <button
                onClick={handleAddQuestion}
                style={{
                  padding: '8px 25px',
                  backgroundColor: '#28a745',
                  color: 'white',
                  border: 'none',
                  borderRadius: '6px',
                  fontSize: '14px',
                  fontWeight: '600',
                  cursor: 'pointer'
                }}
              >
                Добавить
              </button>
            </div>
          </div>
        )}

        {/* Список вопросов */}
        <div style={{ overflowX: 'auto' }}>
          <table style={{ 
            width: '100%',
            borderCollapse: 'collapse'
          }}>
            <thead>
              <tr style={{ 
                borderBottom: '2px solid #e9ecef',
                textAlign: 'left'
              }}>
                <th style={{ 
                  padding: '12px 15px',
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  minWidth: '50px'
                }}>
                  #
                </th>
                <th style={{ 
                  padding: '12px 15px',
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  minWidth: '300px'
                }}>
                  Вопрос
                </th>
                <th style={{ 
                  padding: '12px 15px',
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  minWidth: '120px'
                }}>
                  Статус
                </th>
                <th style={{ 
                  padding: '12px 15px',
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  minWidth: '120px'
                }}>
                  Приоритет
                </th>
                <th style={{ 
                  padding: '12px 15px',
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  minWidth: '150px'
                }}>
                  Дата
                </th>
                <th style={{ 
                  padding: '12px 15px',
                  color: '#2c3e50',
                  fontWeight: '600',
                  fontSize: '14px',
                  minWidth: '150px'
                }}>
                  Действия
                </th>
              </tr>
            </thead>
            <tbody>
              {filteredQuestions.map((q) => (
                <tr key={q.id} style={{ 
                  borderBottom: '1px solid #e9ecef',
                  transition: 'background 0.2s ease'
                }}>
                  <td style={{ 
                    padding: '15px',
                    color: '#6c757d',
                    fontWeight: '600'
                  }}>
                    #{q.id}
                  </td>
                  <td style={{ padding: '15px' }}>
                    <div style={{ fontWeight: '500', color: '#2c3e50', marginBottom: '5px' }}>
                      {q.question}
                    </div>
                    <div style={{ fontSize: '12px', color: '#6c757d' }}>
                      Автор: {q.author}
                    </div>
                    <div style={{ display: 'flex', gap: '5px', flexWrap: 'wrap', marginTop: '5px' }}>
                      {q.tags.map((tag, index) => (
                        <span key={index} style={{
                          background: '#e9ecef',
                          color: '#495057',
                          padding: '2px 8px',
                          borderRadius: '10px',
                          fontSize: '11px'
                        }}>
                          {tag}
                        </span>
                      ))}
                    </div>
                  </td>
                  <td style={{ padding: '15px' }}>
                    <select
                      value={q.status}
                      onChange={(e) => handleStatusChange(q.id, e.target.value)}
                      style={{
                        padding: '6px 10px',
                        border: '1px solid',
                        borderColor: getStatusColor(q.status),
                        borderRadius: '4px',
                        fontSize: '12px',
                        color: getStatusColor(q.status),
                        backgroundColor: `${getStatusColor(q.status)}15`,
                        cursor: 'pointer',
                        outline: 'none',
                        fontWeight: '600'
                      }}
                    >
                      {statusOptions.filter(opt => opt !== "Все статусы").map(option => (
                        <option key={option} value={option}>{option}</option>
                      ))}
                    </select>
                  </td>
                  <td style={{ padding: '15px' }}>
                    <span style={{
                      padding: '4px 10px',
                      borderRadius: '4px',
                      fontSize: '12px',
                      fontWeight: '600',
                      backgroundColor: `${getPriorityColor(q.priority)}20`,
                      color: getPriorityColor(q.priority)
                    }}>
                      {q.priority}
                    </span>
                  </td>
                  <td style={{ padding: '15px', color: '#6c757d', fontSize: '14px' }}>
                    {q.date}
                  </td>
                  <td style={{ padding: '15px' }}>
                    <div style={{ display: 'flex', gap: '8px' }}>
                      <Link to={`/answers`}>
                        <button
                          style={{
                            padding: '6px 12px',
                            backgroundColor: '#187ce1ff',
                            color: 'white',
                            border: 'none',
                            borderRadius: '4px',
                            fontSize: '12px',
                            fontWeight: '600',
                            cursor: 'pointer'
                          }}
                        >
                          Ответить
                        </button>
                      </Link>
                      <button
                        onClick={() => alert(`Просмотр вопроса #${q.id}`)}
                        style={{
                          padding: '6px 12px',
                          backgroundColor: 'transparent',
                          color: '#6c757d',
                          border: '1px solid #dee2e6',
                          borderRadius: '4px',
                          fontSize: '12px',
                          fontWeight: '600',
                          cursor: 'pointer'
                        }}
                      >
                        Подробнее
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Статистика */}
        <div style={{ 
          marginTop: '30px',
          padding: '20px',
          background: '#f8f9fa',
          borderRadius: '8px',
          fontSize: '14px',
          color: '#495057'
        }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', flexWrap: 'wrap', gap: '15px' }}>
            <div>
              <strong>Всего вопросов:</strong> {questions.length}
            </div>
            <div>
              <strong>Ожидают ответа:</strong> {questions.filter(q => q.status === "Ожидает ответ").length}
            </div>
            <div>
              <strong>Отвечено:</strong> {questions.filter(q => q.status === "Есть ответ").length}
            </div>
            <div>
              <strong>В работе:</strong> {questions.filter(q => q.status === "В работе").length}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Questions;