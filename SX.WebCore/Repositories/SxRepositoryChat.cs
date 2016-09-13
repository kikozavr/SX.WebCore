using Dapper;
using SX.WebCore.ViewModels;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepositoryChat<TDbContext> where TDbContext : SxDbContext
    {
        public SxVMAppUser[] OnlineUsers
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;
                if (connStr == null) return new SxVMAppUser[0];
                var values = MvcApplication.SxMvcApplication<TDbContext>.UsersOnSite.Values;
                if (values.Count == 0) return new SxVMAppUser[0];

                var sb = new StringBuilder();
                foreach (var email in values)
                {
                    sb.AppendFormat(",'{0}'", email);
                }
                sb.Remove(0, 1);

                using (var conn = new SqlConnection(connStr))
                {
                    var data = conn.Query<SxVMAppUser>("dbo.get_users_by_emails @emails", new { emails = sb.ToString() });
                    if(data.Any())
                        foreach (var item in data)
                        {
                            item.IsOnline = values.Contains(item.Email);
                        }
                    return data.ToArray();
                }
            }
        }
    }
}
