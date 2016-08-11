using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteTestQuestion<TDbContext> : SxDbRepository<int, SxSiteTestQuestion, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSiteTestQuestion[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_SITE_TEST_QUESTION AS dstq ");
            var joinString = @" JOIN D_SITE_TEST AS dst
            ON  dst.Id = dstq.TestId ";
            sb.Append(joinString);

            object param = null;
            var gws = getSiteTestQuestionWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dstq.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_SITE_TEST_QUESTION AS dstq ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTest, SxSiteTestQuestion>(sb.ToString(), (q, t) =>
                {
                    q.Test = t;
                    return q;
                }, param: param, splitOn: "Id");
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getSiteTestQuestionWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dstq.[Text] LIKE '%'+@text+'%' OR @text IS NULL) ");
            query.Append(" AND (dst.Id=@testId) ");

            var text = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Text != null ? (string)filter.WhereExpressionObject.Text : null;
            var testId = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? (int)filter.AddintionalInfo[0] : -1;

            param = new
            {
                text = text,
                testId= testId
            };

            return query.ToString();
        }

        public override void Delete(SxSiteTestQuestion model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.del_site_test_question @questionId", new { questionId = model.Id });
            }
        }

        public override SxSiteTestQuestion Create(SxSiteTestQuestion model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion>("dbo.add_site_test_question @testId, @text", new
                {
                    testId = model.TestId,
                    text = model.Text
                }).SingleOrDefault();
                return data;
            }
        }
    }
}
