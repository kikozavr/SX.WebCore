using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoEmployee<TDbContext> : SxDbRepository<string, SxEmployee, TDbContext> where TDbContext : SxDbContext
    {
        public override SxEmployee[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_EMPLOYEE AS de ");
            var joinString = @" JOIN AspNetUsers AS anu ON anu.Id = de.Id ";
            sb.Append(joinString);

            object param = null;
            var gws = getEmployeeWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "de.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_EMPLOYEE AS de ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxEmployee, SxAppUser, SxEmployee>(sb.ToString(), (e, u) => {
                    e.User = u;
                    return e;
                }, param: param, splitOn: "Id");
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getEmployeeWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();

            return query.ToString();
        }

        public override SxEmployee GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxEmployee, SxAppUser, SxEmployee>("dbo.get_employees @id", (e, u) =>
                {
                    e.User = u;
                    return e;
                }, new
                {
                    id = id[0]
                }).SingleOrDefault();

                return data;
            }
        }

        public override SxEmployee Create(SxEmployee model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.add_employee @uid", new
                {
                    uid = model.Id
                });
            }
            return null;
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.del_employee @uid", new
                {
                    uid = id[0]
                });
            }
        }
    }
}
