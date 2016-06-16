using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoRedirect<TDbContext> : SxDbRepository<Guid, SxRedirect, TDbContext> where TDbContext : SxDbContext
    {
        /// <summary>
        /// Редирект страницы
        /// </summary>
        /// <returns></returns>
        public SxRedirect GetRedirect(string url)
        {
            SxRedirect result = new SxRedirect();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxRedirect>("get_redirect @url", new { url= url }).SingleOrDefault();
                if (data != null)
                    result = data;
            }

            return result;
        }
    }
}
