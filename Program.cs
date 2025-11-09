// создаём builder статическим методом из класса WebApplication
using Services;

var builder = WebApplication.CreateBuilder(args);

//настраиваем будущее приложение
builder.Services.AddControllers(); // добавляем поддержку endpoint`ов, то есть тех самых API-шек
builder.Services.AddEndpointsApiExplorer(); // нужно для работы Swagger`a
builder.Services.AddSwaggerGen(); // добавляем генерацию документации с помощью Swagger
builder.Services.AddScoped<ITelegramBotService, TelegramBotService>(); //телега


//ВОТ ТУТ ДО ПОСТРОЕНИЯ ЭКЗЕМПЛЯРА WebApplication НУЖНО НАСТРОИТЬ CORS,
//ЧТОБЫ ФРОНТ-ПРИЛОЖЕНИЕ МОГЛО РАБОТАТЬ С НАШИМ БЕКЕНД-ПРИЛОЖЕНИЕ


//с помощью builder`a создаём экземпляр класса WebApplication (то есть наше приложение)
var app = builder.Build();

// Подключает документацию на этапе РАЗРАБОТКИ
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // сложно короче, но если приходит HTTP-запрос, то перенаправляет его на HTPPS-порт (защищённый)

app.UseAuthorization(); //если мы будем использовать авторизацию, то эта команда нужна (разрешает авторизацию)

app.MapControllers(); //связываем Controllers с реальными адресами типо нам придёт:
                      //GET: ourserver/telegram/task/{id}
                      //и благодаря этой строчке на нашем сервере исполнится код из метода
                      //Controllers/TelegramController/GetTaskById

app.Run(); //запускаем наше веб-приложение
