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
        
    }
}
