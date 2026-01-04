// создаём builder статическим методом из класса WebApplication
using ChatBot_Inpad_server.Services;
using ChatBotInpadserver.Data.DataBase;
using ChatBotInpadServer.Data;
using ChatBotInpadServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers(); // Поддержка контроллеров
builder.Services.AddEndpointsApiExplorer(); // Для Swagger
builder.Services.AddSwaggerGen(); // Документация API


// Сервис для работы с базой знаний
builder.Services.AddScoped<KnowledgeService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdminService>();

// Сервис для хеширования паролей
builder.Services.AddSingleton<PasswordHasherService>();

// Сервисы для Telegram бота
builder.Services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(Secrets.TgBotToken));
builder.Services.AddTransient<ITelegramBotService, TelegramBotService>();
builder.Services.AddHostedService<TelegramBotBackgroundService>();

// Сервисы для Revit`a
//builder.Services.AddSingleton<IRevitService, RevitBotService>();



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);


//ВОТ ТУТ ДО ПОСТРОЕНИЯ ЭКЗЕМПЛЯРА WebApplication НУЖНО НАСТРОИТЬ CORS,
//ЧТОБЫ ФРОНТ-ПРИЛОЖЕНИЕ МОГЛО РАБОТАТЬ С НАШИМ БЕКЕНД-ПРИЛОЖЕНИЕМ
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000", // React dev server
                    "http://localhost:5173", // Web dev server
                    "https://localhost:3000",
                    "https://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

//с помощью builder`a создаём экземпляр класса WebApplication (то есть наше приложение)
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        if (db.Database.CanConnect())
            Console.WriteLine("База данных успешно создана и заполнена\n" +
                $"Количество советов в таблице: {db.KnowledgeItems.Count()}");
        else
            Console.WriteLine("База данных не работает");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при создании БД: {ex.Message}");
    }
}

//Красивый вывод в хост
app.MapGet("/", () => "Bot is running!");
//app.MapControllers();


// Подключает документацию на этапе РАЗРАБОТКИ
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // сложно короче, но если приходит HTTP-запрос, то перенаправляет его на HTPPS-порт (защищённый)

//app.UseAuthorization(); //если мы будем использовать авторизацию, то эта команда нужна (разрешает авторизацию)

app.MapControllers(); //связываем Controllers с реальными адресами типо нам придёт:
                      //GET: ourserver/telegram/task/{id}
                      //и благодаря этой строчке на нашем сервере исполнится код из метода
                      //Controllers/TelegramController/GetTaskById

app.Run(); //запускаем наше веб-приложение
