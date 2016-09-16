using Dapper;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System;
using SX.WebCore.Repositories.Abstract;

namespace SX.WebCore.Repositorise
{
    public sealed class SxRepoMaterialTag : SxDbRepository<string, SxMaterialTag, SxVMMaterialTag>
    {
        public override SxMaterialTag Create(SxMaterialTag model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialTag>("dbo.add_material_tag @id, @mid, @mct, @title", new {
                    id = model.Id,
                    mid = model.MaterialId,
                    mct = model.ModelCoreType,
                    title = model.Title
                });
                return data.SingleOrDefault();
            }
        }

        public override SxVMMaterialTag[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dmt.Id",
                "dmt.Title",
                "dmt.DateCreate",
                "dmt.MaterialId",
                "dmt.ModelCoreType"
            }));
            sb.Append(@" FROM D_MATERIAL_TAG AS dmt ");

            object param = null;
            var gws = getMaterialTagWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dmt.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_MATERIAL_TAG AS dmt ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMMaterialTag>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getMaterialTagWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dmt.Id LIKE '%'+@id+'%' OR @id IS NULL)");
            query.Append(" AND (dmt.Title LIKE '%'+@title+'%' OR @title IS NULL)");
            query.Append(" AND (dmt.MaterialId=@mid AND dmt.ModelCoreType=@mct) ");

            var id = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Id != null ? (string)filter.WhereExpressionObject.Id : null;
            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;

            param = new
            {
                id = id,
                title= title,
                mid = filter.MaterialId,
                mct = (byte)filter.ModelCoreType
            };

            return query.ToString();
        }

        /// <summary>
        /// Обновление тега облака материала не поддерживается
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override SxMaterialTag Update(SxMaterialTag model)
        {
            throw new NotImplementedException();
        }

        public override void Delete(SxMaterialTag model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_material_tag @id, @mct", new { id = model.Id, mct = model.ModelCoreType });
            }
        }

        /// <summary>
        /// Тег может быть уникальным в пределах Id и ModelCoreType
        /// </summary>
        /// <param name="id">Значения ключа</param>
        /// <returns></returns>
        public override SxMaterialTag GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialTag>("dbo.get_material_tag @id, @mct", new { id = id[0], mct=id[1] });
                return data.SingleOrDefault();
            }
        }

        public SxVMMaterialTag[] GetCloud(SxFilter filter, int amount = 50)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMMaterialTag>("dbo.get_material_cloud @amount, @mid, @mct", new
                {
                    mid = filter.MaterialId,
                    mct = filter.ModelCoreType,
                    amount = amount
                });
                return data.ToArray();
            }
        }
    }
}
