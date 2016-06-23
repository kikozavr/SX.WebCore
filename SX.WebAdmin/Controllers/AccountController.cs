using Microsoft.AspNet.Identity;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;

namespace SX.WebAdmin.Controllers
{
    public sealed class AccountController : WebCore.MvcControllers.SxAccountController<Infrastructure.DbContext>
    {
        protected override Action<SxVMLogin> ActionLogin
        {
            get
            {
                return RegisterLoginUser;
            }
        }

        protected override Action ActionLogOff
        {
            get
            {
                return UnregisterLoginUser;
            }
        }

        public void UnregisterLoginUser()
        {
            var sessionId = Session.SessionID;
            var usersOnSite = MvcApplication.UsersOnSite;
            if (usersOnSite.ContainsKey(sessionId))
                usersOnSite.Remove(sessionId);
        }

        private void RegisterLoginUser(SxVMLogin model)
        {
            var date = DateTime.Now;
            var sessionId = Session.SessionID;
            var usersOnSite = MvcApplication.UsersOnSite;
            if (!usersOnSite.ContainsKey(sessionId))
                usersOnSite.Add(sessionId, model.Email);
            else
            {
                if (usersOnSite.ContainsValue(model.Email))
                {
                    var key = usersOnSite.SingleOrDefault(x => x.Value == model.Email).Key;
                    usersOnSite.Remove(key);
                }

                usersOnSite[sessionId] = model.Email;
            }

            addStatisticUserLogin(date, model.Email);
        }

        private void addStatisticUserLogin(DateTime date, string email)
        {
            var user = UserManager.FindByEmail(email);
            new SxRepoStatistic<Infrastructure.DbContext>().CreateStatisticUserLogin(date, user.Id);
        }
    }
}