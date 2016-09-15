using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoAnalizatorSession : SxDbRepository<int, SxAnalizatorSession, SxVMAnalizatorSession>
    {
        public new SxAnalizatorSession Create(SxAnalizatorSession model)
        {
            SxAnalizatorSession newSession = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                newSession = connection.Query<SxAnalizatorSession>("dbo.add_analizator_session @userId", new { userId = model.UserId }).SingleOrDefault();
                if (newSession == null) return null;

                foreach (var item in model.Urls)
                {
                    connection.Execute("dbo.add_analizator_session_url @url, @sessionId", new { url=item.Url, sessionId=newSession.Id});
                }
            }

            return newSession;
        }
        public override async Task<SxAnalizatorSession> CreateAsync(SxAnalizatorSession model)
        {
            return await Task.Run(() => { return Create(model); });
        }

        public override SxVMAnalizatorSession[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "das.*",
                "(SELECT COUNT(1) FROM D_ANALIZATOR_URL AS dau WHERE dau.AnalizatorSessionId=das.Id) AS UrlsCount"
            }));
            sb.Append(" FROM D_ANALIZATOR_SESSION AS das ");

            object param = null;
            var gws = getAnalizerSessionsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new Dictionary<string, string>
            {
                ["DateCreate"] = "das.DateCreate"
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_ANALIZATOR_SESSION AS das ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMAnalizatorSession>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getAnalizerSessionsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (das.[UserId] = @userId) ");

            param = new
            {
                userId = filter.UserId
            };

            return query.ToString();
        }
    }
}
