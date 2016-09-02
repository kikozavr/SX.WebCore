using AutoMapper;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxProjectStepsController<TDbContext>:SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoProjectStep<TDbContext> _repo;
        public SxProjectStepsController()
        {
            if(_repo==null)
                _repo = new SxRepoProjectStep<TDbContext>();
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var filter = new SxFilter();
            var data = _repo.Read(filter).ToArray();
            var parents = data.Where(x => !x.ParentStepId.HasValue).ToArray();
            if (parents.Any())
            {
                for (int i = 0; i < parents.Length; i++)
                {
                    fillSteps(parents[i], data, Mapper);
                }
            }

            return View(parents);
        }

        private static void fillSteps(SxVMProjectStep step, SxVMProjectStep[] steps, IMapper mapper)
        {
            step.Steps = steps.Where(x => x.ParentStepId == step.Id).ToArray();
            if (step.Steps.Any())
            {
                for (int i = 0; i < step.Steps.Length; i++)
                {
                    fillSteps(step.Steps[i], steps, mapper);
                }
            }
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null, int? pid = null)
        {
            var data = id.HasValue ? _repo.GetByKey(id) : new SxProjectStep { ParentStepId = pid };
            var viewModel = Mapper.Map<SxProjectStep, SxVMProjectStep>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMProjectStep model)
        {
            var redactModel = Mapper.Map<SxVMProjectStep, SxProjectStep>(model);
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel, true, "Title", "Foreword", "Html", "ParentStepId");
                return Redirect("/projectsteps/index#pstep-" + model.Id);
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxProjectStep model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual EmptyResult ReplaceOrder(int id, bool dir, int? osid = null)
        {
            _repo.ReplaceOrder(id, dir, osid);
            return null;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectResult ReplaceDone(int id, bool done)
        {
            _repo.ReplaceDone(id, done);
            return Redirect("/projectsteps/index#pstep-" + id);
        }

        [HttpPost]
        public string FillHtml(int id)
        {
            var html = _repo.GetByKey(id).Html;
            return html;
        }
    }
}
