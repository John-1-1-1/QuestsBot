using Microsoft.EntityFrameworkCore.Diagnostics;
using QuestsBot.Kernel;
using QuestsBot.Kernel.Tables;

namespace QuestsBot.Telegram.Services;

public class QuestService {
    private quest _telegramScriptUnits;

    public QuestService() {
        _telegramScriptUnits = JsonRider.JsonRide<quest>("quest.json")
                               ?? throw new Exception("Null quest.json");
    }

    public TelegramScriptUnit GetTextUnitByNumQuestion(int numQuestion) {
        if (numQuestion == -1) {
            return _telegramScriptUnits.StartMessage;
        }

        if (numQuestion >= _telegramScriptUnits.Quest.Count) {
            return _telegramScriptUnits.EndMessage;
        }
        return _telegramScriptUnits.Quest.Skip(numQuestion).First();
    }
}

public class quest {
    public TelegramScriptUnit StartMessage{ get; set; }
    public ICollection<TelegramScriptUnit> Quest{ get; set; }
    public TelegramScriptUnit EndMessage{ get; set; }
}