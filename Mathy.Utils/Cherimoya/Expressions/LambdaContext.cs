using System.Collections.Generic;

namespace Cherimoya.Expressions
{
    public class LambdaContext
    {
        private List<LambdaVariable> variables = new List<LambdaVariable>();

        private List<Expression> callerMethodStack = new List<Expression>();

        public void PushCallerMethod(Expression method)
        {
            callerMethodStack.Add(method);
        }

        public void PopCallerMethod()
        {
            callerMethodStack.RemoveAt(callerMethodStack.Count - 1);
        }

        public int GetCallerStackSize()
        {
            return callerMethodStack.Count;
        }

        public void PushVariables(string[] names)
        {

            int index = 0;
            foreach (string name in names)
            {

                LambdaVariable v = new LambdaVariable();
                v.Name = name;
                v.CallerMethod = callerMethodStack[callerMethodStack.Count - 1];
                v.Index = index;
                variables.Add(v);

                index++;
            }
        }

        public void PopVariables(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                variables.RemoveAt(variables.Count - 1);
            }
        }

        public LambdaVariable GetVariable(string name)
        {
            foreach (LambdaVariable variable in variables)
            {
                if (variable.Name == name)
                {
                    return variable;
                }
            }


            return null;
        }
    }
}