using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System;
using System.Collections.Generic;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoShareButton<TDbContext> : SxDbRepository<int, SxShareButton, TDbContext> where TDbContext : SxDbContext
    {
        public override SxShareButton[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM D_SHARE_BUTTON AS dlb
JOIN D_NET AS dn ON dn.Id = dlb.NetId ";

            object param = null;
            query += getShareButtonsWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "NetName", Direction = SortDirection.Asc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order ?? defaultOrder, new Dictionary<string, string> {
                { "NetName", "dn.Name"}
            });

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxShareButton, SxNet, SxShareButton>(query, (b,n)=> {
                    b.Net = n;
                    return b;
                }, param: param, splitOn:"Id");
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SHARE_BUTTON AS dlb
JOIN D_NET AS dn ON dn.Id = dlb.NetId ";

            object param = null;
            query += getShareButtonsWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<int>(query, param: param).Single();
            }
        }

        private static string getShareButtonsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dn.Name LIKE '%'+@netName+'%' OR @netName IS NULL)";

            var netName = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NetName != null ? (string)filter.WhereExpressionObject.NetName : null;

            param = new
            {
                netName = netName
            };

            return query;
        }

        public override SxShareButton Create(SxShareButton model)
        {
            throw new NotImplementedException("Создание сети не поддерживается");
        }

        public override SxShareButton Update(SxShareButton model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxShareButton, SxNet, SxShareButton>("dbo.update_share_button @id, @show, @showCounter", (b, n) => {
                    b.Net = n;
                    return b;
                }, new {
                    id=model.Id,
                    show=model.Show,
                    showCounter=model.ShowCounter
                }, splitOn: "Id");

                return data.SingleOrDefault();
            }
        }

        public override void Delete(params object[] id)
        {
            throw new NotImplementedException("Удаление сети не поддерживается");
        }

        public SxShareButton[] ShareButtonsList
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxShareButton, SxNet, SxShareButton>("dbo.get_share_buttons_list", (b, n) => {
                        b.Net = n;
                        return b;
                    }, splitOn: "Id");

                    return data.ToArray();
                }
            }
        }
    }
}
