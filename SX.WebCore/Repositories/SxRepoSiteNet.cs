using Dapper;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteNet : SxDbRepository<int, SxSiteNet, SxVMSiteNet>
    {
        public override SxSiteNet Create(SxSiteNet model)
        {
            throw new NotImplementedException();
        }

        public override SxVMSiteNet[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM D_SITE_NET AS dsn RIGHT OUTER JOIN D_NET AS dn ON dn.Id = dsn.NetId ");

            object param = null;
            var gws = getSiteNetsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dn.Name", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new System.Collections.Generic.Dictionary<string, string> {
                { "Url", "dsn.Url" },
                { "NetName", "dn.Name"}
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_SITE_NET AS dsn RIGHT OUTER JOIN D_NET AS dn ON dn.Id = dsn.NetId ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMSiteNet, SxVMNet, SxVMSiteNet>(sb.ToString(), (sn, n)=> {
                    sn.Net = n;
                    return sn;
                }, param: param, splitOn: "NetId, Id");
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getSiteNetsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dn.Name LIKE '%'+@netName+'%' OR @netName IS NULL)");
            query.Append(" AND (dsn.Url LIKE '%'+@url+'%' OR @url IS NULL)");

            var netName = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NetName != null ? (string)filter.WhereExpressionObject.NetName : null;
            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;

            param = new
            {
                netName = netName,
                url=url
            };

            return query.ToString();
        }

        public override SxSiteNet Update(SxSiteNet model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxSiteNet, SxNet, SxSiteNet>("dbo.update_site_net @netId, @url, @show", (sn, n) => {
                    sn.Net = n;
                    return sn;
                }, new { netId = model.NetId, url=model.Url, show=model.Show }, splitOn: "NetId, Id");

                return data.SingleOrDefault();
            }
        }

        public override void Delete(SxSiteNet model)
        {
            base.Delete(model);
        }

        public override SxSiteNet GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxSiteNet, SxNet, SxSiteNet>("dbo.get_site_net @netId", (sn, n) => {
                    sn.Net = n;
                    return sn;
                }, new { netId = id[0] }, splitOn: "NetId, Id");

                return data.SingleOrDefault();
            }
        }

        public SxVMSiteNet[] SiteNets
        {
            get
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<SxVMSiteNet, SxVMNet, SxVMSiteNet>("dbo.get_site_nets", (sn, n) => {
                        sn.Net = n;
                        return sn;
                    }, splitOn: "NetId, Id");

                    return data.ToArray();
                }
            }
        }
    }
}
