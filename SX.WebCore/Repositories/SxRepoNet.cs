using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System;
using System.Text;
using SX.WebCore.ViewModels;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoNet<TDbContext> : SxDbRepository<int, SxNet, TDbContext, SxVMNet> where TDbContext : SxDbContext
    {
        public override SxVMNet[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM D_NET AS dn ");

            object param = null;
            var gws = getNetsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dn.Name", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_NET AS dn ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMNet>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getNetsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dn.Code LIKE '%'+@code+'%' OR @code IS NULL)");
            query.Append(" AND (dn.Name LIKE '%'+@name+'%' OR @name IS NULL)");
            query.Append(" AND (dn.Url LIKE '%'+@url+'%' OR @url IS NULL)");
            query.Append(" AND (dn.LogoCssClass LIKE '%'+@logoCssClass+'%' OR @logoCssClass IS NULL)");
            query.Append(" AND (dn.Color LIKE '%'+@color+'%' OR @color IS NULL)");

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

            return query.ToString();
        }

        public override SxNet Create(SxNet model)
        {
            throw new NotImplementedException("Создание сети не поддерживается");
        }

        public override SxNet Update(SxNet model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            throw new NotImplementedException("Обновление сети не поддерживается");
        }

        public sealed override void Delete(SxNet model)
        {
            throw new NotImplementedException("Удаление сети не поддерживается");
        }
    }
}
