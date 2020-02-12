using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Entity
{
    public class Job 
    {
        public int AutoID { get; set; }
        public string PlanID { get; set; }
        public int UserAutoID { get; set; }
        public int PlanTitle { get; set; }
        public int IsComplete { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Variables { get; set; }
        public int Name { get; set; }
        public int PlanAutoID { get; set; }
        public int DecimalCount { get; set; }
        public int JobType { get; set; }
        public int DeleteFlag { get; set; }
    }
}
