using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTestBlock<TDbContext> : SxDbRepository<int, SxSiteTestBlock, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxSiteTestBlock> Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dstb.*", "dst.Id", "dst.Title", "dst.Title AS TestTitle" });
            query += @" FROM D_SITE_TEST_BLOCK AS dstb
JOIN D_SITE_TEST AS dst ON dst.Id = dstb.TestId ";

            object param = null;
            query += getSiteTestBlockWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("dstb.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestBlock, SxSiteTest, SxSiteTestBlock>(query, (b, t) =>
                {
                    b.Test = t;
                    return b;
                }, param: param, splitOn: "Id");
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SITE_TEST_BLOCK AS dstb JOIN D_SITE_TEST AS dst ON dst.Id = dstb.TestId ";

            object param = null;
            query += getSiteTestBlockWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestBlockWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dstb.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dstb.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";
            query += " AND (dst.Title LIKE '%'+@tTitle+'%' OR @tTitle IS NULL) ";
            query += " AND (dstb.TestId = @testId OR @testId=-1) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;
            var tTitle = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Test != null ? (string)filter.WhereExpressionObject.Test.Title : null;
            var testId = filter.WhereExpressionObject != null && filter.WhereExpressionObject.TestId != 0 ? (int)filter.WhereExpressionObject.TestId : -1;

            param = new
            {
                title = title,
                desc = desc,
                tTitle = string.IsNullOrEmpty(tTitle) ? null : tTitle,
                testId = testId
            };

            return query;
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_site_test_block @blockId", new { blockId = id[0] });
            }
        }
    }
}
