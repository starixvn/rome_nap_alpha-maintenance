namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;

    public class SuperLimitDateViewModel
    {
        public SuperLimitDateViewModel(IEnumerable<SuperLimitDate> supers
            , int totalRecords
            , int pageIndex
            , int pageSize)
        {
            Supers = supers;
            TotalRecords = totalRecords;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public IEnumerable<SuperLimitDate> Supers { get; set; }

        public int TotalRecords { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
