using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteTestController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteTest<TDbContext> _repo;
        public SxSiteTestController()
        {
            if(_repo==null)
                _repo = new SxRepoSiteTest<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query<SxVMSiteTest>(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMSiteTest filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query<SxVMSiteTest>(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(SxVMSiteTest filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= pageSize ? 1 : page;
            var viewModel = _repo.Query<SxVMSiteTest>(filter);

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTest();
            return View(Mapper.Map<SxSiteTest, SxVMEditSiteTest>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditSiteTest model)
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

            var redactModel = Mapper.Map<SxVMEditSiteTest, SxSiteTest>(model);
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
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "TestType", "TitleUrl", "Show");
                    else
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "TestType", "Show");
                }
                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(SxVMEditSiteTest model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpGet]
        public virtual PartialViewResult TestMatrix(int testId)
        {
            var data = _repo.GetMatrix(testId).Select(x => Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(x)).ToArray();
            return PartialView("_Matrix", data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult LoadTestFromFile(HttpPostedFileBase file)
        {
            var blocksRepo = new SxRepoSiteTestBlock<TDbContext>();
            var questionRepo = new SxRepoSiteTestQuestion<TDbContext>();
            var data = _repo.LoadFromFile(file);
            data.TestType = SxSiteTest.SiteTestType.GuessYesNo;

            SxSiteTest test = null;
            SxSiteTestBlock block = null;
            SxSiteTestQuestion question = null;

            test = _repo.All.SingleOrDefault(x => x.Title == data.Title);
            if (test != null)
                _repo.Delete(test.Id);
            var testId = createTest(data, _repo, blocksRepo, questionRepo, ref test, ref block, ref question);

            return RedirectToAction("edit", new { controller="sitetests", id= testId });
        }
        private static int createTest(SxSiteTest data, SxRepoSiteTest<TDbContext> testRepo, SxRepoSiteTestBlock<TDbContext> blocksRepo, SxRepoSiteTestQuestion<TDbContext> questionRepo, ref SxSiteTest test, ref SxSiteTestBlock block, ref SxSiteTestQuestion question)
        {
            test = new SxSiteTest { Title = data.Title, Description = data.Description, TestType = data.TestType };
            test = testRepo.Create(test);

            if (test != null)
            {
                foreach (var b in data.Blocks)
                {
                    block = new SxSiteTestBlock { TestId = test.Id, Title = b.Title, Description = b.Description };
                    block = blocksRepo.Create(block);
                    if (block != null)
                    {
                        foreach (var q in b.Questions)
                        {
                            question = new SxSiteTestQuestion { BlockId = block.Id, Text = q.Text, IsCorrect = q.IsCorrect };
                            question = questionRepo.Create(question);
                        }
                    }
                }
            }

            return test.Id;
        }
    }
}
