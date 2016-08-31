using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepo301Redirect<TDbContext> : SxDbRepository<Guid, Sx301Redirect, TDbContext, SxVM301Redirect> where TDbContext : SxDbContext
    {
        public override SxVM301Redirect[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dr.Id",
                "dr.OldUrl",
                "dr.NewUrl",
                "dr.DateCreate"
            }));
            sb.Append(@" FROM D_REDIRECT AS dr ");

            object param = null;
            var gws = get301RedirectWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dr.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_REDIRECT AS dr ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVM301Redirect>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string get301RedirectWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dr.OldUrl LIKE '%'+@old_url+'%' OR @old_url IS NULL)");
            query.Append(" AND (dr.NewUrl LIKE '%'+@new_url+'%' OR @new_url IS NULL)");

            var oldUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.OldUrl != null ? (string)filter.WhereExpressionObject.OldUrl : null;
            var newUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NewUrl != null ? (string)filter.WhereExpressionObject.NewUrl : null;

            param = new
            {
                old_url = oldUrl,
                new_url = newUrl
            };

            return query.ToString();
        }

        /// <summary>
        /// Редирект страницы
        /// </summary>
        /// <returns></returns>
        public Sx301Redirect Get301Redirect(string url)
        {
            Sx301Redirect result = new Sx301Redirect();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<Sx301Redirect>("dbo.get_redirect @url", new { url= url }).SingleOrDefault();
                if (data != null)
                    result = data;
            }

            return result;
        }

        public override void Delete(Sx301Redirect model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_redirect @redirectId", new { redirectId = model.Id });
            }
        }
    }
}
