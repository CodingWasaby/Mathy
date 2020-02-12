using System;

namespace Cherimoya.Lexicons
{
    public class LexiconizationRuleFactory
    {
        class JavaEscapeRule : EscapeRule
        {
            public override char ReadEscapedChar(string s, int startPosition, int[] endPosition, LexiconizationErrorProvider errorProvider)
            {
                char result = Convert.ToChar(200);
                char c = s[startPosition];

                if (c == '0')
                {
                    result = Convert.ToChar(0);
                }
                else if (c == '\'')
                {
                    result = '\'';
                }
                else if (c == '\"')
                {
                    result = '\"';
                }
                else if (c == '\\')
                {
                    result = '\\';
                }
                else if (c == 'n')
                {
                    result = '\n';
                }
                else if (c == 'r')
                {
                    result = '\r';
                }
                else if (c == 'v')
                {
                    result = Convert.ToChar(11);
                }
                else if (c == 't')
                {
                    result = '\t';
                }
                else if (c == 'b')
                {
                    result = '\b';
                }
                else if (c == 'f')
                {
                    result = '\f';
                }

                if (result != 200)
                {
                    endPosition[0] = startPosition + 1;
                    return result;
                }


                if (c == 'u')
                {
                    endPosition[0] = startPosition + 5;
                    int d1 = hexCharToInt(s[startPosition + 1], startPosition + 1, errorProvider);
                    int d2 = hexCharToInt(s[startPosition + 2], startPosition + 2, errorProvider);
                    int d3 = hexCharToInt(s[startPosition + 3], startPosition + 3, errorProvider);
                    int d4 = hexCharToInt(s[startPosition + 4], startPosition + 4, errorProvider);
                    return (char)(d4 + 16 * d3 + 16 * 16 * d2 + 16 * 16 * 16 * d1);
                }
                else
                {
                    errorProvider.ThrowException("Unregonized escape char.", startPosition);
                    return Convert.ToChar(0);
                }
            }
        }


        class JavascriptEscapeRule : EscapeRule
        {
            public override char ReadEscapedChar(string s, int startPosition, int[] endPosition, LexiconizationErrorProvider errorProvider)
            {
                char result = Convert.ToChar(200);
                char c = s[startPosition];

                if (c == '0')
                {
                    result = Convert.ToChar(0);
                }
                else if (c == '\'')
                {
                    result = '\'';
                }
                else if (c == '\"')
                {
                    result = '\"';
                }
                else if (c == '\\')
                {
                    result = '\\';
                }
                else if (c == 'n')
                {
                    result = '\n';
                }
                else if (c == 'r')
                {
                    result = '\r';
                }
                else if (c == 'v')
                {
                    result = Convert.ToChar(11);
                }
                else if (c == 't')
                {
                    result = '\t';
                }
                else if (c == 'b')
                {
                    result = '\b';
                }
                else if (c == 'f')
                {
                    result = '\f';
                }

                if (result != 200)
                {
                    endPosition[0] = startPosition + 1;
                    return result;
                }


                if (c == 'x')
                {
                    endPosition[0] = startPosition + 3;
                    int d1 = hexCharToInt(s[startPosition + 1], startPosition + 1, errorProvider);
                    int d2 = hexCharToInt(s[startPosition + 2], startPosition + 2, errorProvider);
                    return (char)(d2 + 16 * d1);
                }
                else if (c == 'u')
                {
                    endPosition[0] = startPosition + 5;
                    int d1 = hexCharToInt(s[startPosition + 1], startPosition + 1, errorProvider);
                    int d2 = hexCharToInt(s[startPosition + 2], startPosition + 2, errorProvider);
                    int d3 = hexCharToInt(s[startPosition + 3], startPosition + 3, errorProvider);
                    int d4 = hexCharToInt(s[startPosition + 4], startPosition + 4, errorProvider);
                    return (char)(d4 + 16 * d3 + 16 * 16 * d2 + 16 * 16 * 16 * d1);
                }
                else
                {
                    errorProvider.ThrowException("Unregonized escape char.", startPosition);
                    return Convert.ToChar(0);
                }
            }
        }


        private static int hexCharToInt(char c, int pos, LexiconizationErrorProvider errorProvider)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            else if (c >= 'a' && c <= 'f')
            {
                return c - 'a';
            }
            else if (c >= 'A' && c <= 'F')
            {
                return c - 'A';
            }
            else
            {
                errorProvider.ThrowException("Expect hex char.", pos);
                return 0;
            }
        }


        public static LexiconizationRule CreateRuleForJava()
        {

            return new LexiconizationRule()
            {
                Punctuations = new Punctuations(new string[]
                {
                    "!=", "<=", ">=", "==", "++", "--", "->", "&&", "||", "+=", "-=", "*=", "/=", "&=", "|=", "~=", "..."
                }
                ),
                StringEnclosers = new char[] { '"' },
                CharEnclosers = new char[] { '\'' },
                EscapeRule = new JavaEscapeRule()
            };
        }

        /**
         * Escape chars for JavaScript is referenced from:
         * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_objects/string?redirectlocale=en-US&redirectslug=JavaScript%2FReference%2FGlobal_objects%2Fstring
         * @return lexiconization rule for JavaScript.
         */
        public static LexiconizationRule createRuleForJavascript()
        {

            return new LexiconizationRule()
            {
                Punctuations = new Punctuations(new string[]
                {
                    "!=", "<=", ">=", "==", "++", "--", "->", "&&", "||", "+=", "-=", "*=", "/=", "&=", "|=", "~=", "..."
                }
                ),
                StringEnclosers = new char[] { '\'', '"' },
                CharEnclosers = new char[] { },
                EscapeRule = new JavascriptEscapeRule()
            };
        }
    }
}