using BookService.CommonEntities.Pathes;

namespace BookService.Configuration.Abstractions
{
    public interface ILogConfig
    {
        FilePath LogFilePath { get; }

        string Level { get; }
    }
}
