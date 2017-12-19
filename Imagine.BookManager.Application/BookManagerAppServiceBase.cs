using Abp.Application.Services;

namespace Imagine.BookManager
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class BookManagerAppServiceBase : ApplicationService
    {
        protected BookManagerAppServiceBase()
        {
            LocalizationSourceName = BookManagerConsts.LocalizationSourceName;
        }
    }
}