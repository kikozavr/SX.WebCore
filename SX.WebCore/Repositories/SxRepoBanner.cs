using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoBanner<TDbContext> : SxDbRepository<Guid, SxBanner, TDbContext> where TDbContext : SxDbContext
    {
        public SxBanner[] Query(SxFilter filter, bool? forGroup = null, bool? forMaterial = null)
        {
            var query = SxQueryProvider.GetSelectString();
            query += " FROM D_BANNER AS db ";

            object param = null;
            query += getBannerWhereString(filter, out param, forGroup, forMaterial);

            var defaultOrder = new SxOrder { FieldName = "db.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBanner>(query, param: param);
                return data.ToArray();
            }
        }

        public int Count(SxFilter filter, bool? forGroup = null, bool? forMaterial = null)
        {
            var query = @"SELECT COUNT(1) FROM D_BANNER AS db ";

            object param = null;
            query += getBannerWhereString(filter, out param, forGroup, forMaterial);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getBannerWhereString(SxFilter filter, out object param, bool? forGroup = null, bool? forMaterial = null)
        {
            param = null;
            string query = null;
            query += " WHERE (db.Url LIKE '%'+@url+'%' OR @url IS NULL) ";
            query += " AND (db.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            if (forGroup.HasValue && filter.WhereExpressionObject != null && filter.WhereExpressionObject.BannerGroupId != null)
            {
                //for group banners
                if (forGroup == true)
                    query += " AND (db.Id IN (SELECT dbgl.BannerId FROM D_BANNER_GROUP_LINK dbgl WHERE dbgl.BannerGroupId=@bgid)) ";
                else if (forGroup == false)
                    query += " AND (db.Id NOT IN (SELECT dbgl.BannerId FROM D_BANNER_GROUP_LINK dbgl WHERE dbgl.BannerGroupId=@bgid)) ";
            }
            if (forMaterial.HasValue && filter.WhereExpressionObject != null && (filter.WhereExpressionObject.MaterialId != null && filter.WhereExpressionObject.ModelCoreType != null))
            {
                //for material banners
                if (forMaterial == true)
                    query += " AND (db.Id IN (SELECT dmb.BannerId FROM D_MATERIAL_BANNER AS dmb WHERE dmb.MaterialId=@mid AND dmb.ModelCoreType=@mct)) ";
                else if (forMaterial == false)
                    query += " AND (db.Id NOT IN (SELECT dmb.BannerId FROM D_MATERIAL_BANNER AS dmb WHERE dmb.MaterialId=@mid AND dmb.ModelCoreType=@mct)) ";
            }

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;

            param = new
            {
                title = title,
                url = url,
                bgid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.BannerGroupId != null ? (Guid)filter.WhereExpressionObject.BannerGroupId : (Guid?)null,
                mid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.MaterialId != null ? (int)filter.WhereExpressionObject.MaterialId : (int?)null,
                mct = filter.WhereExpressionObject != null && filter.WhereExpressionObject.ModelCoreType != null ? (int)filter.WhereExpressionObject.ModelCoreType : (int?)null,
            };

            return query;
        }
        public override IQueryable<SxBanner> All
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxBanner, SxPicture, SxBanner>("get_banners @id, @place", (b, p) =>
                    {
                        b.PictureId = p.Id;
                        b.Picture = p;
                        return b;
                    }, new
                    {
                        id = (Guid?)null,
                        place = (SxBanner.BannerPlace?)null
                    }, splitOn: "Id");

                    return data.AsQueryable();
                }
            }
        }

        public override SxBanner GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBanner, SxPicture, SxBanner>("get_banners @id, @place", (b, p) =>
                {
                    b.PictureId = p.Id;
                    b.Picture = p;
                    return b;
                }, new
                {
                    id = id[0],
                    place = (SxBanner.BannerPlace?)null
                }, splitOn: "Id").SingleOrDefault();

                return data;
            }
        }

        public override SxBanner Update(SxBanner model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("update_banner @id, @url, @pid, @title, @place, @controller, @action", new
                {
                    id = model.Id,
                    url = model.Url,
                    pid = model.PictureId,
                    title = model.Title,
                    place = model.Place,
                    controller = model.ControllerName,
                    action = model.ActionName
                });
            }
            return GetByKey(model.Id);
        }

        public void AddClick(Guid bannerId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_banner_clicks_count @id", new { id = bannerId });
            }
        }

        public void AddShows(Guid[] bannersId)
        {
            if (!bannersId.Any()) return;

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_banners_shows_count @keys", new { keys = getBannerGuids(bannersId) });
            }
        }
        private static string getBannerGuids(Guid[] bannersId)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bannersId.Length; i++)
            {
                sb.AppendFormat(",'{0}'", bannersId[i]);
            }
            sb.Remove(0, 1);

            return sb.ToString();
        }
    }
}
