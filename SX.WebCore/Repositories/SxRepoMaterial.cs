﻿using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static SX.WebCore.Enums;

namespace SX.WebCore.Repositories
{
    public class SxRepoMaterial<TModel, TDbContext> : SxDbRepository<int, TModel, TDbContext>
        where TModel : SxMaterial
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

        [Obsolete]
        public virtual TViewModel GetByTitleUrl<TViewModel>(int year, string month, string day, string titleUrl) where TViewModel : SxVMMaterial
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<TViewModel>("get_material_by_url @year, @month, @day, @title_url, @mct", new { year = year, month = month, day = day, title_url = titleUrl, mct = _mct }).SingleOrDefault();
                if (data != null)
                    data.Videos = conn.Query<SxVideo>("get_material_videos @mid, @mct", new { mid = data.Id, mct = data.ModelCoreType }).ToArray();
                return data;
            }
        }

        public virtual TModel GetByTitleUrl(int year, string month, string day, string titleUrl)
        {
            return null;
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

        public virtual SxVMMaterial[] GetLikeMaterial(SxFilter filter, int amount=10)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMMaterial, SxVMAppUser, SxVMMaterial>("dbo.get_like_materials @amount, @mid, @mct", (m, u) =>
                {
                    m.User = u;
                    return m;
                }, new { mid = filter.MaterialId, mct = filter.ModelCoreType, amount = amount }, splitOn: "Id");
                return data.ToArray();
            }
        }

        public virtual SxVMMaterial[] GetByDateMaterials(int mid, ModelCoreType mct, bool dir = false, int amount = 3)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMMaterial, SxVMAppUser, SxVMMaterial>("dbo.get_other_materials @mid, @mct, @dir, @amount", (m, u) => {
                    m.User = u;
                    return m;
                }, new { mid = mid, mct = mct, dir = dir, amount = amount }, splitOn: "UserId").ToArray();

                return data;
            }
        }
    }
}
