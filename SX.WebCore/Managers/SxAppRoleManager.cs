﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace SX.WebCore.Managers
{
    public class SxAppRoleManager : RoleManager<SxAppRole>
    {
        public SxAppRoleManager(RoleStore<SxAppRole> store)
                : base(store)
        { }

        public static SxAppRoleManager Create(IdentityFactoryOptions<SxAppRoleManager> options, IOwinContext context)
        {
            return new SxAppRoleManager(new RoleStore<SxAppRole>(context.Get<SxDbContext>()));
        }
    }
}
