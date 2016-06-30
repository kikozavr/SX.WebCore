using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSiteTest<TDbContext> : SxDbRepository<int, SxSiteTest, TDbContext> where TDbContext : SxDbContext
    {
        public override SxVMSiteTest[] Query<SxVMSiteTest>(SxFilter filter)
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
                var data = conn.Query<SxVMSiteTest>(query, param: param);
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

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc = desc
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

        public DataTable GetMatrix(int testId)
        {
            var result=new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("get_site_test_matrix @testId", conn))
                {
                    adp.SelectCommand.Parameters.AddWithValue("testId", testId);
                    adp.Fill(result);
                }
            }
            return result;
        }

        //public SxSiteTest LoadFromFile(HttpPostedFileBase file)
        //{
        //    var test = new SxSiteTest();
        //    var testBlocks = new List<SxSiteTestBlock>();
        //    var blockQuestions = new SxSiteTestQuestion[0];

        //    using (ExcelPackage pck = new ExcelPackage(file.InputStream))
        //    {
        //        var ws = pck.Workbook.Worksheets["test"];

        //        var range = ws.Cells["A1"];
        //        test.Title = range.Value.ToString();

        //        range = ws.Cells["A2"];
        //        test.Description = range.Value.ToString();

        //        //100 вопросов
        //        range = ws.Cells["C6:CX6"];
        //        var qList = new List<SxSiteTestQuestion>();
        //        foreach (var cell in range)
        //        {
        //            if (cell.Value != null)
        //                qList.Add(new SxSiteTestQuestion
        //                {
        //                    Text = cell.Value.ToString()
        //                });
        //            else
        //                break;
        //        }
        //        blockQuestions = qList.ToArray();

        //        range = ws.Cells["A7"];
        //        fillBlocksFromRows(testBlocks, blockQuestions, ws, range);
        //        test.Blocks = testBlocks;

        //        return test;
        //    }

        //}
        //private static void fillBlocksFromRows(List<SxSiteTestBlock> testBlocks, SxSiteTestQuestion[] blockQuestions, ExcelWorksheet ws, ExcelRange range)
        //{
        //    var block = new SxSiteTestBlock();
        //    block.Title = range.Value.ToString();

        //    var startRow = range.Start.Row;

        //    range = ws.Cells["B" + startRow];
        //    if (range.Value != null)
        //        block.Description = range.Value.ToString();

        //    var cells = ws.Cells["C" + startRow + ":CX" + startRow].ToArray();
        //    SxSiteTestQuestion q;
        //    object value = null;
        //    var questions = new List<SxSiteTestQuestion>();
        //    for (int i = 0; i < blockQuestions.Length; i++)
        //    {
        //        q = blockQuestions[i];
        //        value = cells[i].Value;
        //        questions.Add(new SxSiteTestQuestion
        //        {
        //            Text = q.Text,
        //            IsCorrect = value != null ? Convert.ToBoolean(value) : false
        //        });
        //    }
        //    block.Questions = questions;
        //    testBlocks.Add(block);

        //    range = ws.Cells["A" + (startRow + 1)];
        //    if (range.Value != null)
        //        fillBlocksFromRows(testBlocks, blockQuestions, ws, range);
        //}

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

        public SxSiteTestQuestion GetGuessYesNoStep(string ttu, List<SxSiteTestStep> pastQ, out int blocksCount)
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn { Caption = "QuestionText" });
            table.Columns.Add(new DataColumn { Caption = "IsCorrect" });
            table.Columns.Add(new DataColumn { Caption = "Order" });
            foreach (var item in pastQ)
            {
                //table.Rows.Add(item.Question.Text, item.Question.IsCorrect, item.Order);
            }

            using (var conn = new SqlConnection(ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("ttu", ttu);
                p.Add("pastQ", table.AsTableValuedParameter("dbo.SiteTestStep"));
                p.Add("blocksCount", dbType: DbType.Int32, direction: ParameterDirection.Output );

                var data = conn.Query<SxSiteTestQuestion, SxSiteTest, SxSiteTestQuestion>("get_guess_yes_no_step", (q, t) =>
                {
                    return q;
                }, p, splitOn: "Id", commandType: CommandType.StoredProcedure).SingleOrDefault();

                blocksCount = p.Get<int>("blocksCount");

                return data;
            }
        }
    }
}
