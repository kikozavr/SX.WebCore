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
    public sealed class SxRepoRedirect<TDbContext> : SxDbRepository<Guid, SxRedirect, TDbContext, SxVMRedirect> where TDbContext : SxDbContext
    {
        /// <summary>
        /// Добавить редирект страницы
        /// </summary>
        /// <param name="model">Новая модель редиректа</param>
        /// <returns></returns>
        public override SxRedirect Create(SxRedirect model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxRedirect>("dbo.add_redirect @oldUrl, @newUrl", new { oldUrl = model.OldUrl, newUrl = model.NewUrl });
                return data.SingleOrDefault();
            }
        }

        public override SxVMRedirect[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dr.*",
                "CASE WHEN dr2.Id IS NOT NULL THEN 1 ELSE 0 END AS IsDuplicate"
            }, isDistinct: true));
            sb.Append(@" FROM D_REDIRECT AS dr LEFT JOIN D_REDIRECT AS dr2 ON dr2.NewUrl=dr.NewUrl AND dr2.Id NOT IN (dr.Id) ");

            object param = null;
            var gws = getRedirectWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new System.Collections.Generic.Dictionary<string, string> {
                ["DateCreate"] = "dr.DateCreate"
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(DISTINCT dr.Id) FROM D_REDIRECT AS dr LEFT JOIN D_REDIRECT AS dr2 ON dr2.NewUrl=dr.NewUrl AND dr2.Id NOT IN (dr.Id) ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMRedirect>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getRedirectWhereString(SxFilter filter, out object param)
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
        /// Обновить редирект страницы
        /// </summary>
        /// <param name="model">Обновляемый редирект</param>
        /// <returns></returns>
        public override SxRedirect Update(SxRedirect model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxRedirect>("dbo.update_redirect @redirectId, @oldUrl, @newUrl", new { redirectId = model.Id, oldUrl = model.OldUrl, newUrl = model.NewUrl });
                return data.SingleOrDefault();
            }
        }

        /// <summary>
        /// Удалить редирект
        /// </summary>
        /// <param name="model">Удаляемый редирект</param>
        public override void Delete(SxRedirect model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_redirect @redirectId", new { redirectId = model.Id });
            }
        }
        
        /// <summary>
        /// Получить редирект текущей страницы
        /// </summary>
        /// <param name="rawUrl">Относительный url страницы (RawUrl)</param>
        /// <returns></returns>
        public SxRedirect GetPageRedirect(string rawUrl)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxRedirect>("dbo.get_page_redirect @rawUrl", new { rawUrl = rawUrl });
                return data.SingleOrDefault();
            }
        }
    }
}
