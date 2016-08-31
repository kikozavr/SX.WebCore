using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoProjectStep<TDbContext> : SxDbRepository<int, SxProjectStep, TDbContext, SxVMProjectStep> where TDbContext : SxDbContext
    {
        public override SxVMProjectStep[] Read(SxFilter filter)
        {
            var query = @"WITH j(Id, [Level]) AS (
         SELECT dps.Id,
                1
         FROM   D_PROJECT_STEP AS dps
         WHERE  dps.ParentStepId IS NULL
         UNION ALL
         SELECT dps1.Id,
                j.[Level] + 1
         FROM   D_PROJECT_STEP  AS dps1
                JOIN j          AS j
                     ON  j.Id = dps1.ParentStepId
     )

SELECT dps.*,
       j.[Level]
FROM   j                    AS j
       JOIN D_PROJECT_STEP  AS dps
            ON  dps.Id = j.Id
ORDER BY
       dps.[Order] DESC";

            var queryCount= @"SELECT COUNT(dps.Id)
FROM   D_PROJECT_STEP AS dps";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMProjectStep>(query);
                filter.PagerInfo.TotalItems = connection.Query<int>(queryCount).SingleOrDefault();
                return data.ToArray();
            }
        }

        public override void Delete(SxProjectStep model)
        {
            var query = @"WITH j(Id) AS (
         SELECT dps.Id
         FROM   D_PROJECT_STEP AS dps
         WHERE  dps.Id = @id
         UNION ALL
         SELECT dps1.Id
         FROM   D_PROJECT_STEP  AS dps1
                JOIN j          AS j
                     ON  j.Id = dps1.ParentStepId
     )

DELETE 
FROM   D_PROJECT_STEP
WHERE  Id IN (SELECT j.Id
              FROM   j AS j)";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { id = model.Id });
            }
        }

        public void ReplaceOrder(int id, bool dir, int? osid=null)
        {
            var query = @"BEGIN TRANSACTION
DECLARE @order      SMALLINT,
        @fid        INT = NULL,
        @forder     SMALLINT = NULL

SELECT @order = dps.[Order]
FROM   D_PROJECT_STEP AS dps
WHERE  dps.Id = @id

IF (@dir = 1)
BEGIN
    SELECT TOP(1) @fid = dps.Id,
           @forder = dps.[Order]
    FROM   D_PROJECT_STEP AS dps
    WHERE  dps.ParentStepId = @osid
           AND dps.[Order] > @order
    
    UPDATE D_PROJECT_STEP
    SET    [Order]     = CASE 
                          WHEN @fid IS NOT NULL THEN @forder + 1
                          ELSE @order + 1
                     END
    WHERE  Id          = @id
END
ELSE
BEGIN
    SELECT TOP(1) @fid = dps.Id,
           @forder = dps.[Order]
    FROM   D_PROJECT_STEP AS dps
    WHERE  dps.ParentStepId = @osid
           AND dps.[Order] < @order
    
    UPDATE D_PROJECT_STEP
    SET    [Order]     = CASE 
                          WHEN @fid IS NOT NULL THEN @forder - 1
                          ELSE @order -1
                     END
    WHERE  Id          = @id
END

COMMIT TRANSACTION";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new {
                    id=id,
                    osid=osid,
                    dir=dir
                });
            }
        }

        public void ReplaceDone(int id, bool done)
        {
            var query = @"UPDATE D_PROJECT_STEP
SET    IsDone     = @done
WHERE  Id         = @id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new
                {
                    id = id,
                    done = done
                });
            }
        }
    }
}
