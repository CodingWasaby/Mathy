using Cherimoya.Diff;
using Cherimoya.Expressions;
using Mathy.Utils.Dandelion.Serialization;
using Mathy.Language;
using Mathy.Maths;
using Mathy.Visualization.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mathy
{
    public class MathyShell
    {
        public static void Init(string rootPath)
        {
            MathyContext.Init(rootPath);
        }


        public static Expression Diff(Expression e, string x, Dictionary<string, object> variables)
        {
            VariableContext c = Funcs.CreateVariableContext(variables);
            Expression diff = new Differ(e, x).Calculate(c);

            foreach (string variableName in variables.Keys)
            {
                c.Set(variableName, variables[variableName]);
            }

            new MathyLanguageService().CreateTypeChecker().PerformTypeChecking(diff, new MathyLanguageService().CreateTypeCheckingContext(c));
            diff = new Cherimoya.Reduction.ExpressionReductor(new MathyLanguageService()).Reduce(diff);

            foreach (string variableName in variables.Keys)
            {
                c.Remove(variableName);
            }


            return diff;
        }

        public static void Parse(string s, out string expression, out Dictionary<string, object> variables)
        {
            IDictionary dict = new JsonDeserializer().DeserializeString(s) as IDictionary;

            expression = dict["expression"] as string;
            IDictionary variableDict = dict["variables"] as IDictionary;


            variables = new Dictionary<string, object>();

            foreach (string key in variableDict.Keys.Cast<string>())
            {
                string t = (variableDict[key] as string).Trim();


                object value = null;

                int intValue;
                if (int.TryParse(t, out intValue))
                {
                    value = intValue;
                }

                if (value == null)
                {
                    double doubleValue;
                    if (double.TryParse(t, out doubleValue))
                    {
                        value = doubleValue;
                    }
                }

                if (value == null)
                {
                    if (t.StartsWith("["))
                    {
                        value = Matrix.Parse(t);
                    }
                }

                if (value != null)
                {
                    variables.Add(key, value);
                }
            }
        }

        public static Expression[] Compile(string expressions, Dictionary<string, object> variables)
        {
            VariableContext c = Funcs.CreateVariableContext(variables);

            return new MathyLanguageService().Compile(expressions, c);
        }

        public static object Evaluate(Expression[] expressions, Dictionary<string, object> variables)
        {
            VariableContext c = Funcs.CreateVariableContext(variables);

            object[] result =
                expressions
                .Select(i => new Mathy.Language.MathyLanguageService().CreateEvaluator().Evaluate(i, c))
                .ToArray();


            variables.Clear();
            foreach (string variableName in c.GetAllVariables())
            {
                variables.Add(variableName, c.GetValue(variableName));
            }


            return result.Length == 1 ? result[0] : result;
        }

        public static object Evaluate(string expressions, Dictionary<string, object> variables)
        {
            VariableContext c = Funcs.CreateVariableContext(variables);
            return Evaluate(new MathyLanguageService().Compile(expressions, c), variables);
        }

        public static Bitmap Visualize(Expression[] expressions, Dictionary<string, object> variables)
        {
            return new NodeVisualizer(
                expressions
                .Select(i => new NodeConverter().Convert(i))
                .ToArray()).VisulizeAsBitmap();
        }

        public static Bitmap Visualize(string expressions, Dictionary<string, object> variables)
        {
            VariableContext c = Funcs.CreateVariableContext(variables);

            return new NodeVisualizer(
                new MathyLanguageService().Compile(expressions, c)
                .Select(i => new NodeConverter().Convert(i))
                .ToArray()).VisulizeAsBitmap();
        }
    }
}
