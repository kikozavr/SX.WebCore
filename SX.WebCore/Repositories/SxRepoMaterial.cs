using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoMaterial<TModel, TViewModel, TDbContext> : SxDbRepository<int, TModel, TDbContext>
        where TModel : SxMaterial
        where TViewModel : SxVMMaterial
        where TDbContext : SxDbContext
    {
        private static ModelCoreType _mct;
        protected static ModelCoreType ModelCoreType
        {
            get
            {
                return _mct;
            }
        }

        public SxRepoMaterial(ModelCoreType mct)
        {
            _mct = mct;
        }

        public SxDateStatistic[] DateStatistic
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxDateStatistic>("dbo.get_material_date_statistic @mct", new { mct = _mct });
                    return data.ToArray();
                }
            }
        }

        public override TModel[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dm.*",
                "dmc.*",
                "anu.*",
                "dp.Id", "dp.Width", "dp.Height"
            }));
            sb.Append(" FROM DV_MATERIAL AS dm ");
            sb.Append(" LEFT JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId ");
            sb.Append(" LEFT JOIN AspNetUsers AS anu ON anu.Id = dm.UserId ");
            sb.Append(" LEFT JOIN D_PICTURE AS dp ON dp.Id = dm.FrontPictureId ");

            object param = null;
            var gws = getMaterialsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM DV_MATERIAL AS dm ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<TModel, SxMaterialCategory, SxAppUser, SxPicture, TModel>(sb.ToString(), (m, c, u, p)=> {
                    m.Category = c;
                    m.User = u;
                    m.FrontPicture = p;
                    return m;
                }, param: param, splitOn:"Id");
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getMaterialsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dm.Title LIKE '%'+@title+'%' OR @title IS NULL) ");

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;

            param = new
            {
                title = title
            };

            return query.ToString();
        }

        public virtual TViewModel GetByTitleUrl(int year, string month, string day, string titleUrl, int materialTagsCount=20)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<TViewModel, SxVMMaterialCategory, SxVMAppUser, SxVMPicture, SxVMSeoTags, TViewModel>(
                    "dbo.get_material_by_url @year, @month, @day, @title_url, @mct",
                    (m, c, u, p, st)=> {
                        m.Category = c;
                        m.User = u;
                        m.FrontPicture = p;
                        m.SeoTags = st;
                        return m;
                    },
                    new {
                        year = year,
                        month = month,
                        day = day,
                        title_url = titleUrl,
                        mct = _mct
                    },
                    splitOn:"Id"
                    )
                    .SingleOrDefault();

                if (data != null)
                {
                    //kewords
                    if (data.SeoTags != null)
                        data.SeoTags.Keywords = connection.Query<SxVMSeoKeyword>("dbo.get_page_seo_info_keywords @seoTagsId", new { seoTagsId = (int)data.SeoTagsId }).ToArray();

                    //videos
                    data.Videos = connection.Query<SxVideo>("dbo.get_material_videos @mid, @mct", new { mid = data.Id, mct = data.ModelCoreType }).ToArray();

                    //cloud
                    data.MaterialTags = connection.Query<SxVMMaterialTag>("dbo.get_material_cloud @amount, @mid, @mct", new { amount = materialTagsCount, mid = data.Id, mct = data.ModelCoreType }).ToArray();
                }
                return data;
            }
        }

        public override TModel GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<TModel, SxMaterialCategory, SxAppUser, SxPicture, TModel>("dbo.get_material_by_id @id, @mct",
                    (m, c, u, p)=> {
                        m.Category = c;
                        m.User = u;
                        m.FrontPicture = p;
                        return m;
                    },
                    new { id = id[0], mct = _mct },
                    splitOn:"Id");
                return data.SingleOrDefault();
            }
        }

        public bool ExistsMaterialByTitleUrl(string titleUrl)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<bool>("dbo.exists_material_by_title_url @titleUrl", new { titleUrl = titleUrl }).SingleOrDefault();
                return data;
            }
        }

        public void AddUserView(int mid, ModelCoreType mct)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.add_material_view @mid, @mct", new { mid = mid, mct = mct });
            }
        }

        public override void Delete(TModel model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_material @mid, @mct", new { mid = model.Id, mct = model.ModelCoreType });
            }
        }

        public async Task<int> AddLikeAsync(bool ld, int mid, ModelCoreType mct)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<int>("dbo.add_material_like @ld, @mid, @mct", new { ld = ld, mid = mid, mct = mct });
                    return data.SingleOrDefault();
                }
            });

        }

        public virtual TViewModel[] GetLikeMaterials(SxFilter filter, int amount=10)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<TViewModel, SxVMAppUser, TViewModel>("dbo.get_like_materials @amount, @mid, @mct", (m, u) =>
                {
                    m.User = u;
                    return m;
                }, new { mid = filter.MaterialId, mct = filter.ModelCoreType, amount = amount }, splitOn: "Id");
                return data.ToArray();
            }
        }

        public virtual TViewModel[] GetByDateMaterials(int mid, ModelCoreType mct, bool dir = false, int amount = 3)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<TViewModel, SxVMAppUser, TViewModel>("dbo.get_other_materials @mid, @mct, @dir, @amount", (m, u) => {
                    m.User = u;
                    return m;
                }, new { mid = mid, mct = mct, dir = dir, amount = amount }, splitOn: "UserId").ToArray();

                return data;
            }
        }

        public virtual TViewModel[] GetPopular(ModelCoreType mct, int? mid=null, int amount=10)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<TViewModel>("dbo.get_popular_materials @mid, @mct, @amount", new { mct = mct, mid = mid, amount = amount });
                return data.ToArray();
            }
        }

        public virtual TViewModel[] Last(ModelCoreType? mct=null, int amount=5, int? mid=null)
        {
            return new TViewModel[0];
        }
    }
}
