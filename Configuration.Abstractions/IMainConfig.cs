﻿namespace BookService.Configuration.Abstractions
{
    public interface IMainConfig
    {
        IDatabaseConfig MyDatabase { get; }

        ILogConfig Log { get; }
    }
}
