var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку контроллеров
builder.Services.AddControllers();

// Добавляем поддержку OpenAPI (документация API)
builder.Services.AddOpenApi();

var app = builder.Build();

// ==================== КОНФИГУРАЦИЯ ПАЙПЛАЙНА ====================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                    // ← Это важно!
    // Можно также добавить удобный UI:
    // app.UseSwaggerUI();               // если захотим классический Swagger UI
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();                    // ← Подключаем все контроллеры

app.Run();