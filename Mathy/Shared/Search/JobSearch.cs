using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Search
{
    public class JobSearch : BaseSearch
    {
        public string JobName { get; set; }
        public string PlanName { get; set; }
        public int IsComplete { get; set; } = -1;
        public string Author { get; set; }
    }
}
