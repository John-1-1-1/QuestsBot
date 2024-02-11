using System.Text.Json;
using QuestsBot.Kernel.Tables;

namespace QuestsBot.Kernel;

public static class JsonRider {
    public static  T? JsonRide<T>(string fileName) {
        using StreamReader fs = new StreamReader(fileName);
        var r = fs.ReadToEnd();
        T? iTelegramScriptUnits = JsonSerializer.Deserialize<T>(r);
        return iTelegramScriptUnits;
    }
}