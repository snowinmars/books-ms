using System;
using System.Security;
using BookService.CommonEntities;
using BookService.Configuration.Abstractions;

namespace BookService.Configuration
{
    internal class DatabaseConfig : IDatabaseConfig
    {
        public DatabaseConfig(string host, int port, string username, string password, string databaseName)
        {
            Host = host;
            Port = port;
            Username = username.ToSecureString();
            Password = password.ToSecureString();
            DatabaseName = databaseName.ToSecureString();
        }

        public string Host { get; }

        public int Port { get; }

        public SecureString Username { get; }

        public SecureString Password { get; }

        public SecureString DatabaseName { get; }
    }
}
