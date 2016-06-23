using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoVideo<TDbContext> : SxDbRepository<Guid, SxVideo, TDbContext> where TDbContext : SxDbContext
    {
        public override SxVMVideo[] Query<SxVMVideo>(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM D_VIDEO dv ";

            object param = null;
            query += getVideoWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName="DateCreate", Direction=SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMVideo>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_VIDEO dv ";

            object param = null;
            query += getVideoWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getVideoWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dv.VideoId LIKE '%'+@vid+'%' OR @vid IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var vid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.VideoId != null ? (string)filter.WhereExpressionObject.VideoId : null;

            param = new
            {
                title = title,
                vid = vid
            };

            return query;
        }

        public virtual SxVMVideo[] LinkedVideos(SxFilter filter, bool forMaterial)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dv.*" });
            query += @" FROM D_VIDEO AS dv " + (forMaterial ? "" : @"LEFT") + @" JOIN D_VIDEO_LINK AS dvl ON dvl.VideoId = dv.Id ";

            object param = null;
            query += getLinkedVideoWhereString(filter, forMaterial, out param);

            var defaultOrder = new SxOrder { FieldName = "dv.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMVideo>(sql: query, param: param);
                return data.ToArray();
            }
        }

        public virtual int LinkedVideosCount(SxFilter filter, bool forMaterial)
        {
            var query = @"SELECT COUNT(1) FROM D_VIDEO AS dv " + (forMaterial ? "" : @"LEFT") + @" JOIN D_VIDEO_LINK AS dvl ON dvl.VideoId = dv.Id ";

            object param = null;
            query += getLinkedVideoWhereString(filter, forMaterial, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<int>(query, param: param).Single();
            }
        }

        private static void checkLinkedVideosFilter(SxFilter filter)
        {
            if (!filter.MaterialId.HasValue)
                throw new ArgumentNullException("Фильт должен сожержать идентификатор материала");
            if (filter.ModelCoreType == ModelCoreType.Unknown)
                throw new ArgumentNullException("Фильт должен сожержать тип материала");
        }

        private static string getLinkedVideoWhereString(SxFilter filter, bool forMaterial, out object param)
        {
            checkLinkedVideosFilter(filter);

            param = null;
            string query = null;
            query += " WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dv.VideoId LIKE '%'+@vid+'%' OR @vid IS NULL) ";
            if (forMaterial)
            {
                query += " AND (dvl.ModelCoreType = @mct) ";
                query += " AND (dvl.Materialid = @mid) ";
            }
            else
                query += " AND (dv.Id NOT IN (SELECT dvl2.VideoId FROM D_VIDEO_LINK AS dvl2 WHERE dvl2.MaterialId=@mid AND dvl2.ModelCoreType=@mct)) ";


            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var vid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.VideoId != null ? (string)filter.WhereExpressionObject.VideoId : null;

            param = new
            {
                title = title,
                vid = vid,
                mid = filter.MaterialId,
                mct = (byte)filter.ModelCoreType
            };

            return query;
        }

        public virtual void AddMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            var query = @"INSERT INTO D_VIDEO_LINK
VALUES
(
	@mid,
	@mct,
	@vid
)";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, vid = vid });
            }
        }

        public virtual void DeleteMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            var query = @"DELETE FROM D_VIDEO_LINK
WHERE MaterialId=@mid AND ModelCoreType=@mct AND VideoId=@vid";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, vid = vid });
            }
        }
    }
}
