using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using Abp.Dependency;
using System.Reflection;

namespace Imagine.BookManager.Web
{
    public class MvcApplication : AbpWebApplication<BookManagerWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                            f => f.UseAbpLog4Net().WithConfig(Server.MapPath("log4net.config"))
                        );
            base.Application_Start(sender, e);
        }


    }
}
