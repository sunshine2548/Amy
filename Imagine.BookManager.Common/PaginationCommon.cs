using System;
using System.Configuration;
using System.Linq;

namespace Imagine.BookManager.Common
{
    public class PaginationCommon
    {
        private static readonly int SingletonPageCount;

        static PaginationCommon()
        {
            int.TryParse(ConfigurationManager.AppSettings["SingletonPageCount"], out var singletonPageCount);
            if (singletonPageCount <= 0)
                throw new Exception(
                    " not found\"SingletonPageCount\" in appsetting or  \"SingletonPageCount\" type must int");
            SingletonPageCount = singletonPageCount;
        }

        public static int GetPageIndex(int? pageIndex)
        {
            if (pageIndex.HasValue == false)
                return 1;
            if (pageIndex.Value < 1)
                return 1;
            return pageIndex.Value;
        }

        public static int GetSingletonPageCount(int? singletonPageCount)
        {
            if (singletonPageCount.HasValue == false)
                return SingletonPageCount;
            if (singletonPageCount.Value < 0)
                return SingletonPageCount;
            return singletonPageCount.Value;
        }
    }

    public static class QueryableExtend
    {
        public static PaginationDataList<T> ToPagination<T>(
            this IQueryable<T> queryable,
            int? pageIndex,
            int? singletonPageCount) where T : new()
        {
            var index = PaginationCommon.GetPageIndex(pageIndex);
            var pageCount = PaginationCommon.GetSingletonPageCount(singletonPageCount);
            var result = new PaginationDataList<T>
            {
                TotalPages = (int)Math.Ceiling(queryable.Count() * 1.0 / pageCount)
            };
            if (result.TotalPages == 0)
                return result;
            var list = queryable
                .Skip((index - 1) * pageCount)
                .Take(pageCount);
            result.CurrentPage = index;
            result.ListData = list.ToList();
            return result;
        }
    }
}