using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteTestQuestion<TDbContext> : SxDbRepository<int, SxSiteTestQuestion, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSiteTestQuestion[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM   D_SITE_TEST_QUESTION  AS dstq
       JOIN D_SITE_TEST      AS dst
            ON  dst.Id = dstq.TestId ";

            object param = null;
            query += getSiteTestQuestionWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dstq.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTest, SxSiteTestQuestion>(query, (q, t) =>
                {
                    q.Test = t;
                    return q;
                }, param: param, splitOn: "Id");
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM   D_SITE_TEST_QUESTION  AS dstq
       JOIN D_SITE_TEST      AS dst
            ON  dst.Id = dstq.TestId ";

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
            query += " AND (dst.Id=@testId) ";

            var text = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Text != null ? (string)filter.WhereExpressionObject.Text : null;
            var testId = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? (int)filter.AddintionalInfo[0] : -1;

            param = new
            {
                text = text,
                testId= testId
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

        public override SxSiteTestQuestion Create(SxSiteTestQuestion model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion>("add_site_test_question @testId, @text", new
                {
                    testId = model.TestId,
                    text = model.Text
                }).SingleOrDefault();
                return data;
            }
        }
    }
}
