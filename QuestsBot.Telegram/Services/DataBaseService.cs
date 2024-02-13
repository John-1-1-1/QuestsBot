using QuestsBot.Kernel.Tables;
using QuestsBot.Telegram.Context;
using Telegram.Bot.Types;

namespace QuestsBot.Telegram.Services;

public class DataBaseService {
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<DataBaseService> _logger;

    public DataBaseService(ILogger<DataBaseService> logger, IServiceProvider serviceProvider) {
        _logger = logger;
        var scope = serviceProvider.CreateScope();
        var applicationContext = scope.ServiceProvider.GetService<ApplicationContext>();

        if (applicationContext != null) {
            _applicationContext = applicationContext;
        }
        else {
            _logger.LogError(typeof(ApplicationContext) + " is null");
            throw new Exception(typeof(ApplicationContext) + " is null");
        }
    }

    public DataBaseUserState CreateUser(long chatId) {
        try {

            var user = new TelegramUser(chatId);
            
            
            var findUser = _applicationContext.TelegramUser
                .FirstOrDefault(u => u.TgId == user.TgId);
            
            if (findUser == null) {
                _applicationContext.TelegramUser.Add(user);
                _applicationContext.SaveChanges();
                return DataBaseUserState.Ok;
            }
            else {
                return DataBaseUserState.Exist;
            }
            
        }
        catch {
            _logger.LogError("AddUser: ApplicationContext incorrect");
            return DataBaseUserState.Error;
        }
    }

    public TelegramUser? GetUserByChatId(long chatId) {
        try {
            return _applicationContext.TelegramUser
                .FirstOrDefault(u => u.TgId == chatId);
        }
        catch {
            _logger.LogError("GetUserByChatId: ApplicationContext incorrect");
            return null;
        }
    }

    public void UpdateUser(TelegramUser user) {
        try {
            _applicationContext.TelegramUser.Update(user);
            _applicationContext.SaveChanges();
        }
        catch {
            _logger.LogError("UpdateUser: ApplicationContext incorrect");
        }
    }
}

public enum DataBaseUserState {
    None,
    Exist,
    Ok,
    Error
}