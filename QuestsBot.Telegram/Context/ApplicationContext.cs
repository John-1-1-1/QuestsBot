using Microsoft.EntityFrameworkCore;
using QuestsBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace QuestsBot.Telegram.Context;

public sealed class ApplicationContext : DbContext {
    public DbSet<TelegramUser> TelegramUser { get; set; } = null!;

    public ApplicationContext(DbContextOptions<DbContext> options):  base(options) {
        Database.EnsureCreated();
    }
}