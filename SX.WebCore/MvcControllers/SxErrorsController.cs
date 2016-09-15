using SX.WebCore.Managers;
using SX.WebCore.ViewModels;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendErrorToArchitect(SxVMSiteError model)
        {
            var smtpUserName = ConfigurationManager.AppSettings["NoReplyEmail"];
            var smtpUserPassword= ConfigurationManager.AppSettings["NoReplyEmailPassword"];
            var smtpHost = ConfigurationManager.AppSettings["NoReplySmtpHost"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["NoReplySmtpPort"]);

            var sb = new StringBuilder();
            sb.AppendFormat("Относительный адрес: {0}", model.RawUrl);
            sb.AppendLine(model.Html);

            var mailManager = new SxAppMailManager(smtpUserName, smtpUserPassword, smtpHost, smtpPort);
            var result = await mailManager.SendMail(
                mailFrom: smtpUserName,
                body: sb.ToString(),
                mailsTo: new string[] { "simlex.dev.2014@gmail.com" },
                subject: "Ошибка сайта " + Request.Url.Host,
                isBodyHtml: false
                );

            return Json(result);
        }
    }
}
