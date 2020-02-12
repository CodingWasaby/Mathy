using Mathy.Shared.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Search
{
    public abstract class BaseSearch
    {
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PageInfo Page { get; set; }
    }
}
