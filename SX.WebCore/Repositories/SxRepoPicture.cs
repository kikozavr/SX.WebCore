﻿using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoPicture<TDbContext> : SxDbRepository<Guid, SxPicture, TDbContext> where TDbContext : SxDbContext
    {
        public override SxPicture[] Read(SxFilter filter)
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
                var data = conn.Query<SxPicture>(sb.ToString(), param: param);
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

            var caption = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Caption != null ? (string)filter.WhereExpressionObject.Caption : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;
            var w = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Width != null ? (int)filter.WhereExpressionObject.Width : 0;
            var h = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Height != null ? (int)filter.WhereExpressionObject.Height : 0;

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
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dsts.PictureId FROM D_SITE_TEST_SUBJECT AS dsts)");
            sb.AppendLine(" INSERT INTO @result SELECT dp.Id FROM D_PICTURE AS dp WHERE dp.Id IN(SELECT dm.FrontPictureId FROM DV_MATERIAL AS dm)");
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
    }
}
