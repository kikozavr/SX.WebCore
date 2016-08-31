using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSeoTags<TDbContext> : SxDbRepository<int, SxSeoTags, TDbContext, SxVMSeoTags> where TDbContext : SxDbContext
    {
        private static SxRepoSeoKeyword<TDbContext> _repoSeoKeywords;
        public SxRepoSeoTags()
        {
            if (_repoSeoKeywords == null)
                _repoSeoKeywords = new SxRepoSeoKeyword<TDbContext>();
        }

        public override SxVMSeoTags[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_SEO_TAGS AS dsi ");

            object param = null;
            var gws = getSeoTagsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_SEO_TAGS AS dsi ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMSeoTags>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getSeoTagsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dsi.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL) ");
            query.Append(" AND (dsi.SeoTitle LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dsi.SeoDescription LIKE '%'+@desc+'%' OR @desc IS NULL) ");
            query.Append(" AND (dsi.H1 LIKE '%'+@h1+'%' OR @h1 IS NULL) ");
            query.Append(" AND (dsi.MAterialId IS NULL) ");

            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;
            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.SeoTitle != null ? (string)filter.WhereExpressionObject.SeoTitle : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.SeoDescription != null ? (string)filter.WhereExpressionObject.SeoDescription : null;
            var h1 = filter.WhereExpressionObject != null && filter.WhereExpressionObject.H1 != null ? (string)filter.WhereExpressionObject.H1 : null;

            param = new
            {
                raw_url = rawUrl,
                title = title,
                desc = desc,
                h1 = h1
            };

            return query.ToString();
        }


        /// <summary>
        /// Удалить seo-теги материала
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        public void DeleteMaterialSeoInfo(int mid, ModelCoreType mct)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_material_seo_tags @mid, @mct", new { mid = mid, mct = mct });
            }
        }
        public async Task DeleteMaterialSeoInfoAsync(int mid, ModelCoreType mct)
        {
            await Task.Run(()=> {
                DeleteMaterialSeoInfo(mid, mct);
            });
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
                if (data != null)
                    data.Keywords = connection.Query<SxSeoKeyword>("dbo.get_page_seo_info_keywords @seoTagsId", new { seoTagsId = data.Id }).ToArray();
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
                var data = conn.Query<SxSeoTags>("dbo.get_page_seo_info @url", new { url = url }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>("dbo.get_page_seo_info_keywords @seoTagsId", new { seoTagsId = data.Id }).ToArray();
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
                var data = conn.Query<SxSeoTags>("dbo.get_material_seo_info @mid, @mct", new { mid = mid, mct = mct }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>("dbo.get_page_seo_info_keywords @seoInfoId", new { seoInfoId = data.Id }).ToArray();
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
                var data= conn.Query<SxSeoTags>(query, new { @RAW_URL = rawUrl }).SingleOrDefault();
                return data;
            }
        }

        public void UpdateMaterialSeoTags(int mid, ModelCoreType mct, int? stid)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.update_material_seo_tags @mid, @mct, @stid", new
                {
                    mid = mid,
                    mct = mct,
                    stid = stid
                });
            }
        }

        public async Task UpdateMaterialSeoTagsAsync(int mid, ModelCoreType mct, int? stid)
        {
            await Task.Run(()=> {
                UpdateMaterialSeoTags(mid, mct, stid);
            });
        }

        public override void Delete(SxSeoTags model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_seo_tags @seoTagsId", new { seoTagsId = model.Id });
            }
        }
    }
}
