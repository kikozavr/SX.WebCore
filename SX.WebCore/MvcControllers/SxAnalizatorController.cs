﻿using Microsoft.AspNet.Identity;
using SX.WebCore.MvcControllers.Abstract;
using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxAnalizatorController : SxBaseController
    {
        private static readonly int _pageSize = 20;

        [HttpGet]
        public ActionResult Index(int? sessionId=null, int? messageCode=null)
        {
            ViewBag.MessageCode = messageCode;
            ViewBag.SessionId = sessionId;

            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;

            var filter = new SxFilter() { UserId = userId };
            var sessions = SxAnalizatorSessionsController.Repo.Read(filter);
            ViewBag.Sessions = sessions;
            ViewBag.Filter = filter;

            if (sessions.Any())
            {
                filter = new SxFilter(1, _pageSize) { AddintionalInfo = new object[] { sessionId } };
                var urls = SxAnalizatorUrlsController.Repo.Read(filter);
                ViewBag.Urls = urls;
            }

            return View();
        }
    }
}
