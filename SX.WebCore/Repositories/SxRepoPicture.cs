using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoPicture<TDbContext> : SxDbRepository<Guid, SxPicture, TDbContext> where TDbContext : SxDbContext
    {
        public override SxPicture GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<SxPicture>("get_picture @pictureId", new { pictureId = id[0] }).SingleOrDefault();
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
