using Dapper;
using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteSetting<TDbContext> : SxDbRepository<string, SxSiteSetting, TDbContext> where TDbContext : SxDbContext
    {
        public Dictionary<string, SxSiteSetting> GetByKeys(params string[] keys)
        {
            if (keys == null || !keys.Any()) return new Dictionary<string, SxSiteSetting>();

            var sb = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                sb.AppendFormat(",'{0}'", key);
            }

            sb.Remove(0, 1);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteSetting>("get_site_settings @keys", new { keys=sb.ToString()});
                return data.ToDictionary(x => x.Id);
            }
        }
    }
}
