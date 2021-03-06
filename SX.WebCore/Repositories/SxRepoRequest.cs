﻿using Dapper;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoRequest : SxDbRepository<Guid, SxRequest, SxVMRequest>
    {
        public override SxRequest Create(SxRequest model)
        {
            var id = Guid.NewGuid();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data=conn.Query<SxRequest>("dbo.add_request @id, @browser, @clientIP, @rawUrl, @requestType, @urlRef, @sessionId, @userAgent", new
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

                return data.SingleOrDefault();
            }
        }

        public override SxVMRequest[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dr.Id",
                "dr.SessionId",
                "dr.UrlRef",
                "dr.Browser",
                "dr.ClientIP",
                "dr.UserAgent",
                "dr.RequestType",
                "dr.DateCreate",
                "dr.RawUrl"
            }));
            sb.Append(" FROM D_REQUEST AS dr ");

            object param = null;
            var gws = getRequestWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_REQUEST AS dr ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMRequest>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getRequestWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dr.SessionId LIKE '%'+@sid+'%' OR @sid IS NULL)");
            query.Append(" AND (dr.UrlRef LIKE '%'+@url_ref+'%' OR @url_ref IS NULL)");
            query.Append(" AND (dr.Browser LIKE '%'+@browser+'%' OR @browser IS NULL)");
            query.Append(" AND (dr.ClientIP LIKE '%'+@cip+'%' OR @cip IS NULL)");
            query.Append(" AND (dr.UserAgent LIKE '%'+@ua+'%' OR @ua IS NULL)");
            query.Append(" AND (dr.RequestType LIKE '%'+@rt+'%' OR @rt IS NULL)");
            query.Append(" AND (dr.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL)");

            string sid = filter.WhereExpressionObject?.SessionId;
            string urlRef = filter.WhereExpressionObject?.UrlRef;
            string browser = filter.WhereExpressionObject?.Browser;
            string cip = filter.WhereExpressionObject?.ClientIP;
            string ua = filter.WhereExpressionObject?.UserAgent;
            string rt = filter.WhereExpressionObject?.RequestType;
            string rawUrl = filter.WhereExpressionObject?.RawUrl;

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

            return query.ToString();
        }

        public async Task<SxDateStatistic[]> DateStatisticAsync()
        {
            return await Task.Run(() =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    return conn.Query<SxDateStatistic>("dbo.get_request_date_statistic").ToArray();
                }
            });
        }
    }
}
