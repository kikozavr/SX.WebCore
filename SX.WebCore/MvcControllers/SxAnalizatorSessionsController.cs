using HtmlAgilityPack;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;
using SX.WebCore.ViewModels;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles="seo")]
    public abstract class SxAnalizatorSessionsController : SxBaseController
    {
        private static SxRepoAnalizatorSession _repo = new SxRepoAnalizatorSession();
        public static SxRepoAnalizatorSession Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static readonly int _pageSize = 10;

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order, UserId=User.Identity.GetUserId() };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(SxVMAnalizatorSession filterModel, SxOrder order, int page = 1, int? sessionId=null)
        {
            ViewBag.SessionId = sessionId;

            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, UserId=User.Identity.GetUserId() };

            var viewModel = (await _repo.ReadAsync(filter));
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var host = "http://" + Request.Url.Authority;
            var set = new HashSet<string>();

            try
            {
                var doc = await gGetPageHtml(host);
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute att = link.Attributes["href"];
                    if(!set.Contains(att.Value))
                        set.Add(att.Value);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Analizator", new { messageCode = 1 }); //Ошибка создания сессии
            }

            set.RemoveWhere(x => x.Contains("http") || x=="#" || x=="/" || !x.Contains("/"));
            await createSession(User.Identity.GetUserId(), set);

            return RedirectToAction("Index", "Analizator");
        }
        private async Task<HtmlDocument> gGetPageHtml(string url)
        {
            return await Task.Run(() =>
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = Settings.adminPanelName;
                var response = (HttpWebResponse)request.GetResponse();
                var statusCode = response.StatusCode;

                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        HtmlDocument htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(reader.ReadToEnd());
                        return htmlDoc;
                    }
                }
            });
        }
        private static async Task createSession(string userId, HashSet<string> links)
        {
            var model = new SxAnalizatorSession { UserId = userId };
            model.Urls = links.Select(x => new SxAnalizatorUrl { Url = x }).ToArray();
            await Repo.CreateAsync(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int sessionId)
        {
            var data = await Repo.GetByKeyAsync(sessionId);
            if (data == null)
                return new HttpNotFoundResult();

            await Repo.DeleteAsync(data);

            return RedirectToAction("Index", new { sessionId = sessionId });
        }
    }
}
