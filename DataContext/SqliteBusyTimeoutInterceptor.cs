using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataContext
{
    public class SqliteBusyTimeoutInterceptor : DbConnectionInterceptor
    {
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA busy_timeout = 5000;";
            command.ExecuteNonQuery();
        }
    }
}