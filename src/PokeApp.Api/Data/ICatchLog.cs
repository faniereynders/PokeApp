using PokeApp.Api.Models;
using System.Collections.Generic;

namespace PokeApp.Api.Data
{
    public interface ICatchLog
    {
        IEnumerable<LogItem> GetAllFrom(int fromId, int limit);
        LogItem AddEntry(LogEntry entry);
    }
}
