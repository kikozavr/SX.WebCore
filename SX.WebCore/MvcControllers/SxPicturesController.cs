using SX.WebCore.Attrubutes;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxPicturesController : SxBaseController
    {
        private static SxRepoPicture _repo = new SxRepoPicture();
        public static SxRepoPicture Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 20;
        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost]
        public virtual async Task<ActionResult> Index(SxVMPicture filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ActionResult Edit(Guid? id)
        {
            var viewModel = id.HasValue ? _repo.GetByKey(id) : new SxPicture();
            if (id.HasValue && viewModel == null)
                return new HttpNotFoundResult();
            return View(Mapper.Map<SxPicture, SxVMPicture>(viewModel));
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMany(HttpPostedFileBase[] files)
        {
            var httpContext = System.Web.HttpContext.Current;
            return await Task.Run(() =>
            {
                System.Web.HttpContext.Current = httpContext;
                var data = files.Where(x => x.ContentLength <= maxSize && allowFormats.Contains(x.ContentType));
                foreach (var file in data)
                {
                    var redactModel = new SxPicture
                    {
                        Caption = file.FileName,
                        ImgFormat = file.ContentType,
                        Description=string.Format("Name: {0} Type: {1} Size: {2} byte", file.FileName, file.ContentType, file.ContentLength)
                    };
                    redactModel = getImage(redactModel, file);
                    _repo.Create(redactModel);
                }
                return RedirectToAction("Index");
            });
        }

        private static readonly int maxSize = int.Parse(ConfigurationManager.AppSettings["MaxPictureLength"]);
        private static readonly string[] allowFormats = new string[] {
                "image/jpeg",
                "image/png",
                "image/gif"
            };
        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMPicture picture, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > maxSize)
                ModelState.AddModelError("Caption", string.Format("Размер файла не должен превышать {0} kB", maxSize / 1024));
            if (file != null && !allowFormats.Contains(file.ContentType))
                ModelState.AddModelError("Caption", string.Format("Недопустимый формат файла {0}", file.ContentType));

            if (ModelState.IsValid)
            {
                var isNew = picture.Id == Guid.Empty;
                var redactModel = Mapper.Map<SxVMPicture, SxPicture>(picture);
                if (isNew)
                {
                    redactModel = getImage(redactModel, file);
                    _repo.Create(redactModel);
                }
                else
                {
                    if (file != null)
                    {
                        redactModel = getImage(redactModel, file);
                        _repo.Update(redactModel, true, "Caption", "Description", "OriginalContent", "Width", "Height", "Size", "ImgFormat");
                    }
                    else
                    {
                        _repo.Update(redactModel, true, "Caption", "Description");
                    }
                }
                return RedirectToAction("Index");
            }

            return View(picture);
        }
        private static SxPicture getImage(SxPicture redactModel, HttpPostedFileBase file)
        {
            byte[] imageData = null;
            Image image = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                imageData = binaryReader.ReadBytes(file.ContentLength);
                image = Image.FromStream(file.InputStream);
            }
            redactModel.OriginalContent = imageData;
            redactModel.Width = image.Width;
            redactModel.Height = image.Height;
            redactModel.Size = imageData.Length;

            return redactModel;
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxPicture model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        private const int pictureCachingSeconds = 3600;

        [HttpGet]
        [NotLogRequest]
        [OutputCache(Duration = pictureCachingSeconds, VaryByParam = "id;width;height", Location = OutputCacheLocation.ServerAndClient)]
        public async virtual Task<ActionResult> Picture(Guid id, int? width = null, int? height = null)
        {
            var data = await _repo.GetByKeyAsync(id);
            if (data == null)
                return new HttpNotFoundResult();

            if (width.HasValue && data.Width > width)
                data.OriginalContent = SxPictureProvider.ScaleImage(data.OriginalContent, SxPictureProvider.ImageScaleMode.Width, destWidth: width);
            else if (height.HasValue && data.Height > height)
                data.OriginalContent = SxPictureProvider.ScaleImage(data.OriginalContent, SxPictureProvider.ImageScaleMode.Height, destHeight: height);

            return new FileStreamResult(new MemoryStream(data.OriginalContent), data.ImgFormat);

        }

        [HttpPost]
        [Authorize(Roles = "photo-redactor")]
        public virtual PartialViewResult FindGridView(SxVMPicture filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = _repo.Read(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }

        private static readonly int _freePageSize = 10;

        [HttpGet]
        [Authorize(Roles = "photo-redactor")]
        public ActionResult FreePictures(int page = 1)
        {
            var order = new SxOrder { FieldName = "Size", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _freePageSize) { Order = order };

            var viewModel = Repo.GetFreePictures(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_FreePictures", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "photo-redactor")]
        public async Task<ActionResult> FreePictures(SxVMPicture filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _freePageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await Repo.GetFreePicturesAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_FreePictures", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "photo-redactor")]
        public async Task<ActionResult> DeleteFreePictures()
        {
            var filter = new SxFilter(int.MaxValue, _freePageSize);
            var freePicturesId = (await Repo.GetFreePicturesAsync(filter)).Select(x=>x.Id).ToList();
            var data = await Repo.DeleteFreePicturesAsync(freePicturesId);

            //получаем оставшиеся фото
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            filter = new SxFilter(1, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }
    }
}
