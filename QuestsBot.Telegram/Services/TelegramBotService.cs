using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace QuestsBot.Telegram.Services;

public class TelegramBotService {
    
    public readonly ITelegramBotClient Client;
    public readonly ReceiverOptions ReceiverOptions;
    
    public TelegramBotService(ILogger<TelegramBotService> logger,
         IConfiguration configuration) {
        
        var token = configuration.GetValue<string>("TelegramToken");

        if (token == null) {
            logger.LogError("TelegramToken is null");
            throw new ArgumentNullException(token);
        }
        Client = new TelegramBotClient(token);
        ReceiverOptions = new ReceiverOptions() {
            AllowedUpdates = new[] {
                UpdateType.Message,
                UpdateType.CallbackQuery 
            }
        };
    }
}