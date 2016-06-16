using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTest<TDbContext> : SxDbRepository<int, SxSiteTest, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxSiteTest> All
        {
            get
            {
                var query = @"SELECT * FROM D_SITE_TEST AS dst";
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxSiteTest>(query);
                    return data.AsQueryable();
                }
            }
        }

        public override IQueryable<SxSiteTest> Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += " FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("dst.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTest>(query, param: param);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dst.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dst.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc=desc
            };

            return query;
        }

        public dynamic[] RandomList(int amount=3)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {

                var data = conn.Query("get_random_site_tests @amount", new { amount= amount });
                return data.ToArray();
            }
        }

        public SxSiteTestQuestion[] GetSiteTestPage(string titleUrl)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTestBlock, SxSiteTest, SxSiteTestQuestion>("get_site_test_page @titleUrl", (q, b, t)=> {
                    b.Test = t;
                    q.Block = b;
                    return q;
                }, new { titleUrl = titleUrl }, splitOn:"Id");
                return data.ToArray();
            }
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_site_test @testId", new { testId = id[0] });
            }
        }

        public SxSiteTestQuestion[] GetMatrix(int testId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTestBlock, SxSiteTestQuestion>("get_site_test_matrix @testId", (q,b)=> {
                    q.Block = b;
                    return q;
                }, new { testId = testId });
                return data.ToArray();
            }
        }
    }
}
