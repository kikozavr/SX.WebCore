﻿using Dapper;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoPicture : SxDbRepository<Guid, SxPicture, SxVMPicture>
    {
        public override SxPicture Create(SxPicture model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxPicture>("dbo.add_picture @format, @content, @width, @height, @caption, @desc, @size", new {
                    @format=model.ImgFormat,
                    @content=model.OriginalContent,
                    @width=model.Width,
                    @height=model.Height,
                    @caption=model.Caption,
                    @desc=model.Description,
                    @size=model.Size
                });

                return data.SingleOrDefault();
            }
        }

        public override SxVMPicture[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dp.Id",
                "dp.Caption",
                "dp.[Description]",
                "dp.Width",
                "dp.Height",
                "dp.Size"
            }));
            sb.Append(" FROM D_PICTURE AS dp ");

            object param = null;
            var gws = getPicturesWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_PICTURE AS dp ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMPicture>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getPicturesWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dp.Caption LIKE '%'+@caption+'%' OR @caption IS NULL)");
            query.Append(" AND (dp.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL)");
            query.Append(" AND (dp.Width >=@w OR @w=0)");
            query.Append(" AND (dp.Height >=@h OR @h=0)");

            string caption = filter.WhereExpressionObject?.Caption;
            string desc = filter.WhereExpressionObject?.Description;
            int? w = filter.WhereExpressionObject?.Width ?? 0;
            int? h = filter.WhereExpressionObject?.Height ?? 0;

            param = new
            {
                caption = caption,
                desc = desc,
                w = w,
                h = h
            };

            return query.ToString();
        }

        public override SxPicture GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<SxPicture>("dbo.get_picture @pictureId", new { pictureId = id[0] }).SingleOrDefault();
            }
        }

        public override void Delete(SxPicture model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_picture @pictureId", new { pictureId = model.Id });
            }
        }

        protected Action<StringBuilder> InsertNotFreePictures { get; set; }
        public virtual SxVMPicture[] GetFreePictures(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DECLARE @result TABLE(Id UNIQUEIDENTIFIER)");
            if (InsertNotFreePictures != null)
                InsertNotFreePictures(sb);
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT anu.AvatarId FROM AspNetUsers AS anu)");
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT db.PictureId FROM D_BANNER AS db)");
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dmc.FrontPictureId FROM D_MATERIAL_CATEGORY AS dmc)");
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dm.FrontPictureId FROM DV_MATERIAL AS dm)");
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dpl.PictureId FROM D_PICTURE_LINK AS dpl)");
            //sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dsts.PictureId FROM D_SITE_TEST_SUBJECT AS dsts)");
            sb.AppendLine(" DECLARE @value NVARCHAR(MAX) DECLARE c CURSOR LOCAL FORWARD_ONLY FAST_FORWARD FOR SELECT dss.[Value] FROM   D_SITE_SETTING AS dss OPEN c FETCH NEXT FROM c INTO @value WHILE @@FETCH_STATUS = 0 BEGIN DECLARE @pictureId UNIQUEIDENTIFIER BEGIN TRY SET @pictureId = CAST(@value AS UNIQUEIDENTIFIER) IF EXISTS( SELECT * FROM   D_PICTURE AS dp WHERE  dp.Id = @pictureId ) INSERT INTO @result VALUES ( @pictureId ) END TRY BEGIN CATCH END CATCH FETCH NEXT FROM c INTO @value END CLOSE c DEALLOCATE c");

            object param = null;
            var gws = getPicturesWhereString(filter, out param);

            //count
            sb.Append(" SELECT @count= COUNT(DISTINCT dp.Id) FROM D_PICTURE AS dp ");
            sb.Append(gws);
            sb.Append(" AND dp.Id NOT IN (SELECT Id FROM @result) ");


            sb.Append(" SELECT DISTINCT dp.Id, dp.Caption, dp.[Description], dp.Width, dp.Height, dp.Size, dp.DateCreate");
            sb.Append(" FROM D_PICTURE AS dp ");
            sb.Append(gws);
            sb.Append(" AND dp.Id NOT IN (SELECT Id FROM @result) ");

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            if(filter.PagerInfo.Page < int.MaxValue)
                sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            var p = new DynamicParameters();
            p.AddDynamicParams(param);
            p.Add("count", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMPicture>(sb.ToString(), param: p);
                filter.PagerInfo.TotalItems = p.Get<int>("count");
                return data.ToArray();
            }
        }
        public virtual async Task<SxVMPicture[]> GetFreePicturesAsync(SxFilter filter)
        {
            return await Task.Run(()=> {
                return GetFreePictures(filter);
            });
        }
        public virtual async Task<int> DeleteFreePicturesAsync(List<Guid> pictures)
        {
            if (pictures == null || !pictures.Any())
                return 0;

            var sb = new StringBuilder();
            foreach (var id in pictures)
            {
                sb.AppendFormat(",'{0}'", id);
            }
            sb.Remove(0, 1);

            return await Task.Run(() =>
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<int>("dbo.del_free_pictures @ids", new { ids = sb.ToString() });
                    return data.SingleOrDefault();
                }
            });
        }

        public virtual SxVMPicture[] LinkedPictures(SxFilter filter, bool forMaterial)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] { "dp.*" }));
            sb.AppendFormat(@" FROM D_PICTURE AS dp {0} JOIN D_PICTURE_LINK AS dpl ON dpl.PictureId = dp.Id ", forMaterial ? "" : @"LEFT");

            object param = null;
            var gws = getLinkedPictureWhereString(filter, forMaterial, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new Dictionary<string, string> {
                ["DateCreate"]= "dp.DateCreate"
            }));
            sb.Append(" OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY");

            //count
            var sbCount = new StringBuilder();
            sbCount.AppendFormat("SELECT COUNT(1) FROM D_PICTURE AS dp {0} JOIN D_PICTURE_LINK AS dpl ON dpl.PictureId = dp.Id ", forMaterial ? "" : @"LEFT");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMPicture>(sql: sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        public async Task<SxVMPicture[]> LinkedPicturesAsync(SxFilter filter, bool forMaterial)
        {
            return await Task.Run(() => { return LinkedPictures(filter, forMaterial); }); 
        }

        private static string getLinkedPictureWhereString(SxFilter filter, bool forMaterial, out object param)
        {
            checkLinkedPhotoFilter(filter);

            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dp.Caption LIKE '%'+@caption+'%' OR @caption IS NULL) ");
            if (forMaterial)
            {
                query.Append(" AND (dpl.ModelCoreType = @mct) ");
                query.Append(" AND (dpl.Materialid = @mid) ");
            }
            else
                query.Append(" AND (dp.Id NOT IN (SELECT dpl2.PictureId FROM D_PICTURE_LINK AS dpl2 WHERE dpl2.MaterialId=@mid AND dpl2.ModelCoreType=@mct)) ");


            var caption = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Caption != null ? (string)filter.WhereExpressionObject.Caption : null;

            param = new
            {
                caption = caption,
                mid = filter.AddintionalInfo[0],
                mct = filter.AddintionalInfo[1]
            };

            return query.ToString();
        }
        private static void checkLinkedPhotoFilter(SxFilter filter)
        {
            if (filter.AddintionalInfo == null)
                throw new ArgumentNullException("AddintionalInfo");
            if (filter.AddintionalInfo[0] == null)
                throw new ArgumentNullException("AddintionalInfo.MaterialId");
            if (filter.AddintionalInfo[1] == null)
                throw new ArgumentNullException("AddintionalInfo.ModelCoreType");
        }
        public virtual void AddMaterialPicture(int mid, ModelCoreType mct, Guid pid)
        {
            var query = @"INSERT INTO D_PICTURE_LINK
VALUES
(
	@mid,
	@mct,
	@pid
)";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, pid = pid });
            }
        }
        public virtual void DeleteMaterialPicture(int mid, ModelCoreType mct, Guid pid)
        {
            var query = @"DELETE FROM D_PICTURE_LINK
WHERE MaterialId=@mid AND ModelCoreType=@mct AND PictureId=@pid";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, pid = pid });
            }
        }
    }
}
