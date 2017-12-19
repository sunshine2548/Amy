using Abp.Web.Mvc.Views;

namespace Imagine.BookManager.Web.Views
{
    public abstract class BookManagerWebViewPageBase : BookManagerWebViewPageBase<dynamic>
    {

    }

    public abstract class BookManagerWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected BookManagerWebViewPageBase()
        {
            LocalizationSourceName = BookManagerConsts.LocalizationSourceName;
        }
    }
}