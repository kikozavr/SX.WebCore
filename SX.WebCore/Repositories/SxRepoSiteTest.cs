using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteTest<TDbContext> : SxDbRepository<int, SxSiteTest, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSiteTest[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += " FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dst.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTest>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dst.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dst.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";
            query += " AND (dst.Show=@show OR @show IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc = desc,
                show= filter.OnlyShow
            };

            return query;
        }

        public dynamic[] RandomList(int amount = 3)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {

                var data = conn.Query("get_random_site_tests @amount", new { amount = amount });
                return data.ToArray();
            }
        }

        public SxSiteTestQuestion GetSiteTestPage(string titleUrl)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTest, SxSiteTestQuestion>("get_site_test_page @titleUrl", (q, t) =>
                {
                    q.Test = t;
                    return q;
                }, new { titleUrl = titleUrl }, splitOn: "Id").SingleOrDefault();

                return data;
            }
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_site_test @testId", new { testId = id[0] });
            }
        }

        public DataTable GetMatrix(int testId, out int count, int page=1, int pageSize=10)
        {
            var result=new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("get_site_test_matrix @testId, @page, @pageSize, @count OUTPUT", conn))
                {
                    adp.SelectCommand.Parameters.AddWithValue("testId", testId);
                    adp.SelectCommand.Parameters.AddWithValue("page", page);
                    adp.SelectCommand.Parameters.AddWithValue("pageSize", pageSize);

                    var par = new SqlParameter { ParameterName= "count", DbType=DbType.Int32, Direction=ParameterDirection.Output };
                    adp.SelectCommand.Parameters.Add(par);

                    adp.Fill(result);

                    count = Convert.ToInt32(par.Value);
                }
            }
            return result;
        }

        public void RevertMatrixValue(string subjectTitle, string questionText, int value)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("revert_site_test_matrix_value @subjectTitle, @questionText, @value", new {
                    subjectTitle = subjectTitle,
                    questionText = questionText,
                    value = value == 0 ? 1 : 0
                });
            }
        }

        public override SxSiteTest Create(SxSiteTest model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTest>("add_site_test @title, @desc, @titleUrl", new
                {
                    title = model.Title,
                    desc = model.Description,
                    titleUrl = UrlHelperExtensions.SeoFriendlyUrl(model.Title)
                }).SingleOrDefault();

                return data;
            }
        }

        public SxSiteTestQuestion GetNextStep(int testId, List<SxVMSiteTestStep> steps)
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn { ColumnName = "QuestionId" });
            table.Columns.Add(new DataColumn { ColumnName = "IsCorrect" });
            steps.ForEach(x=> {
                table.Rows.Add(x.QuestionId, x.IsCorrect);
            });

            var p = new DynamicParameters();
            p.Add("testId", testId);
            p.Add("oldSteps", table.AsTableValuedParameter("dbo.OldSiteTestStep"));
            p.Add("count", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestQuestion, SxSiteTest, SxSiteTestQuestion>("get_site_test_next_step", (q,t)=> {
                    q.Test = t;
                    return q;
                }, p, commandType: CommandType.StoredProcedure).SingleOrDefault();

                var count = p.Get<int>("count");
                return data;
            }
        }
    }
}
