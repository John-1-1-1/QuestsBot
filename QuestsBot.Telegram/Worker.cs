using QuestsBot.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuestsBot.Telegram;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService {
    private readonly DataBaseService _dataBaseService =
        serviceProvider.GetService<DataBaseService>() ?? 
        throw new Exception("DataBaseService is null");
    
    private readonly QuestService _questService =
        serviceProvider.GetService<QuestService>() ?? 
        throw new Exception("QuestService is null");
    
    private readonly TelegramBotService _telegramBotService = 
        serviceProvider.GetService<TelegramBotService>() ?? 
        throw new Exception("TelegramBotService is null");

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        logger.LogInformation("TelegramWorker running at: {time}", DateTimeOffset.Now);
        _telegramBotService.Client.StartReceiving(UpdateHandler, ErrorHandler,
            _telegramBotService.ReceiverOptions, stoppingToken);
        return Task.CompletedTask;
    }

    private Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3) {
        throw new NotImplementedException();
    }

    private Task UpdateHandler(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken) {

        if (update.Type == UpdateType.Message) {
            
            var user = _dataBaseService.GetUserByChatId(update.Message.Chat.Id);
            
            if (update.Message?.Text == "/start") {
                var state = _dataBaseService.CreateUser(update.Message.Chat.Id);
                if (state == DataBaseUserState.Exist) {
                    _telegramBotService.Client.SendTextMessageAsync(
                        update.Message.Chat.Id, "Вы уже зарегестрированы");
                }
                else {
                    _telegramBotService.Client.SendTextMessageAsync(
                        update.Message.Chat.Id, "Вы успешно зарегестрированы");
                }
            }
            
            if (update.Message?.Text == "/help") {
                
                
                return Task.CompletedTask;
            }

            if (update.Message?.Text == "/restart") {
                user.NumberQuestion = -1;
                _dataBaseService.UpdateUser(user);
            }

           
            if (user == null) {
                // написать боту start
                return Task.CompletedTask;
            }
           
            var telegramScriptUnit =
                _questService.GetTextUnitByNumQuestion(user.NumberQuestion);
            
            if (update.Message.Text == telegramScriptUnit.TrueAnswer) {
                user.NumberQuestion += 1;
                _dataBaseService.UpdateUser(user);
            }

            telegramScriptUnit =
                _questService.GetTextUnitByNumQuestion(user.NumberQuestion);
            
            _telegramBotService.Client.SendTextMessageAsync(
                update.Message.Chat.Id, telegramScriptUnit.Question);
            
            if (telegramScriptUnit.PathToImage != "") {
                var fs = System.IO.File.OpenRead(telegramScriptUnit.PathToImage);
                _telegramBotService.Client.SendPhotoAsync( 
                    chatId: update.Message.Chat,
                    photo: InputFile.FromStream(fs)
                );
            }

        }
        return Task.CompletedTask;
    }
}
    