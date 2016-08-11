using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoShareButton<TDbContext> : SxDbRepository<int, SxShareButton, TDbContext> where TDbContext : SxDbContext
    {
        public override SxShareButton[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_SHARE_BUTTON AS dlb ");
            var joinString = @" JOIN D_NET AS dn ON dn.Id = dlb.NetId ";
            sb.Append(joinString);

            object param = null;
            var gws = getShareButtonsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "NetName", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order ?? defaultOrder, new Dictionary<string, string> {
                { "NetName", "dn.Name"}
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_SHARE_BUTTON AS dlb ");
            sbCount.Append(joinString);
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxShareButton, SxNet, SxShareButton>(sb.ToString(), (b, n) => {
                    b.Net = n;
                    return b;
                }, param: param, splitOn: "Id");
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getShareButtonsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dn.Name LIKE '%'+@netName+'%' OR @netName IS NULL)");

            var netName = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NetName != null ? (string)filter.WhereExpressionObject.NetName : null;

            param = new
            {
                netName = netName
            };

            return query.ToString();
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
