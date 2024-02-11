using Microsoft.EntityFrameworkCore;
using QuestsBot.Kernel;
using QuestsBot.Telegram;
using QuestsBot.Telegram.Context;
using QuestsBot.Telegram.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Configuration.AddJsonFile("token.json");

builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddSingleton<DataBaseService>();
builder.Services.AddSingleton<QuestService>();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
optionsBuilder.UseNpgsql(connection);
builder.Services.AddScoped<ApplicationContext>(db 
    => new ApplicationContext(optionsBuilder.Options));

var host = builder.Build();
host.Run();