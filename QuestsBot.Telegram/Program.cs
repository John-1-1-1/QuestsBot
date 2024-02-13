using System.CommandLine;
using Microsoft.EntityFrameworkCore;
using QuestsBot.Kernel;
using QuestsBot.Telegram;
using QuestsBot.Telegram.Context;
using QuestsBot.Telegram.Services;


// var dirOption = new Option<string>
//     ("-dir", "An option whose argument is parsed as an int.");
// dirOption.AddAlias("-d");
//
// var rootCommand = new RootCommand("Parameter binding example");
// rootCommand.Add(dirOption);
//
// rootCommand.SetHandler(
//     (workingDir) => {
//         
//     },
//     dirOption);
// await rootCommand.InvokeAsync(args);

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