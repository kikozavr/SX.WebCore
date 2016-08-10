using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoAffiliateLink<TDbContext> : SxDbRepository<Guid, SxAffiliateLink, TDbContext> where TDbContext : SxDbContext
    {
        public override SxAffiliateLink Create(SxAffiliateLink model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxAffiliateLink>("dbo.add_affiliate_link @id, @rawUrl, @desc, @cc", new
                {
                    id = Guid.NewGuid(),
                    rawUrl = model.RawUrl,
                    desc = model.Description,
                    cc = model.ClickCost
                });

                return data.SingleOrDefault();
            }
        }

        public override SxAffiliateLink[] Read(SxFilter filter, out int count)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM D_AFFILIATE_LINK AS dal ");

            object param = null;
            var gws = getAffiliateLinksWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dal.RawUrl", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_AFFILIATE_LINK AS dal ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxAffiliateLink>(sb.ToString(), param: param);
                count = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getAffiliateLinksWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dal.RawUrl LIKE '%'+@rawUrl+'%' OR @rawUrl IS NULL) ");
            query.Append(" AND (dal.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL) ");

            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                rawUrl = rawUrl,
                desc= desc
            };

            return query.ToString();
        }

        public override SxAffiliateLink Update(SxAffiliateLink model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxAffiliateLink>("dbo.update_affiliate_link @id, @rawUrl, @desc, @cc", new
                {
                    id = model.Id,
                    rawUrl = model.RawUrl,
                    desc = model.Description,
                    cc = model.ClickCost
                });

                return data.SingleOrDefault();
            }
        }

        public override void Delete(SxAffiliateLink model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Execute("dbo.del_affiliate_link @id", new
                {
                    id = model.Id
                });
            }
        }

        public virtual Task AddViewsAsync(List<string> ids)
        {
            return Task.Run(()=> {

                if (ids == null || !ids.Any()) return;

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var sb = new StringBuilder();
                    ids.ForEach(x=> {
                        sb.AppendFormat(",'{0}'", x);
                    });
                    sb.Remove(0, 1);

                    var data = connection.Execute("dbo.add_affiliate_link_views_count @ids", new
                    {
                        ids = sb.ToString()
                    });
                }
            });
        }

        public override SxAffiliateLink GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxAffiliateLink>("dbo.get_affiliate_link @id", new
                {
                    id = id[0]
                });

                return data.SingleOrDefault();
            }
        }
    }
}
