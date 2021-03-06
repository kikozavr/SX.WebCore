﻿using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System;
using static SX.WebCore.Enums;
using SX.WebCore.ViewModels;
using SX.WebCore.Repositories.Abstract;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoRating : SxDbRepository<int, SxRating, SxVMRating>
    {
        public async Task<double> AddRatingAsync(SxRating model)
        {
            return await Task.Run(() => {

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<double>("dbo.add_material_rating @userId, @value, @sessionId, @mid, @mct", new {
                        userId=model.UserId,
                        value=model.Value,
                        sessionId=model.SessionId,
                        mid=model.Material,
                        mct=model.ModelCoreType
                    });

                    return Math.Round(data.SingleOrDefault(),1);
                }
            });
        }

        public async Task<double> GetRatingAsync(int mid, ModelCoreType mct)
        {
            return await Task.Run(()=> {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<double>("dbo.get_material_rating @mid, @mct", new { mid = mid, mct = mct });
                    return data.SingleOrDefault();
                }
            });
        }
    }
}
