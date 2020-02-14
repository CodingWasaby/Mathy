using Mathy.Planning;

namespace Mathy.Libs
{
    public class CustomFuncFactory
    {
        public static int Loop(string steps, int times, object ec)
        {
            EvaluationContext e = ec as EvaluationContext;
            e.DoStep(steps, times);

            return times;
        }
    }
}
