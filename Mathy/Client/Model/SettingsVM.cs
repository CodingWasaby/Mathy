using Mathy.Planning;

namespace Mathy.Client.Model
{
    public class SettingsVM
    {
        public SettingsVM(Settings settings)
        {
            DecimalDigitCount = settings.DecimalDigitCount;
        }


        public int DecimalDigitCount { get; set; }
    }
}