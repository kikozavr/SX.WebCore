using Dapper;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class RepoEmployee<TDbContext> : SxDbRepository<string, SxEmployee, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxEmployee> All
        {
            get
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxEmployee, SxAppUser, SxEmployee>("get_employees @id", (e, u)=> {
                        e.User = u;
                        return e;
                    }, new {
                        id =(string)null
                    });
                    return data.AsQueryable();
                }
            }
        }

        public override SxEmployee GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxEmployee, SxAppUser, SxEmployee>("get_employees @id", (e, u) =>
                {
                    e.User = u;
                    return e;
                }, new
                {
                    id = id[0]
                }).SingleOrDefault();

                return data;
            }
        }

        public override SxEmployee Create(SxEmployee model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_employee @uid", new
                {
                    uid = model.Id
                });
            }
            return null;
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_employee @uid", new
                {
                    uid = id[0]
                });
            }
        }
    }
}
