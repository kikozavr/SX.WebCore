using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using SX.WebCore.MvcApplication;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class SxPicturesController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxDbRepository<Guid, SxPicture, TDbContext> _repo;
        private static CacheItemPolicy _defaultPolicy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
                };
            }
        }
        private static object _lck = new object();
        static SxPicturesController()
        {
            _repo = new SxRepoPicture<TDbContext>();
        }
        protected SxDbRepository<Guid, SxPicture, TDbContext> Repo
        {
            get
            {
                return _repo;
            }
        }

        private static int _pageSize = 20;
        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = (_repo as SxRepoPicture<TDbContext>).Query<SxVMPicture>(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost]
        public virtual PartialViewResult Index(SxVMPicture filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = (_repo as SxRepoPicture<TDbContext>).Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = (_repo as SxRepoPicture<TDbContext>).Query<SxVMPicture>(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? Repo.GetByKey(id) : new SxPicture();
            return View(Mapper.Map<SxPicture, SxVMEditPicture>(model));
        }

        private static int maxSize = 307200;
        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditPicture picture, HttpPostedFileBase file)
        {
            var allowFormats = new string[] {
                "image/jpeg",
                "image/png",
                "image/gif"
            };

            if (file != null && file.ContentLength > maxSize)
                ModelState.AddModelError("Caption", string.Format("Размер файла не должен превышать {0} kB", maxSize / 1024));
            if (file != null && !allowFormats.Contains(file.ContentType))
                ModelState.AddModelError("Caption", string.Format("Недопустимый формат файла {0}", file.ContentType));

            if (ModelState.IsValid)
            {
                var isNew = picture.Id == Guid.Empty;
                var redactModel = Mapper.Map<SxVMEditPicture, SxPicture>(picture);
                if (isNew)
                {
                    redactModel = getImage(redactModel, file);
                    Repo.Create(redactModel);
                }
                else
                {
                    if (file != null)
                    {
                        redactModel = getImage(redactModel, file);
                        Repo.Update(redactModel, true, "Caption", "Description", "OriginalContent", "Width", "Height", "Size", "ImgFormat");
                    }
                    else
                    {
                        Repo.Update(redactModel, true, "Caption", "Description");
                    }
                }
                return RedirectToAction("index");
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
        public virtual ActionResult Delete(Guid id)
        {
            _repo.Delete(id);
            return RedirectToAction("index");
        }

        [HttpGet]
        [NotLogRequest]
        [OutputCache(Duration = 900, VaryByParam = "id;width;height")]
        public async virtual Task<ActionResult> Picture(Guid id, int? width = null, int? height = null)
        {
            return await Task.Run(() =>
            {
                var imgFormat = string.Empty;
                var isExist = false;
                var inCache = false;
                var picture = getPicture(id, out imgFormat, out isExist, out inCache, width, height);

                if (!isExist)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                if(inCache)
                    Response.StatusCode = 304;

                using (var ms = new MemoryStream(picture))
                {
                    return new FileStreamResult(new MemoryStream(picture), imgFormat);
                }
            });
        }

        private static string getPictureName(Guid id, int? width = null, int? height = null)
        {
            var sb = new StringBuilder();
            sb.Append(id.ToString().ToLower());
            if (width.HasValue && !height.HasValue)
                sb.AppendFormat("_w{0}", width);
            else if (!width.HasValue && height.HasValue)
                sb.AppendFormat("_h{0}", height);
            else if (width.HasValue && height.HasValue)
                sb.AppendFormat("_w{0}_h{1}", width, height);

            return sb.ToString();
        }

        private byte[] getPicture(Guid id, out string imgFormat, out bool isExists, out bool inCache, int? width = null, int? height = null)
        {
            var cache = SxApplication<TDbContext>.AppCache;
            var name = "pic_"+getPictureName(id, width, height);
            var picture = (SxPicture)cache.Get(name);
            if (picture == null)
            {
                inCache = false;
                picture = _repo.GetByKey(id);
                isExists = picture!=null;
                imgFormat = isExists ? picture.ImgFormat : null;

                if (!isExists) return null;

                if (width.HasValue && picture.Width > width)
                    picture.OriginalContent = SxPictureProvider.ScaleImage(picture.OriginalContent, SxPictureProvider.ImageScaleMode.Width, destWidth: width);
                else if (height.HasValue && picture.Height > height)
                    picture.OriginalContent = SxPictureProvider.ScaleImage(picture.OriginalContent, SxPictureProvider.ImageScaleMode.Height, destHeight: height);

                lock(_lck)
                {
                    if(cache.Get(name) == null)
                        cache.Add(name, picture, _defaultPolicy);
                }
            }
            else
            {
                inCache = true;
                imgFormat = picture.ImgFormat;
                isExists = true;
            }

            return picture.OriginalContent;
        }
    }
}
