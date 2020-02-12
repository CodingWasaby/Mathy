using System;

namespace Cherimoya.Expressions
{
    public class ExpressionSnippets
    {

        public static int GetOperatorLevel(BinaryOperator op)
        {

            if (op == BinaryOperator.None)
            {
                return 0;
            }
            else if (op == BinaryOperator.Assign)
            {
                return 1;
            }
            else if (op == BinaryOperator.Or || op == BinaryOperator.Xor)
            {
                return 2;
            }
            else if (op == BinaryOperator.And)
            {
                return 3;
            }
            else if (op == BinaryOperator.LessThan
                    || op == BinaryOperator.LessEqualThan
                    || op == BinaryOperator.GreaterThan
                    || op == BinaryOperator.GreaterEqualThan
                    || op == BinaryOperator.Equal
                    || op == BinaryOperator.NotEqual)
            {
                return 4;
            }
            else if (op == BinaryOperator.Add
                    || op == BinaryOperator.Subtract)
            {
                return 5;
            }
            else if (op == BinaryOperator.Multiply
                    || op == BinaryOperator.Divide)
            {
                return 6;
            }


            throw new Exception();
        }

        public static Associavity GetAssociavity(BinaryOperator op)
        {

            if (op == BinaryOperator.Add || op == BinaryOperator.Multiply)
            {
                return Associavity.Left;
            }
            else if (op == BinaryOperator.Subtract || op == BinaryOperator.Divide)
            {
                return Associavity.Right;
            }
            else
            {
                return Associavity.None;
            }
        }



        public static UnaryOperator GetUnaryOperator(string p)
        {

            UnaryOperator op = UnaryOperator.None;

            if (p == "+")
            {
                op = UnaryOperator.Identity;
            }
            else if (p == "-")
            {
                op = UnaryOperator.Negation;
            }

            return op;
        }

        public static BinaryOperator GetBinaryOperator(string p)
        {

            BinaryOperator op = BinaryOperator.None;

            if (p == "+")
            {
                op = BinaryOperator.Add;
            }
            else if (p == "-")
            {
                op = BinaryOperator.Subtract;
            }
            else if (p == "*")
            {
                op = BinaryOperator.Multiply;
            }
            else if (p == "/")
            {
                op = BinaryOperator.Divide;
            }
            else if (p == "&&")
            {
                op = BinaryOperator.And;
            }
            else if (p == "||")
            {
                op = BinaryOperator.Or;
            }
            else if (p == "<")
            {
                op = BinaryOperator.LessThan;
            }
            else if (p == "<=")
            {
                op = BinaryOperator.LessEqualThan;
            }
            else if (p == ">")
            {
                op = BinaryOperator.GreaterThan;
            }
            else if (p == ">=")
            {
                op = BinaryOperator.GreaterEqualThan;
            }
            else if (p == "==")
            {
                op = BinaryOperator.Equal;
            }
            else if (p == "!=")
            {
                op = BinaryOperator.NotEqual;
            }
            else if (p == "=")
            {
                op = BinaryOperator.Assign;
            }


            return op;
        }

        public static string SerializeBinaryOperator(BinaryOperator op)
        {

            string s = "";

            if (op == BinaryOperator.Add)
            {
                s = "+";
            }
            else if (op == BinaryOperator.Subtract)
            {
                s = "-";
            }
            else if (op == BinaryOperator.Multiply)
            {
                s = "*";
            }
            else if (op == BinaryOperator.Divide)
            {
                s = "/";
            }
            else if (op == BinaryOperator.And)
            {
                s = "&&";
            }
            else if (op == BinaryOperator.Or)
            {
                s = "||";
            }
            else if (op == BinaryOperator.LessThan)
            {
                s = "<";
            }
            else if (op == BinaryOperator.LessEqualThan)
            {
                s = "<=";
            }
            else if (op == BinaryOperator.GreaterThan)
            {
                s = ">";
            }
            else if (op == BinaryOperator.GreaterEqualThan)
            {
                s = ">=";
            }
            else if (op == BinaryOperator.Equal)
            {
                s = "==";
            }
            else if (op == BinaryOperator.NotEqual)
            {
                s = "!=";
            }
            else if (op == BinaryOperator.Assign)
            {
                s = "=";
            }

            return s;
        }

        public static object SerializeUnaryOperator(UnaryOperator op)
        {

            string s = "";

            if (op == UnaryOperator.Negation)
            {
                s = "-";
            }

            return s;
        }
    }
}