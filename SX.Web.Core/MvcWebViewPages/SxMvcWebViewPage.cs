using System.Web.Mvc;
namespace SX.Web.Core.MvcWebViewPages
{
    public abstract class SxMvcWebViewPage<TModel> : WebViewPage
    {
        private string _areaName;
        private string _controllerName;
        private string _actionName;

        protected override void InitializePage()
        {
            var routeDataValues = Request.RequestContext.RouteData.Values;
            _areaName = (string)routeDataValues["area"];
            _controllerName = (string)routeDataValues["controller"];
            _actionName = (string)routeDataValues["action"];

            base.InitializePage();
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
