using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buldo.Ngb.Web.Helpers
{
    /// <summary>
    /// Хэлперы баз данных
    /// </summary>
    public static class DbHelpers
    {
        /// <summary>
        /// Перевод строки подключения, которую даёт хероку, в ту, что нужна нам
        /// </summary>
        /// <param name="databaseUrl">
        /// The database url.
        /// </param>
        /// <returns>
        /// Строка подключения, подходящая нам
        /// </returns>
        public static string DatabaseUrlToPostgreConnectionString(string databaseUrl)
        {
            var connectionStringFormat = "Host={0};Port={1};Database={2};Username={3};Password={4};";
            var url = new Uri(databaseUrl);
            var userInfo = url.UserInfo.Split(new[] { ':' }, 2);
            var userName = userInfo[0];
            var password = userInfo[1];
            var host = url.Host;
            var port = url.Port;
            var databaseName = url.AbsolutePath.Remove(0, 1);
            return string.Format(connectionStringFormat, host, port, databaseName, userName, password);
        }
    }
}
