namespace QuestsBot.Kernel.Tables;

public class TelegramUser {
    public int Id { get; set; }
    public long TgId { get; set; }
    public int NumberQuestion { get; set; } = -1;

    public TelegramUser(long id) {
        TgId = id;
    }

    public TelegramUser() {
        
    }
}