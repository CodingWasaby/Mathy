namespace Mathy.Client.Model
{
    public class MCMVM
    {
        public int MCMID { get; set; }
        public int JobID { get; set; }
        public string Title { get; set; }
        public int MCMType { get; set; }
        public int RunNum { get; set; }
        public int DecimalDigitCount { get; set; }
        public string GS { get; set; }
        public Dis[] Diss { get; set; }
        public double[] Result { get; set; }

        public double p { get; set; }
        public int n_dig { get; set; }
    }

    public class Dis
    {
        public string Name { get; set; }

        public string DisType { get; set; }

        public double[] DisParams { get; set; }
    }
}