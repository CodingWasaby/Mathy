using Mathy.Shared.Page;
using Mathy.Shared.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mathy.Client.Model
{
    public class CommonSearch
    {
        public CommonSearch()
        {
            BaseS = new BaseSearch
            {
                BeginDate = DateTime.Now.Date.AddMonths(-6),
                EndDate = DateTime.Now.Date
            };
        }
        public BaseSearch BaseS { get; set; }

        public bool DatePart { get; set; } = true;

        public bool PagePart { get; set; } = true;

        public bool CustomButton { get; set; } = true;

        public List<SearchParam> SearchParams { get; set; } = new List<SearchParam>();
    }

    public class SearchParam
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
