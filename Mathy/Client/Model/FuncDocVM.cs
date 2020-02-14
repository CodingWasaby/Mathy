using Petunia;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Client.Model
{
    public class DocVM
    {
        public FuncDocVM[] List { get; set; }


        public DocVM(FuncDoc[] funcs)
        {
            List<FuncDoc> list = funcs.ToList();
            List = funcs.OrderBy(i => i.Name).Select(i => new FuncDocVM(i, list.IndexOf(i))).ToArray();
        }
    }

    public class FuncDocVM
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ParameterDocVM[] Parameters { get; set; }

        public ReturnDocVM[] Returns { get; set; }

        public FuncArticleVM Article { get; set; }


        public FuncDocVM(FuncDoc lm, int index)
        {
            Index = index;
            Name = lm.Name;
            Title = lm.Title;
            Description = lm.Description;
            Parameters = lm.Parameters.Select(i => new ParameterDocVM()
            {
                Name = i.Name,
                Type = string.Join(", ", i.Type.Select(j => MathyDic.GetDataTypeDisplayName(j))),
                Description = i.Description
            }).ToArray();
            Returns = lm.Returns.Select(i => new ReturnDocVM()
            {
                Type = string.Join(", ", i.Type.Select(j => MathyDic.GetDataTypeDisplayName(j))),
                Description = i.Description
            }).ToArray();
            Article = new FuncArticleVM(lm.Article);
        }
    }

    public class ParameterDocVM
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }

    public class ReturnDocVM
    {
        public string Type { get; set; }

        public string Description { get; set; }
    }
}