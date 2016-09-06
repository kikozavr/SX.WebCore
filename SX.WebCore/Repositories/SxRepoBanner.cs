using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoBanner<TDbContext> : SxDbRepository<Guid, SxBanner, TDbContext, SxVMBanner> where TDbContext : SxDbContext
    {
        public override SxVMBanner[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "*",
                "CASE WHEN db.ShowsCount=0 THEN 0 ELSE ROUND(db.ClicksCount*100/db.ShowsCount, 2) END AS CTR"
            }));
            sb.Append(" FROM D_BANNER AS db ");

            object param = null;
            var gws = getBannersWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "db.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_BANNER AS db ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return connection.Query<SxVMBanner>(sb.ToString(), param: param).ToArray();
            }
        }
        private static string getBannersWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (db.Url LIKE '%'+@url+'%' OR @url IS NULL) ");
            query.Append(" AND (db.Title LIKE '%'+@title+'%' OR @title IS NULL) ");

            bool forGroup = filter.AddintionalInfo != null && filter.AddintionalInfo[1] != null ? (bool)filter.AddintionalInfo[1] : false;
            if (forGroup == true)
                query.Append(" AND (db.Id IN (SELECT dbgl.BannerId FROM D_BANNER_GROUP_LINK dbgl WHERE dbgl.BannerGroupId=@bgid)) ");
            else if (forGroup == false)
                query.Append(" AND (db.Id NOT IN (SELECT dbgl.BannerId FROM D_BANNER_GROUP_LINK dbgl WHERE dbgl.BannerGroupId=@bgid)) ");

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;

            param = new
            {
                title = title,
                url = url,
                bgid = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? filter.AddintionalInfo[0] : null,
                mid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.MaterialId != null ? (int)filter.WhereExpressionObject.MaterialId : (int?)null,
                mct = filter.WhereExpressionObject != null && filter.WhereExpressionObject.ModelCoreType != null ? (int)filter.WhereExpressionObject.ModelCoreType : (int?)null,
            };

            return query.ToString();
        }

        public void GetStatistic(out int showsCount, out int clicksCount)
        {
            var queryShows = "SELECT ISNULL(SUM(db.ShowsCount),0) FROM D_BANNER AS db";
            var queryClicks = "SELECT ISNULL(SUM(db.ClicksCount),0) FROM D_BANNER AS db";

            using (var conn = new SqlConnection(ConnectionString))
            {
                showsCount = conn.Query<int>(queryShows).SingleOrDefault();
                clicksCount = conn.Query<int>(queryClicks).SingleOrDefault();
            }
        }

        public override SxVMBanner[] All
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxVMBanner, SxVMPicture, SxVMBanner>("dbo.get_banners @id, @place", (b, p) =>
                    {
                        b.PictureId = p.Id;
                        b.Picture = p;
                        return b;
                    }, new
                    {
                        id = (Guid?)null,
                        place = (SxBanner.BannerPlace?)null
                    }, splitOn: "Id");

                    return data.ToArray();
                }
            }
        }

        public override SxBanner GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBanner, SxPicture, SxBanner>("dbo.get_banners @id, @place", (b, p) =>
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
                conn.Execute("dbo.update_banner @id, @url, @pid, @title, @place, @rawUrl, @desc", new
                {
                    id = model.Id,
                    url = model.Url,
                    pid = model.PictureId,
                    title = model.Title,
                    place = model.Place,
                    rawUrl = model.RawUrl,
                    desc = model.Description
                });
            }
            return GetByKey(model.Id);
        }

        public override void Delete(SxBanner model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_banner @bannerId", new { bannerId=model.Id });
            }
        }

        public Task AddClickAsync(Guid bannerId, List<string> affiliateLinkIds=null)
        { 
            return Task.Run(() =>
            {
                string alIds = null;
                if (affiliateLinkIds != null && affiliateLinkIds.Any())
                {
                    var sb = new StringBuilder();
                    affiliateLinkIds.ForEach(x=> {
                        sb.AppendFormat(",'{0}'", x);
                    });
                    sb.Remove(0, 1);
                    alIds = sb.ToString();
                }

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("dbo.add_banner_clicks_count @bId, @alIds", new { bId = bannerId, alIds = alIds });
                }
            });
        }

        public Task AddShowsAsync(Guid[] bannersId)
        {
            return Task.Run(() =>
            {
                if (!bannersId.Any()) return;
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("dbo.add_banners_shows_count @keys", new { keys = getBannerGuids(bannersId) });
                }
            });
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

        public SxBannersStatistic[] DateStatistic
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxBannersStatistic>("dbo.get_banners_statistic");
                    return data.ToArray();
                }
            }
        }
    }
}
