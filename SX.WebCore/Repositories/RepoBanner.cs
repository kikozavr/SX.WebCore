using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBanner<TDbContext> : SxDbRepository<Guid, SxBanner, TDbContext> where TDbContext : SxDbContext
    {
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
