using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteTestSubject<TDbContext> : SxDbRepository<int, SxSiteTestSubject, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSiteTestSubject[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] {
                "dstq.Id",
                "dstq.Title",
                "dstq.Description",
                "dstq.TestId",
                "dstq.PictureId",
                "dst.Id",
                "dp.Id"
            });
            query += @" FROM   D_SITE_TEST_SUBJECT  AS dstq
       JOIN D_SITE_TEST      AS dst
            ON  dst.Id = dstq.TestId
       LEFT JOIN D_PICTURE AS dp on dp.Id=dstq.PictureId ";

            object param = null;
            query += getSiteTestSubjectsWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dstq.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestSubject, SxSiteTest, SxPicture, SxSiteTestSubject>(query, (q, t, p) =>
                {
                    q.Picture = p;
                    q.Test = t;
                    return q;
                }, param: param, splitOn: "Id");
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM   D_SITE_TEST_SUBJECT  AS dstq
       JOIN D_SITE_TEST      AS dst
            ON  dst.Id = dstq.TestId
       LEFT JOIN D_PICTURE AS dp on dp.Id=dstq.PictureId ";

            object param = null;
            query += getSiteTestSubjectsWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestSubjectsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dstq.[Title] LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dstq.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ";
            query += " AND (dst.Id=@testId) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;
            var testId = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? (int)filter.AddintionalInfo[0] : -1;

            param = new
            {
                title = title,
                desc=desc,
                testId= testId
            };

            return query;
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_site_test_subject @subjectId", new { subjectId = id[0] });
            }
        }

        public override SxSiteTestSubject Create(SxSiteTestSubject model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestSubject>("add_site_test_subject @testId, @title, @desc, @pictureId", new
                {
                    testId = model.TestId,
                    title = model.Title,
                    desc=model.Description,
                    pictureId=model.PictureId
                }).SingleOrDefault();
                return data;
            }
        }
    }
}
