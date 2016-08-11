using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using static SX.WebCore.Enums;
using System.Text;

namespace SX.WebCore.Repositories
{
    public class SxRepoMaterialCategory<TDbContext> : SxDbRepository<string, SxMaterialCategory, TDbContext>
        where TDbContext : SxDbContext
    {
        public override SxMaterialCategory Create(SxMaterialCategory model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialCategory, SxPicture, SxMaterialCategory, SxMaterialCategory>("dbo.add_material_category @categoryId, @title, @mct, @pcid, @pictureId", (c, p, pc) => {
                    c.FrontPicture = p;
                    c.ParentCategory = pc;
                    return c;
                }, new
                {
                    categoryId=model.Id,
                    title=model.Title,
                    mct=model.ModelCoreType,
                    pcid=model.ParentCategoryId,
                    pictureId=model.FrontPictureId
                }, splitOn:"Id").SingleOrDefault();

                return data;
            }
        }

        public override SxMaterialCategory[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_MATERIAL_CATEGORY AS dmc ");

            object param = null;
            var gws = getMaterialCategoriesWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dmc.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_MATERIAL_CATEGORY AS dmc ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxMaterialCategory>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getMaterialCategoriesWhereString(SxFilter filter, out object param)
        {
            param = null;
            var  query = new StringBuilder();
            query.AppendLine(" WHERE dmc.ModelCoreType=@mct ");

            var mct = filter.ModelCoreType;

            param = new
            {
                mct = mct
            };

            return query.ToString();
        }

        public override SxMaterialCategory Update(SxMaterialCategory model, object[] additionalData, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            var exist = GetByKey(model.Id);
            var isRedactedId = exist == null;
            if (isRedactedId)
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
                    conn.Execute(query, new
                    {
                        id = model.Id,
                        oldid = oldId,
                        title = model.Title,
                        mct = model.ModelCoreType,
                        fpid = model.FrontPictureId
                    });
                }

                return GetByKey(model.Id);
            }
            else
                return base.Update(model, changeDateUpdate, propertiesForChange);
        }

        public SxMaterialCategory Update(SxMaterialCategory model, string oldId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialCategory, SxPicture, SxMaterialCategory, SxMaterialCategory>("dbo.update_material_category @oldCategoryId, @categoryId, @title, @mct, @pcid, @pictureId", (c, p, pc) => {
                    c.FrontPicture = p;
                    c.ParentCategory = pc;
                    return c;
                },  new
                {
                    oldCategoryId= oldId,
                    categoryId = model.Id,
                    title=model.Title,
                    mct=model.ModelCoreType,
                    pcid=model.ParentCategoryId,
                    pictureId=model.FrontPictureId
                }, splitOn:"Id").SingleOrDefault();

                return data;
            }
        }

        //public override void Delete(params object[] id)
        //{
        //    var key = (string)id[0];

        //    using (var connection = new SqlConnection(ConnectionString))
        //    {
        //        connection.Execute("dbo.del_material_category @catId", new { catId= key });
        //    }
        //}

        public override void Delete(SxMaterialCategory model)
        {
            var query = @"BEGIN TRANSACTION
	
	DECLARE @idForDel TABLE (Id VARCHAR(100));
	WITH j(Id, ParentCategoryId) AS
	     (
	         SELECT dmc1.Id,
	                dmc1.ParentCategoryId
	         FROM   D_MATERIAL_CATEGORY AS dmc1
	         WHERE  dmc1.Id = @catId
	         UNION ALL
	         SELECT dmc2.Id,
	                dmc2.ParentCategoryId
	         FROM   D_MATERIAL_CATEGORY AS dmc2
	                JOIN j
	                     ON  dmc2.ParentCategoryId = j.Id
	     )
	
	INSERT INTO @idForDel
	SELECT j.Id
	FROM   j AS j
	
	--Обновить статьи и новости
	UPDATE DV_MATERIAL
	SET    CategoryId = NULL
	WHERE  CategoryId IN (SELECT fd.Id
	                      FROM   @idForDel fd) 
	
	--Удалить категорию	         
	DELETE 
	FROM   D_MATERIAL_CATEGORY
	WHERE  Id IN (SELECT fd.Id
	              FROM   @idForDel fd) 
	
	COMMIT TRANSACTION";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { catId = model.Id });
            }
        }

        public override SxMaterialCategory GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialCategory, SxPicture, SxMaterialCategory, SxMaterialCategory>("dbo.get_material_category @categoryId", (c, p, pc)=> {
                    c.FrontPicture = p;
                    c.ParentCategory = pc;
                    return c;
                }, new
                {
                    categoryId = id[0]
                }, splitOn:"Id").SingleOrDefault();

                return data;
            }
        }

        public virtual SxMaterialCategory[] GetByModelCoreType(ModelCoreType mct)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxMaterialCategory>("dbo.get_material_categories_by_mct @mct", new { mct = mct });
                return data.ToArray();
            }
        }
    }
}
