using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class RepoStatistic<TDbContext> : SxDbRepository<Guid, SxStatistic, TDbContext> where TDbContext : SxDbContext
    {
        public SxStatisticUserLogin[] UserLogins(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dsl.*", "ds.*", "anu.Id", "anu.NikName" });
            query += @" FROM D_STAT_LOGIN AS dsl
JOIN D_STATISTIC AS ds ON ds.Id = dsl.StatisticId
JOIN AspNetUsers AS anu ON anu.Id = dsl.UserId ";

            object param = null;
            query += getUserLoginsWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("ds.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxStatisticUserLogin, SxStatistic, SxAppUser, SxStatisticUserLogin>(query, (dsl, ds, anu)=> {
                    dsl.Statistic = ds;
                    dsl.User = anu;
                    return dsl;
                }, param: param, splitOn: "Id");
                return data.ToArray();
            }
        }

        public int UserLoginsCount(SxFilter filter)
        {
            var query = @"SELECT COUNT(*) FROM D_STAT_LOGIN AS dsl JOIN AspNetUsers AS anu ON anu.Id = dsl.UserId";

            object param = null;
            query += getUserLoginsWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getUserLoginsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (anu.NikName LIKE '%'+@un+'%' OR @un IS NULL) ";

            var un = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NikName != null ? (string)filter.WhereExpressionObject.NikName : null;

            param = new
            {
                un = un
            };

            return query;
        }

        /// <summary>
        /// Статистика входа пользователя
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        public void CreateStatisticUserLogin(DateTime date, string userId)
        {
            Task.Run(() =>
            {
                var stat = Create(new SxStatistic { DateCreate = date, Type = SxStatistic.SxStatisticType.UserLogin });
                var statUserLogin = new SxStatisticUserLogin
                {
                    StatisticId = stat.Id,
                    UserId = userId
                };

                var query = @"INSERT INTO D_STAT_LOGIN
VALUES
  (
    @sid,
    @uid
  )";
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute(query, new { sid = stat.Id, uid = userId });
                }
            });
        }
    }
}
