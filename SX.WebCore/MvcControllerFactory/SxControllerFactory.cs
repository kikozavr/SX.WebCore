using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SX.WebCore.MvcControllerFactory
{
    public class SxControllerFactory<TDbContext> : DefaultControllerFactory
        where TDbContext: SxDbContext
    {
        private static Dictionary<string, Type> _coreControllerCache;
        private static readonly string _coreNamespase = "SX.WebCore.MvcControllers";
        static SxControllerFactory()
        {
            _coreControllerCache = new Dictionary<string, Type>();
            var coreControllers = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && !t.IsAbstract && t.Name.Contains("Controller") && t.Namespace == _coreNamespase)
                       .ToList();
            if (coreControllers.Any())
                coreControllers.ForEach(x => {
                    _coreControllerCache.Add(x.Name.Replace("Sx",null).Replace("Controller",null).Replace("`",null).Replace("1",null).Replace("2", null).ToLower(), x);
                });
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = null;
            try
            {
                controller = base.CreateController(requestContext, controllerName);
            }
            catch { }

            var cName = controllerName.ToLower();
            if (controller==null && _coreControllerCache.ContainsKey(cName))
            {
                var cType = _coreControllerCache[cName];
                Type[] typeArgs = { typeof(TDbContext) };
                controller=(IController)Activator.CreateInstance(cType.MakeGenericType(typeArgs));
            }

            return controller;
        }
    }
}
