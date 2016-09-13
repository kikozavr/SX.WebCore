using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteTestSubjectsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteTestSubject<TDbContext> _repo=new SxRepoSiteTestSubject<TDbContext>();
        public static SxRepoSiteTestSubject<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 10;

        [HttpPost]
        public virtual async Task<ActionResult> Index(int testId, SxVMSiteTestSubject filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dstq.Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { testId } };

            var viewModel = await _repo.ReadAsync(filter);

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int testId, int? id)
        {
            ViewBag.Prefix = "subject";
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTestSubject() { TestId = testId };
            if (id.HasValue)
            {
                if (model == null)
                    return new HttpNotFoundResult();

                if (model.Picture != null)
                    ViewData["PictureIdCaption"] = model.Picture.Caption;
            }
            var viewModel = new SxVMSiteTestSubject
            {
                Description = model.Description,
                Id = model.Id,
                Title = model.Title,
                PictureId = model.PictureId,
                TestId = model.TestId,
                Picture = Mapper.Map<SxPicture, SxVMPicture>(model.Picture),
                Test = model.Test != null ? new SxVMSiteTest
                {
                    Id = model.Test.Id,
                    Description = model.Test.Description,
                    Rules = model.Test.Rules,
                    Show = model.Test.Show,
                    Title = model.Test.Title,
                    TitleUrl = model.Test.TitleUrl,
                    Type = model.Test.Type,
                    DateCreate = model.Test.DateCreate
                } : null
            };
            return PartialView("_Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Prefix = "subject")] SxVMSiteTestSubject model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMSiteTestSubject, SxSiteTestSubject>(model);
                SxSiteTestSubject newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "TestId", "Title", "Description", "PictureId");

                return getResult(newModel.TestId);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<PartialViewResult> Delete(SxSiteTestSubject model)
        {
            var testId = model.TestId;
            await _repo.DeleteAsync(model);
            return getResult(testId);
        }

        private PartialViewResult getResult(int testId)
        {
            var defaultOrder = new SxOrder { FieldName = "dstq.Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(1, _pageSize) { Order = defaultOrder, AddintionalInfo = new object[] { testId } };
            var viewModel = _repo.Read(filter);
            ViewBag.Filter = filter;
            return PartialView("_GridView", viewModel);
        }
    }
}
