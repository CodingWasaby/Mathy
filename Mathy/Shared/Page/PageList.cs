using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Page
{
    public class PageList<T>
    {
        public PageInfo PageInfo { get; set; }
        public List<T> PageData { get; set; }
    }
}
