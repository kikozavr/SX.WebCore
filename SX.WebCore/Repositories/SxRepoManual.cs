using System.Linq;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using Dapper;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Text;
using SX.WebCore.ViewModels;

namespace SX.WebCore.Repositories
{
    public class SxRepoManual<TDbContext> : SxRepoMaterial<SxManual, SxVMMaterial, TDbContext> where TDbContext: SxDbContext
    {
        public SxRepoManual() : base(Enums.ModelCoreType.Manual) { }

        public override SxManual[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "da.Id",
                "dm.Title",
                "dm.CategoryId"
            }));
            sb.Append(@" FROM D_MANUAL AS da ");
            var joinString = @" JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType LEFT JOIN D_MATERIAL_CATEGORY as dmc on dmc.Id=dm.CategoryId ";
            sb.Append(joinString);

            object param = null;
            var gws = getManualWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "..DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_MANUAL AS da ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxManual>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getManualWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dm.Title LIKE '%'+@title+'%' OR @title IS NULL) ");

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;

            param = new
            {
                title = title
            };

            return query.ToString();
        }

        public virtual SxManual[] GetManualsByCategoryId(string categoryId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxManual, SxMaterialCategory, SxManual>("dbo.get_manuals_by_category @cat", (m, c)=> {
                    return m;
                }, param: new { cat= categoryId });
                return data.ToArray();
            }
        }

        public override void Delete(SxManual model)
        {
            var query = "DELETE FROM D_MANUAL WHERE Id=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = model.Id, mct = model.ModelCoreType });
            }

            base.Delete(model);
        }
    }
}
