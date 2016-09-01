﻿using SX.WebCore.Attrubutes;
using SX.WebCore.Managers;
using SX.WebCore.MvcApplication;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class SxPicturesController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoPicture<TDbContext> _repo;
        protected static SxRepoPicture<TDbContext> Repo
        {
            get
            {
                return _repo;
            }
            set
            {
                _repo = value;
            }
        }
        public SxPicturesController()
        {
            if (_repo == null)
                _repo = Repo;
        }

        private static int _pageSize = 20;
        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost]
        public virtual async Task<PartialViewResult> Index(SxVMPicture filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

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
            return View(Mapper.Map<SxPicture, SxVMEditPicture>(viewModel));
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMany(HttpPostedFileBase[] files)
        {
            return await Task.Run(() =>
            {
                var data = files.Where(x => x.ContentLength <= maxSize && allowFormats.Contains(x.ContentType));
                foreach (var file in data)
                {
                    var redactModel = new SxPicture
                    {
                        Caption = file.FileName,
                        ImgFormat = file.ContentType
                    };
                    redactModel = getImage(redactModel, file);
                    _repo.Create(redactModel);
                }
                return RedirectToAction("Index");
            });
        }

        private static int maxSize = int.Parse(ConfigurationManager.AppSettings["MaxPictureLength"]);
        private static string[] allowFormats = new string[] {
                "image/jpeg",
                "image/png",
                "image/gif"
            };
        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditPicture picture, HttpPostedFileBase file)
        {
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
            //var imgFormat = string.Empty;
            //var isExist = false;
            //var inCache = false;
            //var pictureBytes = await Task.Run(() => { return getPicture(id, out imgFormat, out isExist, out inCache, width, height); });

            //if (!isExist || pictureBytes == null)
            //{
            //    Response.StatusCode = 404;
            //    return null;
            //}

            //return new FileStreamResult(new MemoryStream(pictureBytes), imgFormat);

            var data = await _repo.GetByKeyAsync(id);
            if (data == null)
                return new HttpNotFoundResult();

            if (width.HasValue && data.Width > width)
                data.OriginalContent = SxPictureProvider.ScaleImage(data.OriginalContent, SxPictureProvider.ImageScaleMode.Width, destWidth: width);
            else if (height.HasValue && data.Height > height)
                data.OriginalContent = SxPictureProvider.ScaleImage(data.OriginalContent, SxPictureProvider.ImageScaleMode.Height, destHeight: height);

            return new FileStreamResult(new MemoryStream(data.OriginalContent), data.ImgFormat);

        }

        //private static string getPictureName(Guid id, int? width = null, int? height = null)
        //{
        //    var sb = new StringBuilder();
        //    sb.Append(id.ToString().ToLower());
        //    if (width.HasValue && !height.HasValue)
        //        sb.AppendFormat("_w{0}", width);
        //    else if (!width.HasValue && height.HasValue)
        //        sb.AppendFormat("_h{0}", height);
        //    else if (width.HasValue && height.HasValue)
        //        sb.AppendFormat("_w{0}_h{1}", width, height);

        //    return "pic_" + sb.ToString();
        //}

        //private static byte[] getPicture(Guid id, out string imgFormat, out bool isExists, out bool inCache, int? width = null, int? height = null)
        //{
        //    var cache = SxApplication<TDbContext>.AppCache;
        //    var name = getPictureName(id, width, height);
        //    var picture = (SxPicture)cache.Get(name);
        //    if (picture == null)
        //    {
        //        inCache = false;
        //        picture = _repo.GetByKey(id);
        //        isExists = picture != null;
        //        imgFormat = isExists ? picture.ImgFormat : null;

        //        if (!isExists) return null;

        //        if (width.HasValue && picture.Width > width)
        //            picture.OriginalContent = SxPictureProvider.ScaleImage(picture.OriginalContent, SxPictureProvider.ImageScaleMode.Width, destWidth: width);
        //        else if (height.HasValue && picture.Height > height)
        //            picture.OriginalContent = SxPictureProvider.ScaleImage(picture.OriginalContent, SxPictureProvider.ImageScaleMode.Height, destHeight: height);

        //        cache.Add(name, picture, SxCacheExpirationManager.GetExpiration(minutes: pictureCachingSeconds/60));
        //    }
        //    else
        //    {
        //        inCache = true;
        //        imgFormat = picture.ImgFormat;
        //        isExists = true;
        //    }

        //    return picture.OriginalContent;
        //}

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
    }
}
