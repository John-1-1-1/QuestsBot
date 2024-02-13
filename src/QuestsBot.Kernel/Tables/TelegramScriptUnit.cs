namespace QuestsBot.Kernel.Tables;

public class TelegramScriptUnit {

    public string Question { get; set; }
    public string TrueAnswer{ get; set; }

    public ICollection<string> ListAnswers{ get; set; }

    public string PathToImage { get; set; } = string.Empty;
}