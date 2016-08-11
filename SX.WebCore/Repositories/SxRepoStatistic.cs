using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoStatistic<TDbContext> : SxDbRepository<Guid, SxStatistic, TDbContext> where TDbContext : SxDbContext
    {
        public  SxStatisticUserLogin[] UserLogins(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dsl.*",
                "ds.*",
                "anu.Id",
                "anu.NikName"
            }));
            sb.Append(" FROM D_STAT_LOGIN AS dsl ");
            var joinString = @" JOIN D_STATISTIC AS ds ON ds.Id = dsl.StatisticId
JOIN AspNetUsers AS anu ON anu.Id = dsl.UserId ";
            sb.Append(joinString);

            object param = null;
            var gws = getUserLoginsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "ds.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_STAT_LOGIN AS dsl ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxStatisticUserLogin, SxStatistic, SxAppUser, SxStatisticUserLogin>(sb.ToString(), (dsl, ds, anu) =>
                    {
                        dsl.Statistic = ds;
                        dsl.User = anu;
                        return dsl;
                    }, param: param, splitOn: "Id");
                    filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                    return data.ToArray();
                }
        }

        public async Task<SxStatisticUserLogin[]> UserLoginsAsync(SxFilter filter)
        {
            return await Task.Run(() =>
            {
                return UserLogins(filter);
            });
        }

        private static string getUserLoginsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (anu.NikName LIKE '%'+@un+'%' OR @un IS NULL) ");

            var un = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NikName != null ? (string)filter.WhereExpressionObject.NikName : null;

            param = new
            {
                un = un
            };

            return query.ToString();
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
