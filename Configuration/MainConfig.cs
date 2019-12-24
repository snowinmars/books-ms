using BookService.Configuration.Abstractions;

namespace BookService.Configuration
{
    internal class MainConfig : IMainConfig
    {
        public MainConfig(IDatabaseConfig myDatabase, ILogConfig log)
        {
            MyDatabase = myDatabase;
            Log = log;
        }

        public IDatabaseConfig MyDatabase { get; }

        public ILogConfig Log { get; }
    }
}
