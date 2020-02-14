using Mathy.Maths;
using Mathy.Planning;
using Mathy.Utils.Dandelion.Text;
using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Mathy.Client.Model
{
    public class JobStepAreaVM
    {
        public JobStepAreaVM(EvaluationContext context, Step step, int stepIndex, int updateCount)
        {
            Index = stepIndex;
            Title = step.SourceExpression.Title;
            Description = step.SourceExpression.Description;
            ImageUrl = "/Home/GetStepImage?stepIndex=" + stepIndex + "&updateCount=" + updateCount;

            ResultVariables = new ResultVariableVM[step.OutVariables.Length];
            for (int i = 0; i <= step.OutVariables.Length - 1; i++)
            {
                string variableName = step.OutVariables[i];
                object value = context.GetValue(variableName);
                Style style = context.Plan.Styles.FirstOrDefault(j => j.Target == variableName);

                int columnCount = 0;
                string[] rowNames = null;
                string[] columnNames = null;
                string[][] cells = null;
                string displayText = null;
                string url = null;

                if (value == null)
                {
                    displayText = "?";
                }
                else if (value is Matrix || value is Vector)
                {
                    Matrix m = value is Matrix ? value as Matrix : new Matrix(1, (value as Vector).Size, (value as Vector).Items);
                    cells = new string[m.RowCount][];


                    string styleRowName = style == null || string.IsNullOrEmpty(style.RowName) ? null : style.RowName;
                    string[] styleRowNames = styleRowName == null || !styleRowName.Contains(",") ? null : styleRowName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


                    rowNames = new string[m.RowCount];
                    for (int row = 0; row <= m.RowCount - 1; row++)
                    {
                        string rowName;

                        if (styleRowNames != null)
                        {
                            rowName = row <= styleRowNames.Length - 1 ? styleRowNames[row] : string.Format(styleRowName, row + 1);
                        }
                        else
                        {
                            rowName = styleRowName == null ? (row + 1).ToString() : string.Format(style.RowName, row + 1);
                        }


                        rowNames[row] = rowName;
                    }

                    columnCount = m.ColumnCount;
                    columnNames = new string[m.ColumnCount];
                    for (int column = 0; column <= m.ColumnCount - 1; column++)
                    {
                        columnNames[column] = style == null || style.ColumnNames == null || column > style.ColumnNames.Length - 1 ? (column + 1).ToString() : style.ColumnNames[column];
                    }

                    for (int row = 0; row <= m.RowCount - 1; row++)
                    {
                        cells[row] = new string[m.ColumnCount];
                        for (int column = 0; column <= m.ColumnCount - 1; column++)
                        {
                            double d = m[row, column];
                            cells[row][column] = double.IsNaN(d) ? "-" : NumberFormatter.ForDecimalDigits(context.Settings.DecimalDigitCount).ToText(d);
                        }
                    }
                }
                else if (value is Bitmap)
                {
                    url = string.Format("/Home/GetResultImage?stepIndex={0}&variableIndex={1}&timestamp={2}", stepIndex, i, DateTime.Now.ToString());
                }
                else if (value is double)
                {
                    displayText = NumberFormatter.ForDecimalDigits(context.Settings.DecimalDigitCount).ToText((double)value);
                }
                else if (value is int[] || value is double[])
                {
                    Array items = value as Array;

                    StringBuilder b = new StringBuilder();
                    b.Append("[");

                    for (int j = 0; j <= items.Length - 1; j++)
                    {
                        b.Append(NumberFormatter.ForDecimalDigits(context.Settings.DecimalDigitCount).ToText(Convert.ToDouble(items.GetValue(j))));
                        b.Append(j < items.Length - 1 ? "," : "");
                    }

                    b.Append("]");


                    displayText = b.ToString();
                }
                else
                {
                    displayText = value.ToString().Replace("\r\n", "<br />");
                }


                ResultVariables[i] = new ResultVariableVM()
                {
                    Name = step.OutVariables[i],
                    ColumnCount = columnCount,
                    RowNames = rowNames,
                    ColumnNames = columnNames,
                    Cells = cells,
                    Value = displayText,
                    Url = url
                };
            }

            InVariables = new InVariableVM[step.InSourceVariables.Length];
            for (int i = 0; i <= step.InSourceVariables.Length - 1; i++)
            {
                SourceVariable variable = step.InSourceVariables[i];

                InVariables[i] = new InVariableVM()
                {
                    Index = i,
                    Name = variable.Name,
                    Type = Strings.Of(variable),
                    IsMatrix = variable.Type == DataType.Matrix || variable.Type == DataType.Vector,
                    Value = step.InValues[i] == null ? string.Empty : getValue(step.InValues[i], context),
                    Description = step.InSourceVariables[i].Description,
                };
            }
        }

        private string getValue(object value, EvaluationContext e)
        {
            if (value is double[])
            {
                double[] datas = (double[])value;
                datas = (datas as double[]).Where(j => !double.IsNaN(j)).Select(j => Math.Round(j, e.Settings.DecimalDigitCount)).ToArray();
                return string.Join(",", datas);
            }
            return value.ToString();
        }


        public int Index { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsSkipped { get; set; }

        public ResultVariableVM[] ResultVariables { get; set; }

        public InVariableVM[] InVariables { get; set; }
    }

    public class ResultVariableVM
    {
        public string Name { get; set; }

        public int ColumnCount { get; set; }

        public string[] RowNames { get; set; }

        public string[] ColumnNames { get; set; }

        public string[][] Cells { get; set; }

        public string Value { get; set; }

        public string Url { get; set; }
    }

    public class InVariableVM
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsMatrix { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }
    }
}