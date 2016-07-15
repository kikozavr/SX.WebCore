using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.Enums;

namespace SX.WebCore.Repositories
{
    public class SxRepoMaterial<TModel, TDbContext> : SxDbRepository<int, TModel, TDbContext>
        where TModel : SxDbModel<int>
        where TDbContext : SxDbContext
    {
        private static ModelCoreType _mct;
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

        public TViewModel GetByTitleUrl<TViewModel>(int year, string month, string day, string titleUrl) where TViewModel : SxVMMaterial
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<TViewModel>("get_material_by_url @year, @month, @day, @title_url, @mct", new { year = year, month = month, day = day, title_url = titleUrl, mct = _mct }).SingleOrDefault();
                if (data != null)
                    data.Videos = conn.Query<SxVideo>("get_material_videos @mid, @mct", new { mid = data.Id, mct = data.ModelCoreType }).ToArray();
                return data;
            }
        }

        public void AddUserView(int mid, ModelCoreType mct)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_material_view @mid, @mct", new { mid = mid, mct = mct });
            }
        }
    }
}
