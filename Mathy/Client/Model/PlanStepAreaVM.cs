using Mathy.Planning;

namespace Mathy.Client.Model
{
    public class PlanStepAreaVM
    {
        public PlanStepAreaVM(EvaluationContext context, Step step, int stepIndex, int updateCount)
        {
            Index = stepIndex;
            Title = step.SourceExpression.Title;
            Description = step.SourceExpression.Description;
            ImageUrl = "/Home/GetStepImage?stepIndex=" + stepIndex + "&updateCount=" + updateCount;
        }


        public int Index { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}