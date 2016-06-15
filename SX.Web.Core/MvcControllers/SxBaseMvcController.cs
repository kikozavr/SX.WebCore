using System.Web.Mvc;

namespace SX.Web.Core.MvcControllers
{
    public abstract class SxBaseMvcController : Controller
    {
        private string _areaName;
        private string _controllerName;
        private string _actionName;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeDataValues = filterContext.RequestContext.RouteData.Values;
            _areaName = (string)routeDataValues["area"];
            _controllerName = (string)routeDataValues["controller"];
            _actionName = (string)routeDataValues["action"];

            base.OnActionExecuting(filterContext);
        }

        protected string SxAreaName
        {
            get
            {
                return _areaName;
            }
        }

        protected string SxControllerName
        {
            get
            {
                return _controllerName;
            }
        }

        protected string SxActionName
        {
            get
            {
                return _actionName;
            }
        }
    }
}
