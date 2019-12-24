using BookService.CommonEntities.Pathes;
using BookService.Configuration.Abstractions;

namespace BookService.Configuration
{
    internal sealed class LogConfig : ILogConfig
    {
        public LogConfig(FilePath logFilePath, string level)
        {
            LogFilePath = logFilePath;
            Level = level;
        }

        public FilePath LogFilePath { get; }

        public string Level { get; }
    }
}
