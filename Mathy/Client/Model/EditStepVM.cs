
using Mathy.Planning;

namespace Mathy.Client.Model
{
    public class EditStepVM
    {
        public EditStepVM(SourceExpression expression)
        {
            Title = expression.Title;
            Description = expression.Description;
            Expression = expression.Expression;
            Condition = expression.Condition;
        }


        public string Title { get; set; }

        public string Description { get; set; }

        public string Expression { get; set; }

        public string Condition { get; set; }
    }
}