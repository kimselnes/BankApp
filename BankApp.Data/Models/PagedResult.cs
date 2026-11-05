using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.Models
{
    public class PagedResult<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public int PageCount => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPrevious => PageNo > 1;
        public bool HasNext => PageNo < PageCount;
    }
}
