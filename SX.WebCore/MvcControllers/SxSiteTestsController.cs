﻿using OfficeOpenXml;
using SX.WebCore.Attrubutes;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteTestsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteTest<TDbContext> _repo=new SxRepoSiteTest<TDbContext>();
        public static SxRepoSiteTest<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(SxVMSiteTest filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> FindGridView(SxVMSiteTest filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTest();
            return View(Mapper.Map<SxSiteTest, SxVMSiteTest>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMSiteTest model)
        {
            var isArchitect = User.IsInRole("architect");
            var isNew = model.Id == 0;
            if (isNew)
            {
                model.TitleUrl = Url.SeoFriendlyUrl(model.Title);
                if (_repo.All.SingleOrDefault(x => x.TitleUrl == model.TitleUrl) != null)
                    ModelState.AddModelError("Title", "Модель с таким текстовым ключем уже существует");
                else
                    ModelState["TitleUrl"].Errors.Clear();
            }
            else
            {
                if (string.IsNullOrEmpty(model.TitleUrl))
                {
                    var url = Url.SeoFriendlyUrl(model.Title);
                    if (_repo.All.SingleOrDefault(x => x.TitleUrl == url && x.Id != model.Id) != null)
                        ModelState.AddModelError(isArchitect ? "TitleUrl" : "Title", "Модель с таким текстовым ключем уже существует");
                    else
                    {
                        model.TitleUrl = url;
                        ModelState["TitleUrl"].Errors.Clear();
                    }
                }
            }

            var redactModel = Mapper.Map<SxVMSiteTest, SxSiteTest>(model);
            if (ModelState.IsValid)
            {
                SxSiteTest newModel = null;
                if (isNew)
                    newModel = _repo.Create(redactModel);
                else
                {
                    var old = _repo.All.SingleOrDefault(x => x.TitleUrl == model.TitleUrl && x.Id != model.Id);
                    if (old != null)
                        ModelState.AddModelError(isArchitect ? "TitleUrl" : "Title", "Модель с таким текстовым ключем уже существует");
                    if (isArchitect)
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "Rules", "Type", "TitleUrl", "Show", "ShowSubjectDesc");
                    else
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "Rules", "Show", "ShowSubjectDesc");
                }
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxSiteTest model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        private readonly int _matrixPageSize = 7;
        [HttpGet]
        public virtual async Task<PartialViewResult> TestMatrix(int testId, int page = 1)
        {
            return await Task.Run(() =>
            {
                var count = 0;
                var data = _repo.GetMatrix(testId, out count, page, _matrixPageSize);
                ViewBag.Count = count;
                ViewBag.PageSize = _matrixPageSize;
                ViewBag.TestId = testId;
                ViewBag.Page = page;
                return PartialView("_Matrix", data);
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual JsonResult RevertMatrixValue(string subjectTitle, string questionText, int value)
        {
            Task.Run(() =>
            {
                _repo.RevertMatrixValue(subjectTitle, questionText, value);
            });
            return Json(value == 0 ? 1 : 0);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<RedirectToRouteResult> LoadFromFile(HttpPostedFileBase file)
        {
            var test = new SiteTest();
            var questions = new List<string>();
            var subjects = new List<string>();

            using (var excel = new ExcelPackage(file.InputStream))
            {
                var ws = excel.Workbook.Worksheets.First();

                var range = ws.Cells["A1"];
                test.Title = range.Value.ToString().Trim();

                range = ws.Cells["B1"];
                test.Desc = range.Value?.ToString().Trim();

                //questions
                range = ws.Cells["C1"];
                var startRow = range.Start.Row;
                var startColumn = range.Start.Column;
                var count = 0;
                while (range.Value != null)
                {
                    count++;
                    questions.Add(range.Value.ToString().Trim());
                    range = ws.Cells[startRow, startColumn + count];
                }
                test.Questions = questions.ToArray();
                count = 0;

                range = ws.Cells["A2"];
                startRow = range.Start.Row;
                startColumn = range.Start.Column;
                while (range.Value != null)
                {
                    count++;
                    subjects.Add(range.Value.ToString().Trim());
                    range = ws.Cells[startRow + count, startColumn];
                }
                test.Subjects = subjects.ToArray();

                range = ws.Cells["A2"];
                TestAnswer answer;
                string subjectTitle;
                string subjectDesc;
                for (int i = 0; i < test.Subjects.Length; i++)
                {
                    subjectTitle = range.Value.ToString().Trim();

                    startColumn++;
                    range = ws.Cells[startRow, startColumn];
                    subjectDesc = range.Value?.ToString().Trim();

                    for (int y = 0; y < test.Questions.Length; y++)
                    {
                        startColumn++;
                        range = ws.Cells[startRow, startColumn];
                        answer = new TestAnswer
                        {
                            SubjectTitle = subjectTitle,
                            SubjectDesc = subjectDesc,
                            Question = test.Questions[y],
                            IsCorrect = range.Value != null && range.Value.ToString() == "1" ? true : false
                        };
                        test.Answers.Add(answer);
                    }
                    startRow++;
                    startColumn = 1;
                    range = ws.Cells[startRow, startColumn];
                }

                var id = await writeSiteTestToDb(test);
                return RedirectToAction("edit", new { id = id });
            }
        }
        private class SiteTest
        {
            public SiteTest()
            {
                Questions = new string[0];
                Subjects = new string[0];
                Answers = new List<TestAnswer>();
            }
            public string Title { get; set; }
            public string Desc { get; set; }
            public string[] Questions { get; set; }
            public string[] Subjects { get; set; }
            public List<TestAnswer> Answers { get; set; }
        }
        private struct TestAnswer
        {
            public string SubjectTitle { get; set; }
            public string SubjectDesc { get; set; }
            public string Question { get; set; }
            public bool IsCorrect { get; set; }
        }
        private async Task<int> writeSiteTestToDb(SiteTest test)
        {
            return await Task.Run(() =>
            {
                var testId = Repo.Create(new SxSiteTest { Title = test.Title, Description = test.Desc }).Id;
                TestAnswer answer;
                string sTitle;
                string q;
                SxSiteTestSubject subject;
                SxSiteTestQuestion question;
                for (int i = 0; i < test.Subjects.Length; i++)
                {
                    sTitle = test.Subjects[i];
                    subject = SxSiteTestSubjectsController<TDbContext>.Repo.Create(new SxSiteTestSubject { Title = sTitle, TestId = testId });
                    for (int y = 0; y < test.Questions.Length; y++)
                    {
                        q = test.Questions[y];
                        answer = test.Answers.Where(x => x.SubjectTitle == sTitle && x.Question == q).SingleOrDefault();
                        question = SxSiteTestQuestionsController<TDbContext>.Repo.Create(new SxSiteTestQuestion { Text = q, TestId = testId });
                        if (answer.IsCorrect)
                            Repo.RevertMatrixValue(sTitle, q, 0);
                    }
                }

                return testId;
            });
        }

        [HttpPost, NotLogRequest, AllowAnonymous]
        public virtual async Task<JsonResult> Rules(int siteTestId)
        {
            return await Task.Run(() =>
            {
                var data = _repo.GetSiteTestRules(siteTestId);
                return Json(new
                {
                    Title = data.Title,
                    Rules = data.Rules
                });
            });
        }

        [HttpPost, AllowAnonymous]
        public async Task<PartialViewResult> ResultNormal(List<SxVMSiteTestStepNormal> steps)
        {
            return await Task.Run(() =>
            {
                var validSteps = steps.Where(x => x.QuestionId != 0).ToArray();
                SxSiteTestAnswer answer;
                SxVMSiteTestStepNormal userAnswer;
                var result = new SXVMSiteTestResult<SxVMSiteTestResultNormal>();
                var data = _repo.GetNormalResults(steps.First().SubjectId);
                var test = data.First().Question.Test;
                result.SiteTestTitle = test.Title;
                result.SiteTestUrl = Url.Action("Details", "SiteTests", new { titleUrl = test.TitleUrl });
                result.Results = new SxVMSiteTestResultNormal[validSteps.Length];

                for (int i = 0; i < validSteps.Length; i++)
                {
                    userAnswer = validSteps[i];
                    answer = data.First(x => x.SubjectId == userAnswer.SubjectId);

                    result.Results[i] = new SxVMSiteTestResultNormal
                    {
                        SubjectTitle = answer.Subject.Title,
                        QuestionText = data.First(x => x.QuestionId == userAnswer.QuestionId).Question.Text,
                        IsCorrect = answer.QuestionId == userAnswer.QuestionId,
                        Step = userAnswer
                    };

                    var isCorrect = answer.QuestionId == userAnswer.QuestionId;
                    result.BallsCount += userAnswer.BallsSubjectShow + (isCorrect ? userAnswer.BallsGoodRead : 0) + userAnswer.BallsBadRead + (isCorrect ? 15 : 0);
                }


                return PartialView("_ResultNormal", result);
            });
        }
    }
}
