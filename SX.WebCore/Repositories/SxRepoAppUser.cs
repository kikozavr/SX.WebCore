using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoAppUser<TDbContext>
    {
        public SxAppUser[] GetUsersByEmails(string[] emails)
        {
            if (!emails.Any()) return new SxAppUser[0];

            var sb = new StringBuilder();
            for (int i = 0; i < emails.Length; i++)
            {
                sb.AppendFormat(",'{0}'", emails[i]);
            }
            sb.Remove(0, 1);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
            {
                var data = conn.Query<SxAppUser>("get_users_by_emails @emails", new { emails = sb.ToString() });
                return data.ToArray();
            }
        }
    }
}
