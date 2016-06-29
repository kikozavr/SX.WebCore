using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSeoTags<TDbContext> : SxDbRepository<int, SxSeoTags, TDbContext> where TDbContext: SxDbContext
    {
        public override SxSeoTags[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dsi.Id", "dsi.RawUrl" });
            query += " FROM D_SEO_TAGS AS dsi ";

            object param = null;
            query += getSeoInfoWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSeoTags>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SEO_TAGS as dsi";

            object param = null;
            query += getSeoInfoWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSeoInfoWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dsi.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL)";
            query += " AND (dsi.RawUrl IS NOT NULL) ";

            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;

            param = new
            {
                raw_url = rawUrl
            };

            return query;
        }


        /// <summary>
        /// Удалить seo-теги материала
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        public void DeleteMaterialSeoInfo(int mid, ModelCoreType mct)
        {
            var query = "UPDATE DV_MATERIAL SET SeoTagsId=NULL WHERE Id=@mid AND ModelCoreType=@mct;";
            query += "DELETE FROM D_SEO_TAGS WHERE Id IN (SELECT dsi.Id FROM D_SEO_TAGS AS dsi WHERE MaterialId=@mid AND ModelCoreType=@mct)";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { mid = mid, mct = mct });
            }
        }

        /// <summary>
        /// Seo-теги материала
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        /// <returns></returns>
        public SxSeoTags GetMaterialSeoInfo(int mid, ModelCoreType mct)
        {
            var query = "SELECT * FROM D_SEO_TAGS AS dsi WHERE MaterialId=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxSeoTags>(query, new { mid = mid, mct = mct }).SingleOrDefault();
                return data;
            }
        }

        /// <summary>
        /// Теги для url
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public SxSeoTags GetSeoTags(string url)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSeoTags>("get_page_seo_info @url", new { url = url }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>("get_page_seo_info_keywords @seoTagsId", new { seoTagsId = data.Id }).ToArray();
                }

                return data ?? new SxSeoTags();
            }
        }

        /// <summary>
        /// Теги seo для материала
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        /// <returns></returns>
        public SxSeoTags GetSeoTags(int mid, ModelCoreType mct)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSeoTags>("get_material_seo_info @mid, @mct", new { mid = mid, mct = mct }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>("get_page_seo_info_keywords @seoInfoId", new { seoInfoId = data.Id }).ToArray();
                }

                return data ?? new SxSeoTags();
            }
        }

        /// <summary>
        /// Найти страницу по RawUrl
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public SxSeoTags GetByRawUrl(string rawUrl)
        {
            var query = @"SELECT dsi.Id FROM D_SEO_TAGS AS dsi WHERE dsi.RawUrl=@RAW_URL";
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<SxSeoTags>(query, new { @RAW_URL = rawUrl }).SingleOrDefault();
            }
        }

        public void UpdateMaterialSeoInfo(int mid, ModelCoreType mct, int? stid)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.update_material_seo_tags @mid, @mct, @stid", new {
                    mid=mid,
                    mct=mct,
                    stid= stid
                });
            }
        }
    }
}
