using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Page
{
    public class PageInfo
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int OffSet
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }

        public int TotalCount { get; set; }
        public int TotalPage
        {
            get
            {
                if (TotalCount == 0)
                    return 1;
                var p = TotalCount / PageSize;
                var l = TotalCount % PageSize;
                return l == 0 ? p : p + 1;
            }
        }

        public string OrderField { get; set; }
        public bool Desc { get; set; } = false;
        public string DescStr
        {
            get
            {
                return Desc ? "DESC" : "";
            }
        }

        public Action<int> Search { get; set; }
    }
}
