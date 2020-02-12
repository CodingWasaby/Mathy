using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cherimoya.Lexicons
{

    public class Lexiconizer
    {

        private int pos;

        private string text;

        private List<Lexicon> lexicons;

        private LexiconizationErrorProvider errorProvider;


        LexiconizationRule rule;

        public Lexiconizer(LexiconizationRule rule)
        {
            this.rule = rule;
        }


        public Lexicon[] Lexiconize(string s)
        {

            text = s;
            lexicons = new List<Lexicon>();
            errorProvider = new LexiconizationErrorProvider(s);

            while (true)
            {

                SkipWhiteSpaces();
                if (pos == text.Length)
                {
                    break;
                }


                Lexicon lexicon;

                char c = C();

                if (c >= '0' && c <= '9')
                {
                    lexicon = ReadNumber();
                }
                else if (rule.CharEnclosers.Contains(c))
                {
                    lexicon = ReadChar(c);
                }
                else if (rule.StringEnclosers.Contains(c))
                {
                    lexicon = ReadString(c);
                }
                else if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_')
                {
                    lexicon = ReadWord();
                }
                else
                {
                    lexicon = ReadPunctuation();
                }

                lexicons.Add(lexicon);
            }


            return lexicons.ToArray();
        }


        private char C()
        {
            return text[pos];
        }

        private void SkipWhiteSpaces()
        {

            while (pos <= text.Length - 1)
            {
                char c = C();
                if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                {
                    pos++;
                }
                else
                {
                    break;
                }
            }
        }

        private void Skip(char c)
        {
            if (pos <= text.Length - 1 && text[pos] == c)
            {
                pos++;
            }
            else
            {
                errorProvider.ThrowException("Expect " + c, pos);
            }
        }


        private Lexicon ReadNumber()
        {

            int sp = pos;

            bool isNegative = false;

            if (C() == '-')
            {
                isNegative = true;
                pos++;
            }


            bool hasDot = false;
            int intPart = 0;
            int decimalPart = 0;
            int decimalPower = 1;
            int power = 0;
            bool isPowerNegative = false;

            while (pos <= text.Length - 1)
            {

                char c = C();

                if (c == '.')
                {
                    if (hasDot)
                    {
                        break;
                    }
                    else
                    {
                        hasDot = true;
                    }
                }
                else if (c == 'e' || c == 'E')
                {

                    pos++;
                    c = C();

                    if (c == '+')
                    {
                        power = 1;
                        isPowerNegative = true;
                    }
                    else if (c == '-')
                    {
                        power = 1;
                        isPowerNegative = false;
                    }
                    else
                    {
                        pos--;
                        break;
                    }
                }
                else if (c >= 48 && c <= 57)
                {

                    if (power != 0)
                    {
                        power = power * 10 + (c - 48);
                    }
                    else if (hasDot)
                    {
                        decimalPower *= 10;
                        decimalPart = decimalPart * 10 + (c - 48);
                    }
                    else
                    {
                        intPart = intPart * 10 + (c - 48);
                    }
                }
                else
                {
                    break;
                }

                pos++;
            }

            if (!hasDot && power == 0)
            {
                IntegerLexicon lex = new IntegerLexicon(isNegative ? -intPart : intPart);
                lex.SourcePosition = sp;
                lex.Length = pos - sp;
                return lex;
            }
            else
            {

                double value = intPart + (double)decimalPart / decimalPower;

                if (power > 0)
                {
                    if (!isPowerNegative)
                    {
                        value *= power;
                    }
                    else
                    {
                        value /= (double)power;
                    }
                }


                DoubleLexicon lex = new DoubleLexicon(isNegative ? -value : value);
                lex.SourcePosition = sp;
                lex.Length = pos - sp;
                return lex;
            }
        }

        private Lexicon ReadChar(char encloser)
        {

            int sp = pos;

            char result = Convert.ToChar(0);

            pos++;


            char c = C();

            if (c == encloser)
            {
                errorProvider.ThrowException("Char must not be empty.", pos);
            }


            pos++;

            if (c != '\\')
            {
                result = c;
            }
            else
            {

                int[] endPosition = new int[1];
                result = rule.EscapeRule.ReadEscapedChar(text, pos, endPosition, errorProvider);

                pos = endPosition[0];
            }

            Skip(encloser);


            CharLexicon lex = new CharLexicon(result);
            lex.SourcePosition = sp;
            lex.Length = pos - sp;
            return lex;
        }

        private Lexicon ReadString(char encloser)
        {

            int sp = pos;

            StringBuilder b = new StringBuilder();

            pos++;
            while (pos <= text.Length - 1)
            {

                char c = C();
                pos++;

                if (c == encloser)
                {
                    break;
                }
                else if (c != '\\')
                {
                    b.Append(c);
                }
                else
                {

                    int[] endPosition = new int[1];
                    b.Append(rule.EscapeRule.ReadEscapedChar(text, pos, endPosition, errorProvider));

                    pos = endPosition[0];
                }
            }

            StringLexicon lex = new StringLexicon(b.ToString());
            lex.SourcePosition = sp;
            lex.Length = pos - sp;
            return lex;
        }

        private Lexicon ReadWord()
        {

            int sp = pos;

            StringBuilder b = new StringBuilder();

            while (pos <= text.Length - 1)
            {

                char c = C();

                if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_' || c >= '0' && c <= '9')
                {
                    b.Append(c);
                    pos++;
                }
                else
                {
                    break;
                }
            }

            WordLexicon lex = new WordLexicon(b.ToString());
            lex.SourcePosition = sp;
            lex.Length = pos - sp;
            return lex;
        }

        private Lexicon ReadPunctuation()
        {

            int sp = pos;

            string punctuation = rule.Punctuations.Match(text, pos);
            pos += punctuation.Length;

            PunctuationLexicon lex = new PunctuationLexicon(punctuation);
            lex.SourcePosition = sp;
            lex.Length = pos - sp;
            return lex;
        }
    }
}