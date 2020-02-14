using Mathy.Shared.Entity;
using Petunia;

namespace Mathy.Client.Model
{
    public class EditJobVM
    {
        public EditJobVM(Job lm)
        {
            AutoID = lm.AutoID;
            PlanAutoID = lm.PlanAutoID;
            Name = lm.Name;
            PlanTitle = lm.PlanTitle;
        }


        public int AutoID { get; set; }

        public int PlanAutoID { get; set; }

        public string Name { get; set; }

        public string PlanTitle { get; set; }
    }
}