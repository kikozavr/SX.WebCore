using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTestQuestion<TDbContext> : SxDbRepository<int, SxSiteTestQuestion, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxSiteTestQuestion> Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dstq.*", "dstb.Id", "dstb.Title", "dstb.Title AS BlockTitle", "dst.Id", "dst.Title", "dst.Title AS TestTitle" });
            query += @" FROM   D_SITE_TEST_QUESTION    AS dstq
       JOIN D_SITE_TEST_BLOCK  AS dstb
            ON  dstb.Id = dstq.BlockId
       JOIN D_SITE_TEST        AS dst
            ON  dst.Id = dstb.TestId ";

            object param = null;
            query += getSiteTestQuestionWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("dstq.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTestBlock, SxSiteTest, SxSiteTestQuestion>(query, (q, b, t) =>
                {
                    b.Test = t;
                    q.Block = b;
                    return q;
                }, param: param, splitOn: "Id");
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM   D_SITE_TEST_QUESTION    AS dstq
       JOIN D_SITE_TEST_BLOCK  AS dstb
            ON  dstb.Id = dstq.BlockId
       JOIN D_SITE_TEST        AS dst
            ON  dst.Id = dstb.TestId ";

            object param = null;
            query += getSiteTestQuestionWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestQuestionWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dstq.[Text] LIKE '%'+@text+'%' OR @text IS NULL) ";
            query += " AND (dstb.[Title] LIKE '%'+@bTitle+'%' OR @bTitle IS NULL) ";
            query += " AND (dst.[Title] LIKE '%'+@tTitle+'%' OR @tTitle IS NULL) ";

            var text = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Text != null ? (string)filter.WhereExpressionObject.Text : null;
            var bTitle = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Block != null ? (string)filter.WhereExpressionObject.Block.Title : null;
            var tTitle = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Block != null ? (string)filter.WhereExpressionObject.Block.TestTitle : null;

            param = new
            {
                text = text,
                bTitle = string.IsNullOrEmpty(bTitle) ? null : bTitle,
                tTitle = string.IsNullOrEmpty(tTitle) ? null : tTitle,
            };

            return query;
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_site_test_question @questionId", new { questionId = id[0] });
            }
        }
    }
}
