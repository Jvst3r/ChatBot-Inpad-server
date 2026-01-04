using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChatBot_Inpad_server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "Общие вопросы"),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: false),
                    UseCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Tags = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, defaultValue: "revit")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Telegram"),
                    PlatformId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    TextMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Telegram"),
                    IsFromUser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    KnowledgeItemId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_KnowledgeItems_KnowledgeItemId",
                        column: x => x.KnowledgeItemId,
                        principalTable: "KnowledgeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Email", "LastLoginAt", "PasswordHash" },
                values: new object[] { "admin@example.com", null, "$2a$11$gH8gaDXr0TPHX2UK64.SZOymfKZsNQ4xGvhA4zL3Okx.j3iTyCxLW" });

            migrationBuilder.InsertData(
                table: "KnowledgeItems",
                columns: new[] { "Id", "AnswerText", "Category", "CreatedAt", "Tags", "Title" },
                values: new object[,]
                {
                    { 1, "Для начала работы просто отправьте любое сообщение боту. Он ответит на ваши вопросы и поможет с настройками.", "Общие вопросы", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3455), "начало,помощь,бот", "Как начать работу с ботом?" },
                    { 2, "1. Откройте вкладку 'Architecture'\n2. Нажмите на инструмент 'Wall'\n3. Выберите тип стены\n4. Укажите точки размещения на плане", "Revit", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3459), "revit,стена,создание", "Как создать новую стену в Revit?" },
                    { 3, "🔒 Всегда проверяйте сертификаты сервера перед подключением к VPN.", "Безопасность", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3461), "vpn,безопасность,сертификаты", "Безопасное подключение" },
                    { 4, "🚀 Выбирайте сервер ближайший к вашему местоположению для лучшей скорости.", "Производительность", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3464), "vpn,сервер,скорость,оптимизация", "Оптимальный сервер" },
                    { 5, "📡 Всегда используйте VPN при подключении к публичным Wi-Fi сетям.", "Безопасность", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3466), "vpn,безопасность,wi-fi,публичные сети", "Wi-Fi защита" },
                    { 6, "📊 Если скорость низкая, попробуйте переключиться на другой сервер или протокол.", "Производительность", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3468), "vpn,скорость,сервер,протокол,оптимизация", "Скорость соединения" },
                    { 7, "📱 Мой совет это - включить акселератор.", "Общие советы", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3470), "советы,производительность,акселератор,оптимизация", "Включение акселератора" },
                    { 8, "📱 Можете проверить диск C.", "Общие советы", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3472), "советы,диск,память,оптимизация", "Проверка диска" },
                    { 9, "📱 Попробуйте закрыть вкладки браузера.", "Общие советы", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3475), "советы,браузер,память,оптимизация", "Закрытие вкладок" },
                    { 10, "📱 Незабудьте проверить рабочие наборы.", "Revit", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3477), "revit,советы,рабочие наборы,оптимизация", "Проверка рабочих наборов" },
                    { 11, "📱 Советую удалить неиспользуемые группы, виды и DWG.", "Revit", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3479), "revit,советы,очистка,оптимизация", "Удаление неиспользуемых элементов" },
                    { 12, "📱 Так же советую проверить вес файла хранилища.", "Revit", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3481), "revit,советы,память,оптимизация", "Проверка веса файла" },
                    { 13, "📱 Отключите аналитические данные.", "Revit", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3484), "revit,советы,аналитика,оптимизация", "Отключение аналитики" },
                    { 14, "📱 Используйте корпоративные семейства.", "Revit", new DateTime(2026, 1, 3, 11, 43, 59, 34, DateTimeKind.Utc).AddTicks(3486), "revit,советы,семейства,стандартизация", "Корпоративные семейства" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_CreatedAt",
                table: "ChatMessages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_IsFromUser",
                table: "ChatMessages",
                column: "IsFromUser");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_KnowledgeItemId",
                table: "ChatMessages",
                column: "KnowledgeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_Platform",
                table: "ChatMessages",
                column: "Platform");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_Platform_Time",
                table: "ChatMessages",
                columns: new[] { "Platform", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_User_Time",
                table: "ChatMessages",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserId",
                table: "ChatMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeItems_Category",
                table: "KnowledgeItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeItems_Category_CreatedAt",
                table: "KnowledgeItems",
                columns: new[] { "Category", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeItems_Category_UseCount",
                table: "KnowledgeItems",
                columns: new[] { "Category", "UseCount" });

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeItems_CreatedAt",
                table: "KnowledgeItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeItems_UpdatedAt",
                table: "KnowledgeItems",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeItems_UseCount",
                table: "KnowledgeItems",
                column: "UseCount");

            migrationBuilder.CreateIndex(
                name: "IX_User_Platform_PlatformId",
                table: "Users",
                columns: new[] { "Platform", "PlatformId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "KnowledgeItems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
