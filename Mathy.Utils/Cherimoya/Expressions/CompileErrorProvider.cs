using Cherimoya.Lexicons;
using System;
using System.Text;

namespace Cherimoya.Expressions
{
    public class CompileErrorProvider
    {

        //    private List<int> starts = new List<int>();

        private string source;

        private Lexicon[] lexicons;


        public CompileErrorProvider(string source, Lexicon[] lexicons, bool performTypeChecking)
        {
            this.source = source;
            this.lexicons = lexicons;
            PerformTypeChecking = performTypeChecking;
        }

        public bool PerformTypeChecking { get; private set; }


        internal void ThrowException(string message, int from, int to)
        {
            int offset = Math.Max(0, from - 20);

            string source = this.source.Substring(offset, Math.Min(this.source.Length, to + 20) - offset);


            StringBuilder tildes = new StringBuilder();
            for (int i = 1; i <= from - offset; i++)
            {
                tildes.Append(".");
            }

            if (to == from)
            {
                tildes.Append("^");
            }
            else
            {
                for (int i = 1; i <= to - from; i++)
                {
                    tildes.Append("~");
                }
            }

            throw new CompileException(string.Format("\r\n{0}\r\n{1}\r\n{2}", source, tildes.ToString(), message));
        }

        public void ThrowException(string message, Expression expression)
        {
            ThrowException(
                    message,
                    lexicons[expression.FromPosition].SourcePosition,
                    lexicons[expression.ToPosition].SourcePosition + lexicons[expression.ToPosition].Length);
        }

        public void ThrowException(string message, Expression fromExpression, Expression toExpression)
        {
            ThrowException(
                    message,
                    lexicons[fromExpression.FromPosition].SourcePosition,
                    lexicons[toExpression.ToPosition].SourcePosition + lexicons[toExpression.ToPosition].Length);
        }

        /*
        internal void ThrowException(string message)
        {
            int from = starts[starts.Count - 1];

            int offset = Math.Max(0, from - 20);

            string source = this.source.Substring(offset, Math.Min(this.source.Length, from + 20) - offset);


            StringBuilder arrow = new StringBuilder();
            for (int i = 1; i <= from - offset; i++)
            {
                arrow.Append(" ");
            }

            arrow.Append("^");


            throw new CompileException(string.Format("\r\n{0}\r\n{1}\r\n{2}", source, arrow.ToString(), message));
        }*/
    }
}