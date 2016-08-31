using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteSetting<TDbContext> : SxDbRepository<string, SxSiteSetting, TDbContext, SxVMSiteSetting> where TDbContext : SxDbContext
    {
        public override SxSiteSetting GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<SxSiteSetting>("SELECT * FROM D_SITE_SETTING AS dss WHERE dss.Id=@id", new { id = id[0] }).SingleOrDefault();
            }
        }

        public Dictionary<string, SxSiteSetting> GetAll()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteSetting>("dbo.get_site_settings");
                return data.ToDictionary(x => x.Id);
            }
        }

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
                var data = conn.Query<SxSiteSetting>("dbo.get_site_settings_by_keys @keys", new { keys = sb.ToString() });
                return data.ToDictionary(x => x.Id);
            }
        }
    }
}
