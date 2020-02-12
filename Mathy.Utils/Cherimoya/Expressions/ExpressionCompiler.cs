using Cherimoya.Lexicons;
using System;
using System.Collections.Generic;

namespace Cherimoya.Expressions
{
    public abstract class ExpressionCompiler
    {
        private Lexicon[] lexicons;

        private int savedPos;

        private string source;


        public Expression[] Compile(string s, Lexicon[] lexicons, CompileErrorProvider errorProvider)
        {
            source = s;
            this.lexicons = lexicons;
            this.errorProvider = errorProvider;


            return Compile();
        }


        private CompileErrorProvider errorProvider;


        protected int Pos { get; set; }

        protected Lexicon Current
        {
            get { return lexicons[Pos]; }
        }

        protected Lexicon Next
        {
            get { return Pos == lexicons.Length - 1 ? null : lexicons[Pos + 1]; }
        }

        protected bool End
        {
            get { return Pos == lexicons.Length; }
        }

        protected void Move()
        {
            Pos++;
        }

        protected void Save()
        {
            savedPos = Pos;
        }

        protected void GoBack()
        {
            Pos = savedPos;
        }

        protected bool IsConstant
        {
            get
            {
                Type type = Current.GetType();
                return type == typeof(IntegerLexicon)
                        || type == typeof(DoubleLexicon)
                        || type == typeof(StringLexicon)
                        || type == typeof(CharLexicon);
            }
        }

        protected object GetConstant()
        {
            Lexicon lexicon = Current;

            Type type = Current.GetType();

            if (type == typeof(IntegerLexicon))
            {
                return ((IntegerLexicon)lexicon).Value;
            }
            else if (type == typeof(DoubleLexicon))
            {
                return ((DoubleLexicon)lexicon).Value;
            }
            else if (type == typeof(StringLexicon))
            {
                return ((StringLexicon)lexicon).Value;
            }
            else if (type == typeof(CharLexicon))
            {
                return ((CharLexicon)lexicon).Value;
            }

            return null;
        }

        protected bool IsPunctuation
        {
            get { return !End && Current.GetType() == typeof(PunctuationLexicon); }
        }

        protected string GetPunctuation()
        {
            return (End || !(Current is PunctuationLexicon)) ? null : ((PunctuationLexicon)Current).Value;
        }

        protected bool IsPunctuationOf(string p)
        {
            return IsPunctuation && GetPunctuation() == p;
        }

        protected bool IsWord
        {
            get { return End ? false : Current.GetType() == typeof(WordLexicon); }
        }

        protected string GetWord()
        {
            return ((WordLexicon)Current).Text;
        }

        protected void ThrowExpects(string expects)
        {
            ThrowException(string.Format("Expects: {0}", expects));
        }

        protected void ThrowException(string message)
        {
            if (End)
            {
                errorProvider.ThrowException(message, source.Length, source.Length);
            }
            else
            {
                errorProvider.ThrowException(message, Current.SourcePosition, Current.SourcePosition + Current.Length);
            }
        }

        public bool PerformTypeChecking { get; set; }

        protected void SkipWord(string word)
        {
            if (IsWord && GetWord() == word)
            {
                Move();
            }
            else
            {
                ThrowExpects(word);
            }
        }

        protected void SkipPunctuation(string punctuation)
        {
            if (IsPunctuation && GetPunctuation() == punctuation)
            {
                Move();
            }
            else
            {
                ThrowExpects(punctuation);
            }
        }


        private List<int> positions = new List<int>();

        protected void PushPosition()
        {
            positions.Add(Pos);
        }

        protected void PopPosition()
        {
            positions.Remove(positions.Count - 1);
        }

        protected int PeekPos()
        {
            return positions[positions.Count - 1];
        }


        internal abstract LexiconizationRule GetLexiconizationRule();

        public abstract bool CreateVariableOnAssing();

        protected abstract Expression[] Compile();
    }
}