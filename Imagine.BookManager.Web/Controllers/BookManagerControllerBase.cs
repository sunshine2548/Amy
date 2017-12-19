using Abp.Web.Mvc.Controllers;

namespace Imagine.BookManager.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class BookManagerControllerBase : AbpController
    {
        protected BookManagerControllerBase()
        {
            LocalizationSourceName = BookManagerConsts.LocalizationSourceName;
        }
    }
}