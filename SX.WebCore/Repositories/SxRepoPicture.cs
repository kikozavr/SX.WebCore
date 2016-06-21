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
                return conn.Query<SxPicture>("dbo.get_picture @pictureId", new { pictureId = id[0] }).SingleOrDefault();
            }
        }

        public override void Delete(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_picture @pictureId", new { picture_id = id[0] });
            }
        }
    }
}
