using Mathy.Planning;
using System.Linq;

namespace Mathy.Client.Model
{
    public class PlanVM
    {
        public PlanVM(EvaluationContext context)
        {
            Title = context.Plan.Title;
            Description = context.Plan.Description;
            Author = context.Plan.Author;
            Steps = context.Steps.Select((i, index) => new PlanStepListCellVM(i, index)).ToArray();
        }
        public string PlanID { get; set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Author { get; private set; }

        public PlanStepListCellVM[] Steps { get; private set; }
    }
}