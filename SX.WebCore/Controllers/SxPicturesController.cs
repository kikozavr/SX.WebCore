using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using System;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SX.WebCore.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class SxPicturesController<TDbContext> : Controller where TDbContext : SxDbContext
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
        private static MemoryCache _cache;
        private static object _lck = new object();
        static SxPicturesController()
        {
            _repo = new RepoPicture<TDbContext>();
            _cache = new MemoryCache("CACHE_PICTURES");
        }

        protected SxDbRepository<Guid, SxPicture, TDbContext> Repo
        {
            get
            {
                return _repo;
            }
        }

        [HttpGet]
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
            var name = "pic_"+getPictureName(id, width, height);
            var picture = (SxPicture)_cache[name];
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
                    if(_cache[name]==null)
                        _cache.Add(name, picture, _defaultPolicy);
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
