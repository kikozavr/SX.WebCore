using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class RepoPicture<TDbContext> : SxDbRepository<Guid, SxPicture, TDbContext> where TDbContext : SxDbContext
    {
        public override SxPicture GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var picture = conn.Query<SxPicture>("select dp.Id, dp.Caption, dp.Description, dp.OriginalContent, dp.Width, dp.Height, dp.ImgFormat, dp.DateCreate from D_PICTURE dp where dp.ID=@PICTURE_ID", new { PICTURE_ID = id }).SingleOrDefault();
                return picture;
            }
        }

        public override void Delete(params object[] id)
        {
            var query = @"BEGIN TRANSACTION

UPDATE DV_MATERIAL
SET    FrontPictureId = NULL
WHERE  FrontPictureId = @picture_id

UPDATE AspNetUsers
SET    AvatarId = NULL
WHERE  AvatarId = @picture_id

UPDATE D_MATERIAL_CATEGORY
SET    FrontPictureId = NULL
WHERE FrontPictureId= @picture_id

UPDATE D_AUTHOR_APHORISM
SET   PictureId = NULL
WHERE PictureId= @picture_id

DELETE FROM D_PICTURE WHERE Id=@picture_id

COMMIT TRANSACTION";

            using (var connection = new SqlConnection(base.ConnectionString))
            {
                connection.Execute(query, new { picture_id = id[0] });
            }
        }
    }
}
