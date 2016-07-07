using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoNet<TDbContext> : SxDbRepository<int, SxNet, TDbContext> where TDbContext : SxDbContext
    {
        public override SxNet[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += " FROM D_NET dn";

            object param = null;
            query += getNetsWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dn.Name", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order ?? defaultOrder);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxNet>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_NET AS dn";

            object param = null;
            query += getNetsWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<int>(query, param: param).Single();
            }
        }

        private static string getNetsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dn.Code LIKE '%'+@code+'%' OR @code IS NULL)";
            query += " AND (dn.Name LIKE '%'+@name+'%' OR @name IS NULL)";
            query += " AND (dn.Url LIKE '%'+@url+'%' OR @url IS NULL)";
            query += " AND (dn.LogoCssClass LIKE '%'+@logoCssClass+'%' OR @logoCssClass IS NULL)";
            query += " AND (dn.Color LIKE '%'+@color+'%' OR @color IS NULL)";

            var code = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Code != null ? (string)filter.WhereExpressionObject.Code : null;
            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;
            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;
            var logoCssClass = filter.WhereExpressionObject != null && filter.WhereExpressionObject.LogoCssClass != null ? (string)filter.WhereExpressionObject.LogoCssClass : null;
            var color = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Color != null ? (string)filter.WhereExpressionObject.Color : null;

            param = new
            {
                code = code,
                name = name,
                url = url,
                logoCssClass = logoCssClass,
                color = color
            };

            return query;
        }

        public override SxNet Create(SxNet model)
        {
            throw new NotImplementedException("Создание сети не поддерживается");
        }

        public override SxNet Update(SxNet model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            throw new NotImplementedException("Обновление сети не поддерживается");
        }

        public override void Delete(params object[] id)
        {
            throw new NotImplementedException("Удаление сети не поддерживается");
        }
    }
}
