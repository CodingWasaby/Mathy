using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared.Entity
{
    public class Plan
    {
        public int AutoID { get; set; }

        public string ID { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public DateTime CreateTime { get; set; }

        public int ReferenceCount { get; set; }

        public int PlanType { get; set; }

        public string PlanCategory { get; set; }

        public int AuthFlag { get; set; }

        public int DeleteFlag { get; set; }

        public string PlanRepository { get; set; }
    }
}
