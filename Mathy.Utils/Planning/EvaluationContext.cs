using Cherimoya.Expressions;
using Mathy.Language;
using Mathy.Maths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mathy.Planning
{
    public class EvaluationContext
    {
        public EvaluationContext(Plan plan, Step[] steps)
        {
            Plan = plan;
            Steps = steps;
            foreach (var s in Steps)
            {
                s.Evaluation = this;
            }

            List<string> inVariables = new List<string>();

            foreach (Step step in steps)
            {
                inVariables.AddRange(step.InSourceVariables.Select(i => i.Name));
            }

            InVariables = inVariables.ToArray();
        }


        public Settings Settings { get; set; }

        public Plan Plan { get; private set; }

        public Step[] Steps { get; private set; }

        public string[] InVariables { get; private set; }


        private VariableContext _vc;
        private Dictionary<string, object> variables = new Dictionary<string, object>();


        private Dictionary<string, object> sourceVariables = new Dictionary<string, object>();
        public Dictionary<string, object> SourceVariables
        {
            get { return sourceVariables.ToDictionary(i => i.Key, i => i.Value); }
        }

        public void RemoveSourceVariable(string key)
        {
            sourceVariables.Remove(key);
        }

        public string[] Variables
        {
            get { return variables.Keys.ToArray(); }
        }

        public void SetValue(string variableName, object value)
        {
            if (variables.ContainsKey(variableName))
            {
                variables[variableName] = value;
            }
            else
            {
                variables.Add(variableName, value);
            }
        }

        public object GetValue(string variableName)
        {
            return variables.ContainsKey(variableName) ? variables[variableName] : null;
        }

        public void SetValueString(string variableName, string s)
        {
            SourceVariable v = Plan.Variables.FirstOrDefault(i => i.Name == variableName);
            if (v != null && s != "System.Double[]")
            {
                object value = string.IsNullOrEmpty(s) ? null : DataFuncs.DeserializeValue(v.Type, s);

                SetValueAcrossSteps(variableName, value);
            }
        }

        public void SetValueAcrossSteps(string variableName, object value)
        {
            if (!sourceVariables.ContainsKey(variableName))
            {
                sourceVariables.Add(variableName, value);
            }
            else
            {
                sourceVariables[variableName] = value;
            }


            foreach (Step step in Steps.Where(i => i.InSourceVariables.Any(j => j.Name == variableName)))
            {
                SourceVariable variable = step.InSourceVariables.First(i => i.Name == variableName);
                step.InValues[step.InSourceVariables.ToList().IndexOf(variable)] = value;
            }

            foreach (Step step in Steps.Where(i => i.DependentVariables.Any(j => j.Name == variableName)))
            {
                SourceVariable variable = step.DependentVariables.First(i => i.Name == variableName);
                step.DependentValues[step.DependentVariables.ToList().IndexOf(variable)] = value;
                UpdateStepState(step);
            }
        }

        private void UpdateStepState(Step step)
        {
            if (step.Froms.Length > 0 && step.Froms.All(i => i.State == StepState.Skipped))
            {
                step.State = StepState.Skipped;
            }
            else if (step.DependentValues.All(i => i != null) && step.Froms.All(i => i.State != StepState.Unready))
            {
                step.State = StepState.Ready;
            }
            else
            {
                step.State = StepState.Unready;
            }
        }

        public void DoStep(string stepName, int times)
        {
            var step = Steps.FirstOrDefault(m => m.SourceExpression.Title == stepName);
            if (step.State == StepState.Unready)
            {
                _DoStep.State = StepState.Unready;
                return;
            }
            if (step == null)
                throw new Exception("No such step,please check stepName");
            else
            {
                for (int i = 0; i < times; i++)
                    step.Evaluate(_vc);
            }
            foreach (string key in step.OutVariables)
            {
                SetValueAcrossSteps(key, _vc.GetValue(key));
            }

        }

        private Step _DoStep;
        public void Update()
        {
            VariableContext vc = CreateVariableContext();

            foreach (Step step in Steps)
            {
                UpdateStepState(step);
                _DoStep = step;
                if (step.State == StepState.Ready && step.Conditions.Length > 0)
                {
                    try
                    {
                        if (!step.EvaluateConditions(vc))
                        {
                            step.State = StepState.Skipped;
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException == null)
                        {
                            throw;
                        }
                        else
                        {
                            throw ex.InnerException;
                        }
                    }
                }

                if (step.State == StepState.Ready)
                {
                    try
                    {
                        step.Evaluate(vc);
                        this._vc = vc;
                        foreach (string key in step.OutVariables)
                        {
                            SetValueAcrossSteps(key, _vc.GetValue(key));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException == null)
                        {
                            throw;
                        }
                        else
                        {
                            throw ex.InnerException;
                        }
                    }
                }
            }


            variables.Clear();

            foreach (string variable in vc.GetAllVariables())
            {
                SetValue(variable, vc.GetValue(variable));
            }
        }

        private VariableContext CreateVariableContext()
        {
            VariableContext vc = MathyLanguageService.CreateVariableContext();

            foreach (Step step in Steps)
            {
                for (int i = 0; i <= step.InSourceVariables.Length - 1; i++)
                {
                    object value = step.InValues[i];

                    if (value != null)
                    {
                        if (value is double)
                        {
                            value = Math.Round((double)value, Settings.DecimalDigitCount);
                        }
                        else if (value is double[])
                        {
                            value = (value as double[]).Where(j => !double.IsNaN(j)).Select(j => Math.Round(j, Settings.DecimalDigitCount)).ToArray();
                        }
                        else if (value is int[])
                        {
                            value = (value as int[]).Select(j => Math.Round((double)j, Settings.DecimalDigitCount)).ToArray();
                        }
                        else if (value is Vector)
                        {
                            Vector v = (value as Vector).Clone();
                            v.SetDecimalDigitCount(Settings.DecimalDigitCount);
                            value = v;
                        }
                        else if (value is Matrix)
                        {
                            Matrix m = (value as Matrix).Clone();
                            m.SetDecimalDigitCount(Settings.DecimalDigitCount);
                            value = m;
                        }
                        vc.Set(step.InSourceVariables[i].Name, value);
                    }
                }
            }

            vc.Set("_EvaluationContext", this);
            return vc;
        }


        public void DeleteStep(int stepIndex)
        {
            Step target = Steps[stepIndex];

            Step[] dependencies = Steps.Where(i => i != target && i.Froms.Contains(target)).ToArray();
            if (dependencies.Length > 0)
            {
                StringBuilder b = new StringBuilder();
                foreach (Step step in dependencies)
                {
                    int index = Steps.ToList().IndexOf(step);
                    b.AppendLine(Plan.Expressions[index].Title);
                }

                throw new Exception("Cannot delete specified step.\r\nThe following steps are dependent on it:\r\n" + b);
            }


            List<SourceExpression> expressions = Plan.Expressions.ToList();
            expressions.RemoveAt(stepIndex);
            Plan.Expressions = expressions.ToArray();
        }

        public void Refresh()
        {
            EvaluationContext newContext = new PlanAnalyzer(Plan).Analyze();

            Steps = newContext.Steps;
            InVariables = newContext.InVariables;

            foreach (string key in sourceVariables.Keys)
            {
                newContext.SetValueAcrossSteps(key, sourceVariables[key]);
            }


            Update();
        }

        public void ImportMatrix(string variableName, Stream stream)
        {
            Matrix m = CsvParser.Parse(stream);
            //string text = new StreamReader(stream).ReadToEnd();
            //Matrix m = new CsvParser().Parse(text);
            Style style = Plan.Styles.FirstOrDefault(i => i.Target == variableName);
            if (style != null && style.ColumnNames != null && style.ColumnNames.Length > 0)
            {
                int columnCount = style.ColumnNames.Length;
                if (columnCount > m.ColumnCount)
                {
                    m = m.GetSubMatrix(columnCount);
                }
            }

            SetValueAcrossSteps(variableName, m);
        }
    }
}
