using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoAppUser<TDbContext> : SxDbRepository<string, SxAppUser, TDbContext, SxVMAppUser>
        where TDbContext: SxDbContext
    {
        public override SxVMAppUser[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM AspNetUsers AS anu ");

            object param = null;
            var gws = getAppUsersWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "NikName", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new System.Collections.Generic.Dictionary<string, string> {
                { "NikName", "anu.[NikName]" }
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM AspNetUsers AS anu ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMAppUser>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getAppUsersWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (anu.[NikName] LIKE '%'+@nikName+'%' OR @nikName IS NULL)");
            query.Append(" AND (anu.[Email] LIKE '%'+@email+'%' OR @email IS NULL)");

            var nikName = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NikName != null ? (string)filter.WhereExpressionObject.NikName : null;
            var email = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Email != null ? (string)filter.WhereExpressionObject.Email : null;

            param = new
            {
                nikName = nikName,
                email = email
            };

            return query.ToString();
        }

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
