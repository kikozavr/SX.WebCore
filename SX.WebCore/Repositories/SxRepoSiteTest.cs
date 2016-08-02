using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
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
                show = filter.OnlyShow
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

        public SxSiteTestAnswer GetSiteTestPage(string titleUrl)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestAnswer, SxSiteTestQuestion, SxSiteTestSubject, SxSiteTest, SxSiteTestAnswer>("get_site_test_page @titleUrl", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, new { titleUrl = titleUrl }, splitOn: "Id").SingleOrDefault();

                if (Equals(data.Question.Test.Type, SxSiteTest.SiteTestType.Normal))
                {
                    data.Question.Test.Questions = conn.Query<SxSiteTestQuestion>("dbo.get_site_test_normal_questions @testId, @subjectId, @amount", new { testId = data.Question.TestId, subjectId = data.SubjectId, amount = 3 }).ToArray();
                }

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

        public DataTable GetMatrix(int testId, out int count, int page = 1, int pageSize = 10)
        {
            var result = new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("get_site_test_matrix @testId, @page, @pageSize, @count OUTPUT", conn))
                {
                    adp.SelectCommand.Parameters.AddWithValue("testId", testId);
                    adp.SelectCommand.Parameters.AddWithValue("page", page);
                    adp.SelectCommand.Parameters.AddWithValue("pageSize", pageSize);

                    var par = new SqlParameter { ParameterName = "count", DbType = DbType.Int32, Direction = ParameterDirection.Output };
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
                conn.Execute("revert_site_test_matrix_value @subjectTitle, @questionText, @value", new
                {
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
                var data = conn.Query<SxSiteTest>("add_site_test @title, @desc, @rules, @titleUrl, @type", new
                {
                    title = model.Title,
                    desc = model.Description,
                    rules=model.Rules,
                    titleUrl = UrlHelperExtensions.SeoFriendlyUrl(model.Title),
                    type = model.Type
                }).SingleOrDefault();

                return data;
            }
        }

        public SxSiteTestAnswer GetGuessStep(List<SxVMSiteTestStepGuess> steps, out int subjectsCount)
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn { ColumnName = "QuestionId" });
            table.Columns.Add(new DataColumn { ColumnName = "IsCorrect" });
            table.Columns.Add(new DataColumn { ColumnName = "Order" });
            steps.ForEach(x =>
            {
                table.Rows.Add(x.QuestionId, x.IsCorrect, x.Order);
            });

            var p = new DynamicParameters();
            p.Add("oldSteps", table.AsTableValuedParameter("dbo.OldSiteTestStepGuess"));
            p.Add("subjectsCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestAnswer, SxSiteTestQuestion, SxSiteTestSubject, SxSiteTest, SxSiteTestAnswer>("get_site_test_next_guess_step", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, p, commandType: CommandType.StoredProcedure).SingleOrDefault();

                subjectsCount = p.Get<int>("subjectsCount");

                return data;
            }
        }

        public SxSiteTestAnswer GetNormalStep(List<SxVMSiteTestStepNormal> steps, out int subjectsCount, out int allSubjectsCount)
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn { ColumnName = "QuestionId" });
            table.Columns.Add(new DataColumn { ColumnName = "SubjectId" });
            steps.ForEach(x =>
            {
                table.Rows.Add(x.QuestionId, x.SubjectId);
            });

            var p = new DynamicParameters();
            p.Add("oldSteps", table.AsTableValuedParameter("dbo.OldSiteTestStepNormal"));
            p.Add("subjectsCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("allSubjectsCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestAnswer, SxSiteTestQuestion, SxSiteTestSubject, SxSiteTest, SxSiteTestAnswer>("get_site_test_next_normal_step", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, p, commandType: CommandType.StoredProcedure).SingleOrDefault();

                data.Question.Test.Questions = conn.Query<SxSiteTestQuestion>("dbo.get_site_test_normal_questions @testId, @subjectId, @amount", new { testId = data.Question.TestId, subjectId = data.SubjectId, amount = 3 }).ToArray();

                subjectsCount = p.Get<int>("subjectsCount");
                allSubjectsCount = p.Get<int>("allSubjectsCount");

                return data;
            }
        }
        public SxSiteTestAnswer[] GetNormalResults(int subjectId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestAnswer, SxSiteTestQuestion, SxSiteTestSubject, SxSiteTest, SxSiteTestAnswer>("dbo.get_site_test_normal_results @subjectId", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, new { subjectId=subjectId});

                return data.ToArray();
            }
        }

        public SxSiteTest GetSiteTestRules(int siteTestId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTest>("dbo.get_site_test_rules @testId", new { testId = siteTestId }).SingleOrDefault();
                return data;
            }
        }
    }
}
