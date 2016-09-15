using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoAppRole : SxDbRepository<string, SxAppRole, SxVMAppRole>
    {
        public override SxVMAppRole[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM AspNetRoles AS anr ");

            object param = null;
            var gws = getAppRolesWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new System.Collections.Generic.Dictionary<string, string> {
                { "Name", "anr.[Name]" }
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM AspNetRoles AS anr ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMAppRole>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getAppRolesWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (anr.[Name] LIKE '%'+@name+'%' OR @name IS NULL)");
            query.Append(" AND (anr.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL)");

            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                name = name,
                desc = desc
            };

            return query.ToString();
        }
    }
}
