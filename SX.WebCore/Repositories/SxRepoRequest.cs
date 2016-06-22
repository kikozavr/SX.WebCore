using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoRequest<TDbContext> : SxDbRepository<Guid, SxRequest, TDbContext> where TDbContext : SxDbContext
    {
        public override SxRequest Create(SxRequest model)
        {
            var id = Guid.NewGuid();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_request @id, @browser, @clientIP, @rawUrl, @requestType, @urlRef, @sessionId, @userAgent", new
                {
                    id = id,
                    browser = model.Browser,
                    clientIP = model.ClientIP,
                    rawUrl = model.RawUrl,
                    requestType = model.RequestType,
                    urlRef = model.UrlRef,
                    sessionId = model.SessionId,
                    userAgent = model.UserAgent
                });
            }

            return base.GetByKey(id);
        }

        public override SxRequest[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] {
                    "dr.Id", "dr.SessionId", "dr.UrlRef", "dr.Browser", "dr.ClientIP", "dr.UserAgent", "dr.RequestType", "dr.DateCreate", "dr.RawUrl"
                });
            query += @" FROM D_REQUEST dr ";

            object param = null;
            query += getRequestWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxRequest>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_REQUEST AS dr";

            object param = null;
            query += getRequestWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getRequestWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dr.SessionId LIKE '%'+@sid+'%' OR @sid IS NULL)";
            query += " AND (dr.UrlRef LIKE '%'+@url_ref+'%' OR @url_ref IS NULL)";
            query += " AND (dr.Browser LIKE '%'+@browser+'%' OR @browser IS NULL)";
            query += " AND (dr.ClientIP LIKE '%'+@cip+'%' OR @cip IS NULL)";
            query += " AND (dr.UserAgent LIKE '%'+@ua+'%' OR @ua IS NULL)";
            query += " AND (dr.RequestType LIKE '%'+@rt+'%' OR @rt IS NULL)";
            query += " AND (dr.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL)";

            var sid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.SessionId != null ? (string)filter.WhereExpressionObject.SessionId : null;
            var urlRef = filter.WhereExpressionObject != null && filter.WhereExpressionObject.UrlRef != null ? (string)filter.WhereExpressionObject.UrlRef : null;
            var browser = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Browser != null ? (string)filter.WhereExpressionObject.Browser : null;
            var cip = filter.WhereExpressionObject != null && filter.WhereExpressionObject.ClientIP != null ? (string)filter.WhereExpressionObject.ClientIP : null;
            var ua = filter.WhereExpressionObject != null && filter.WhereExpressionObject.UserAgent != null ? (string)filter.WhereExpressionObject.UserAgent : null;
            var rt = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RequestType != null ? (string)filter.WhereExpressionObject.RequestType : null;
            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;

            param = new
            {
                sid = sid,
                url_ref = urlRef,
                browser = browser,
                cip = cip,
                ua = ua,
                rt = rt,
                raw_url = rawUrl
            };

            return query;
        }
    }
}
