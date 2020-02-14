namespace Mathy.Planning
{
    public class Plan
    {
        public int AutoID { get; set; }

        public string ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public int PlanType { get; set; }

        public string PlanCategory { get; set; }

        public SourceExpression[] Expressions { get; set; }

        public SourceVariable[] Variables { get; set; }

        public Style[] Styles { get; set; }


        public static Plan Parse(string s)
        {
            return PlanParser.Parse(s);
        }

        public EvaluationContext CreateEvaluationContext()
        {
            return new PlanAnalyzer(this).Analyze();
        }
    }

    public class SourceExpression
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Expression { get; set; }

        public string Condition { get; set; }
    }

    public class SourceVariable
    {
        public string Name { get; set; }

        public DataType Type { get; set; }

        public string Description { get; set; }
    }

    public class Style
    {
        public string Target { get; set; }

        public int Size { get; set; }

        public string[] ColumnNames { get; set; }

        public int RowHeaderWidth { get; set; }

        public string RowName { get; set; }
    }
}
