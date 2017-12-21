using System.Collections.Generic;

namespace Imagine.BookManager.Common
{
    public class PaginationDataList<T> where T : new()
    {
        public int TotalPages { get; set; }
        public List<T> ListData { get; set; }
        public int CurrentPage { get; set; }

        public PaginationDataList()
        {
            ListData = new List<T>();
        }
    }
}
