using Mathy.Shared.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Search
{
    public class PlanSearch : BaseSearch
    {
        public int PlanType { get; set; } = -1;
        public string PlanName { get; set; }
        public int AuthFlag { get; set; } = -1;
        public string PlanCategory { get; set; }
        public string Desc { get; set; }
        public string Author { get; set; }

    }
}
