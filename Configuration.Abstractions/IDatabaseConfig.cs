using System;
using System.Security;

namespace EmptyService.Configuration.Abstractions
{
    public interface IDatabaseConfig
    {
        string Host { get; }

        int Port { get; }

        SecureString Username { get; }

        SecureString Password { get; }

        SecureString DatabaseName { get; }
    }
}
