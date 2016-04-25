namespace Fanex.Maintenance.Core.Helpers
{
    using System.Collections.Generic;
    using Webdiyer.WebControls.Mvc;

    public class PagerList<T> : List<T>, IPagedList
    {
        public PagerList(int pageIndex, int pageSize, int totalItemCount, IEnumerable<T> items)
        {
            this.CurrentPageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            this.AddRange(items);
        }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }
    }
}
