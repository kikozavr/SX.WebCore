using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Collections.Generic;

namespace SX.WebCore.Repositories
{
    public class RepoMaterialCategory<TDbContext> : SxDbRepository<string, SxMaterialCategory, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxMaterialCategory> Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dmc.*" });
            query += " FROM D_MATERIAL_CATEGORY AS dmc ";

            object param = null;
            query += getMaterialGroupWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("DateCreate", SortDirection.Desc);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialCategory>(query, param: param);
                return data.AsQueryable();
            }
        }

        private static string getMaterialGroupWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE dmc.ModelCoreType=@mct ";

            var mct = filter.ModelCoreType;

            param = new
            {
                mct = mct
            };

            return query;
        }

        public override SxMaterialCategory Update(SxMaterialCategory model, object[] additionalData, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            var exist = GetByKey(model.Id);
            var isRedactedId = exist == null;
            if(isRedactedId)
            {
                var oldId = additionalData[0];
                var query = @"BEGIN TRANSACTION

UPDATE DV_MATERIAL
SET CategoryId = @id
WHERE CategoryId = @oldid

ALTER TABLE D_MATERIAL_CATEGORY NOCHECK CONSTRAINT [FK_dbo.D_MATERIAL_CATEGORY_dbo.D_MATERIAL_CATEGORY_ParentCategoryId]

UPDATE D_MATERIAL_CATEGORY
SET    Id = @id
WHERE  Id = @oldid

UPDATE D_MATERIAL_CATEGORY
SET    ParentCategoryId = @id
WHERE  ParentCategoryId = @oldid

ALTER TABLE [dbo].[D_MATERIAL_CATEGORY] CHECK CONSTRAINT [FK_dbo.D_MATERIAL_CATEGORY_dbo.D_MATERIAL_CATEGORY_ParentCategoryId]

COMMIT TRANSACTION";

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute(query, new {
                        id=model.Id,
                        oldid= oldId,
                        title=model.Title,
                        mct=model.ModelCoreType,
                        fpid=model.FrontPictureId
                    });
                }

                return GetByKey(model.Id);
            }
            else
                return base.Update(model, changeDateUpdate, propertiesForChange);
        }

        public override void Delete(params object[] id)
        {
            var key = (string)id[0];

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("del_material_category @catId", new { catId= key });
            }
        }
    }
}
