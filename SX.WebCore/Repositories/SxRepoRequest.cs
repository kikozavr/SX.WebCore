using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;

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
            return base.Create(model);
        }
    }
}
