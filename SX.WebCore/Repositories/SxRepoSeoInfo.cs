using Dapper;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.Enums;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSeoInfo<TDbContext> : SxDbRepository<int, SxSeoInfo, TDbContext> where TDbContext: SxDbContext
    {
        public override IQueryable<SxSeoInfo> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var seoInfo = conn.Query<SxSeoInfo>(@"get_all_seo_info");
                    return seoInfo.AsQueryable();
                }
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SEO_INFO";
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                return conn.Query<int>(query).Single();
            }
        }

        /// <summary>
        /// Теги для url
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public SxSeoInfo GetSeoInfo(string url)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<SxSeoInfo>("get_page_seo_info @url", new { url = url }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>("get_page_seo_info_keywords @seoInfoId", new { seoInfoId = data.Id }).ToArray();
                }

                return data ?? new SxSeoInfo();
            }
        }

        /// <summary>
        /// Теги seo для материала
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        /// <returns></returns>
        public SxSeoInfo GetSeoInfo(int mid, ModelCoreType mct)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<SxSeoInfo>("get_material_seo_info @mid, @mct", new { mid = mid, mct = mct }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>("get_page_seo_info_keywords @seoInfoId", new { seoInfoId = data.Id }).ToArray();
                }

                return data ?? new SxSeoInfo();
            }
        }
    }
}
