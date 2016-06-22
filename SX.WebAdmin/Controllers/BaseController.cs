using System.Web.Mvc;

namespace SX.WebAdmin.Controllers
{
    [Authorize]
    public abstract class BaseController : WebCore.MvcControllers.SxBaseController<Infrastructure.DbContext>
    {
        
    }
}