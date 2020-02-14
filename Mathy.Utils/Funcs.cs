using Cherimoya.Expressions;
using Mathy.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Mathy
{
    public class Funcs
    {
        static Funcs()
        {
            InitGreekLetters();
        }

        public static byte[] ImageToBytes(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);

            byte[] data = stream.GetBuffer().Take((int)stream.Length).ToArray();

            stream.Close();

            return data;
        }

        public static VariableContext CreateVariableContext(Dictionary<string, object> variables)
        {
            VariableContext c = MathyLanguageService.CreateVariableContext();

            foreach (string variableName in variables.Keys)
            {
                c.Set(variableName, variables[variableName]);
            }


            return c;
        }

        internal static double Atan(double x1, double y1, double x2, double y2)
        {
            if (x1 == x2)
            {
                return y2 >= y1 ? -Math.PI / 2 : Math.PI / 2;
            }
            else if (y1 == y2)
            {
                return x2 >= x1 ? 0 : Math.PI;
            }


            double angle = Math.Atan2(Math.Abs(y2 - y1), Math.Abs(x2 - x1));

            if (y2 > y1 && x2 > x1)
            {
                return -angle;
            }
            else if (y2 > y1 && x2 < x1)
            {
                return Math.PI + angle;
            }
            else if (y2 < y1 && x2 < x1)
            {
                return Math.PI - angle;
            }
            else if (y2 < y1 && x2 > x1)
            {
                return angle;
            }


            return 0;
        }


        private static Dictionary<string, char> greekLetters = new Dictionary<string, char>();
        public static string GetGreekLetter(string name)
        {
            if (!greekLetters.ContainsKey(name))
            {
                return null;
            }
            else
            {
                return new string(new char[] { greekLetters[name] });
            }
        }

        public static string[] GetGreekLetters()
        {
            return greekLetters.Keys.ToArray();
        }

        private static void InitGreekLetters()
        {
            greekLetters.Add("Alpha", (char)0x0391);
            greekLetters.Add("Beta", (char)0x0392);
            greekLetters.Add("Gamma", (char)0x0393);
            greekLetters.Add("Delta", (char)0x0394);
            greekLetters.Add("Epsilon", (char)0x0395);
            greekLetters.Add("Zeta", (char)0x0396);
            greekLetters.Add("Eta", (char)0x0397);
            greekLetters.Add("Theta", (char)0x0398);
            greekLetters.Add("Iota", (char)0x0399);
            greekLetters.Add("Kappa", (char)0x039a);
            greekLetters.Add("Lambda", (char)0x039b);
            greekLetters.Add("Mu", (char)0x039c);
            greekLetters.Add("Nu", (char)0x039d);
            greekLetters.Add("Xi", (char)0x039e);
            greekLetters.Add("Omicron", (char)0x039f);
            greekLetters.Add("Pi", (char)0x03a0);
            greekLetters.Add("Rho", (char)0x03a1);
            greekLetters.Add("Sigma", (char)0x03a3);
            greekLetters.Add("Tao", (char)0x03a4);
            greekLetters.Add("Upsilon", (char)0x03a5);
            greekLetters.Add("Phi", (char)0x03a6);
            greekLetters.Add("Chi", (char)0x03a7);
            greekLetters.Add("Psi", (char)0x03a6);
            greekLetters.Add("Omega", (char)0x03a7);

            greekLetters.Add("alpha", (char)0x03b1);
            greekLetters.Add("beta", (char)0x03b2);
            greekLetters.Add("gamma", (char)0x03b3);
            greekLetters.Add("delta", (char)0x03b4);
            greekLetters.Add("epsilon", (char)0x03b5);
            greekLetters.Add("zeta", (char)0x03b6);
            greekLetters.Add("eta", (char)0x03b7);
            greekLetters.Add("theta", (char)0x03b8);
            greekLetters.Add("iota", (char)0x03b9);
            greekLetters.Add("kappa", (char)0x03ba);
            greekLetters.Add("lambda", (char)0x03bb);
            greekLetters.Add("mu", (char)0x03bc);
            greekLetters.Add("nu", (char)0x03bd);
            greekLetters.Add("xi", (char)0x03be);
            greekLetters.Add("omicron", (char)0x03bf);
            greekLetters.Add("pi", (char)0x03c0);
            greekLetters.Add("rho", (char)0x03c1);
            greekLetters.Add("sigma", (char)0x03c3);
            greekLetters.Add("tao", (char)0x03c4);
            greekLetters.Add("upsilon", (char)0x03c5);
            greekLetters.Add("phi", (char)0x03c6);
            greekLetters.Add("chi", (char)0x03c7);
            greekLetters.Add("psi", (char)0x03c8);
            greekLetters.Add("omega", (char)0x03c9);
        }
    }
}
