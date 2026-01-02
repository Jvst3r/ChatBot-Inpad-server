using ChatBotInpadserver.Data.DataBase;
using ChatBotInpadServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ChatBotInpadServer.Services
{
    // заготовка обработчика БД
    // через него будет происходить изменения БД
    public class DataBaseHandler
    {
        public AppDbContext db;
        public DataBaseHandler(AppDbContext _db)
        {
            db = _db;
        }

        /// <summary>
        /// По id отдаёт совет из БД
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <returns></returns>
        public async Task<KnowledgeItem?> GetKnowledgeItemAsync(int id)
        {
            try
            {
                return await db.KnowledgeItems
                    .Include(ki => ki.ChatMessages)
                    .FirstOrDefaultAsync(ki => ki.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex}\n НЕВОЗМОЖНО ПОЛУЧИТЬ СОВЕТ id({id}) ИЗ БД");
                return null;
            }
        }

        /// <summary>
        /// Отдаёт список советов с пагинацией(сортировка по страницам)
        /// </summary>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="category">Категория</param>
        /// <param name="searchQuery">Введенный запрос</param>
        /// <returns>Пустой список при ошибке с count = 0</returns>
        public async Task<(List<KnowledgeItem> Items, int TotalCount)> GetAllKnowledgeItemsAsync(
            int page = 1,
            int pageSize = 20,
            string? category = null,
            string? searchQuery = null)
        {
            try
            {
                var query = db.KnowledgeItems.AsQueryable();

                // Фильтрация по категории
                if (!string.IsNullOrEmpty(category))
                    query = query.Where(ki => ki.Category == category);

                // Поиск по тексту
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    searchQuery = searchQuery.ToLower();
                    query = query.Where(ki =>
                        ki.Title.ToLower().Contains(searchQuery) ||
                        ki.AnswerText.ToLower().Contains(searchQuery) ||
                        ki.Tags.ToLower().Contains(searchQuery));
                }

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(ki => ki.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}\n" +
                    $"НЕВОЗМОЖНО ПОЛУЧИТЬ СПИСОК СОВЕТОВ");
                return (new List<KnowledgeItem>(), 0);
            }
        }

        /// <summary>
        /// Добавляет в БД новый совет
        /// </summary>
        /// <param name="ki">Knowledge Item</param>
        /// <returns>null при ошибке, ki при успехе</returns>
        public async Task<KnowledgeItem?> CreateNewKnowledgeItemAsync(KnowledgeItem ki)
        {
            try
            {
                ki.CreatedAt = DateTime.UtcNow;
                ki.UpdatedAt = DateTime.UtcNow;

                db.KnowledgeItems.Add(ki);
                await db.SaveChangesAsync();
                return ki;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}\n" +
                    $"НЕВОЗМОЖНО ДОБАВИТЬ НОВЫЙ СОВЕТ!");
                return null;
            }
        }

        /// <summary>
        /// Изменяет СУЩЕСТВУЮЩИЙ совет из БД
        /// </summary>
        /// <param name="id">Id совета</param>
        /// <param name="UpdatedItem">Обновленная версия совета УЖЕ С ИЗМЕНЕНИЯМИ</param>
        /// <returns></returns>
        public async Task<KnowledgeItem?> UpdateKnowledgeItemAsync(int id, KnowledgeItem UpdatedItem)
        {
            try
            {
                var existingItem = await GetKnowledgeItemAsync(id);
                if (existingItem == null)
                {
                    Console.WriteLine("\nЭЛЕМЕНТ ДЛЯ ОБНОВЛЕНИЯ НЕ НАЙДЕН В БД!!!\n");
                    return null;
                }
                existingItem.Title = UpdatedItem.Title;
                existingItem.Category = UpdatedItem.Category;
                existingItem.AnswerText = UpdatedItem.AnswerText;
                existingItem.Tags = UpdatedItem.Tags;
                existingItem.UpdatedAt = DateTime.UtcNow;

                db.KnowledgeItems.Update(existingItem);
                await db.SaveChangesAsync();
                return existingItem;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex}\n" +
                    $"НЕВОЗМОЖНО ОБНОВИТЬ СОВЕТ id({id})!!!");
                return null;
            }
        }
        /// <summary>
        /// Удаляет из БД совет по ID. 
        /// </summary>
        /// <param name="id">Id совета, который нужно удалить</param>
        /// <returns>Возвращает true если совет удалён, false если нет.</returns>
        public async Task<bool> DeleteKnowledgeItemAsync(int id)
        {
            try
            {
                var item = await GetKnowledgeItemAsync(id);
                if (item == null)
                {
                    Console.WriteLine($"СОВЕТ ДЛЯ УДАЛЕНИЯ НЕ НАЙДЕН, id({id})!!!");
                    return false;
                }

                db.KnowledgeItems.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}" +
                    $"ОШИБКА ПРИ УДАЛЕНИИ СОВЕТА С id({id})!");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags">Строка с тегами через разделители</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страниц</param>
        /// <returns>Найденные вопросы с тегами</returns>
        public async Task<List<KnowledgeItem>> SearchByTagsAsync(string[] tags, int page = 1, int pageSize = 20)
        {
            try
            {
                var normalizedTags = tags.Select(t => t.ToLower().Trim()).ToList();

                var query = db.KnowledgeItems.AsQueryable();

                foreach (var tag in normalizedTags)
                {
                    query = query.Where(ki => ki.Tags.ToLower().Contains(tag));
                }

                return await query
                    .OrderByDescending(ki => ki.UseCount)
                    .ThenByDescending(ki => ki.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА ПРИ ПОИСКУ ПО ТЕГАМ:{tags}");
                return new List<KnowledgeItem>();
            }
        }

        /// <summary>
        /// Возвращает все существующие теги
        /// </summary>
        /// <returns>Возвращает все существующие теги</returns>
        public async Task<List<string>> GetAllTagsAsync()
        {
            try
            {
                var allItems = await db.KnowledgeItems.ToListAsync();
                var allTags = new List<string>();

                foreach (var item in allItems)
                {
                    var tags = item.GetTagList();
                    allTags.AddRange(tags);
                }

                return allTags
                    .Distinct()
                    .OrderBy(t => t)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ОШИБКА ПРИ ПОЛУЧЕНИИ ТЕГОВ");
                return new List<string>();
            }
        }

        
        /// <summary>
        /// Метод для поиска совета по запросу пользователя
        /// </summary>
        /// <param name="userQuery">Текст запроса</param>
        /// <returns>Наиболее подходящий совет по тексту.Null при ошибке. </returns>
        public async Task<KnowledgeItem?> SearchByQueryAsync(string userQuery)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userQuery))
                    return null;

                // Приводим запрос к нижнему регистру и разбиваем на слова
                var searchTerms = userQuery.ToLower()
                    .Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(term => term.Length > 2) // Игнорируем слишком короткие слова
                    .Select(term => term.Trim())
                    .Distinct()
                    .ToList();

                if (!searchTerms.Any())
                    return null;

                // Получаем все активные элементы знаний
                var allItems = await db.KnowledgeItems.ToListAsync();

                // Считаем релевантность для каждого элемента
                var scoredItems = allItems.Select(item =>
                {
                    var score = 0;
                    var title = item.Title.ToLower();
                    var answer = item.AnswerText.ToLower();
                    var tags = item.GetTagList();
                    var category = item.Category.ToLower();

                    foreach (var term in searchTerms)
                    {
                        // Разные веса для разных частей
                        if (title.Contains(term)) score += 3; // Заголовок очень важен
                        if (answer.Contains(term)) score += 2; // Ответ важен
                        if (tags.Any(t => t.Contains(term))) score += 4; // Теги наиболее важны
                        if (category.Contains(term)) score += 1; // Категория менее важна
                    }

                    return new { Item = item, Score = score };
                })
                .Where(x => x.Score > 0) // Отфильтровываем элементы без совпадений
                .OrderByDescending(x => x.Score) // Сортируем по релевантности
                .ThenByDescending(x => x.Item.UseCount) // При равной релевантности выбираем более популярные
                .ThenByDescending(x => x.Item.CreatedAt) // Затем по новизне
                .ToList();

                return scoredItems.FirstOrDefault()?.Item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА ПОИСКА ПО ЗАПРОСУ: {userQuery}");
                return null;
            }
        }

        /// <summary>
        /// Увеличивает каунтер у совета
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True - увеличен каунтер,false - ошибка</returns>
        public async Task<bool> IncrementUseCountAsync(int id)
        {
            try
            {
                var item = await GetKnowledgeItemAsync(id);
                if (item == null)
                    return false;

                item.IncrementUseCount();
                db.KnowledgeItems.Update(item);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"СЧЕТЧИК ДЛЯ ЭЛЕМЕНТА {id} НЕ УВЕЛИЧЕН!");
                return false;
            }
        }

        /// <summary>
        /// Возвращает все советы из категории( с пагинацией )
        /// </summary>
        /// <param name="category">Нужная категория</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns></returns>
        public async Task<List<KnowledgeItem>> GetItemsByCategoryAsync(string category, int page = 1, int pageSize = 20)
        {
            try
            {
                return await db.KnowledgeItems
                    .Where(ki => ki.Category == category)
                    .OrderByDescending(ki => ki.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"НЕ УДАЛОСЬ ПОЛУЧИТЬ ВОПРОСЫ ИЗ КАТЕГОРИИ {category}!!!");
                return new List<KnowledgeItem>();
            }
        }

        /// <summary>
        /// Даёт список всех категорий из БД
        /// </summary>
        /// <returns>Список категорий, при ошибке - пустой список</returns>
        public async Task<List<string>> GetAllCategoriesAsync()
        {
            try
            {
                return await db.KnowledgeItems
                    .Select(ki => ki.Category)
                    .Distinct()
                    .OrderBy(category => category)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ОШИБКА ПРИ ПОЛУЧЕНИИ КАТЕГОРИЙ!");
                return new List<string>();
            }
        }
    }
}
