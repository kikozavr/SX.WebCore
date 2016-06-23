using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoEmployee<TDbContext> : SxDbRepository<string, SxEmployee, TDbContext> where TDbContext : SxDbContext
    {
        public override SxEmployee[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM D_EMPLOYEE AS de
JOIN AspNetUsers AS anu ON anu.Id = de.Id ";

            object param = null;
            query += getEmployeeWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "de.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxEmployee, SxAppUser, SxEmployee>(query, (e, u) => {
                    e.User = u;
                    return e;
                }, param: param, splitOn: "Id");
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_EMPLOYEE AS de";

            object param = null;
            query += getEmployeeWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getEmployeeWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            //query += " WHERE (dg.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            //query += " AND (dg.TitleAbbr LIKE '%'+@title_abbr+'%' OR @title_abbr IS NULL) ";
            //query += " AND (dg.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            //var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            //var title_abbr = filter.WhereExpressionObject != null && filter.WhereExpressionObject.TitleAbbr != null ? (string)filter.WhereExpressionObject.TitleAbbr : null;
            //var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            //param = new
            //{
            //    title = title,
            //    title_abbr = title_abbr,
            //    desc = desc
            //};

            return query;
        }

        public override SxEmployee GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxEmployee, SxAppUser, SxEmployee>("get_employees @id", (e, u) =>
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
                conn.Execute("add_employee @uid", new
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
                conn.Execute("del_employee @uid", new
                {
                    uid = id[0]
                });
            }
        }
    }
}
