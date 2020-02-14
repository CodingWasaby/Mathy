

using Mathy.Shared.Entity;

namespace Mathy.Client.Model
{
    public class PlanListCellVM
    {
        public PlanListCellVM(Plan lm, int pageIndex, string author = "")
        {
            CreateJobUrl = string.Format("/Home/CreateJob?planAutoID={0}", lm.AutoID);
            EditPlanUrl = string.Format("/Home/EditPlan?planID={0}", lm.ID);
            DeletePlanUrl = string.Format("javascript:deletePlan({0},{1});", lm.AutoID, pageIndex);
            ViewPlanUrl = string.Format("/Home/ViewPlan?planID={0}", lm.AutoID);
            DownloadUrl = string.Format("/Home/DownloadPlan?planID={0}", lm.ID);
            UpdatePlanUrl = string.Format("/Home/UploadPlan?planID={0}", lm.ID);
            AuthPlanUrl = string.Format("/Home/AuthPlan?planID={0}", lm.ID);
            Title = lm.Title;
            Author = lm.Author;
            Description = lm.Description;
            CreateTime = lm.CreateTime.ToShortDateString();
            ReferenceCount = lm.ReferenceCount;
            PlanType = GetPlanType(lm.PlanType);
            IsCustom = string.IsNullOrEmpty(author);
            AuthFlag = lm.AuthFlag;
            PlanCategory = "类型" + lm.PlanCategory;
        }


        public string CreateJobUrl { get; private set; }

        public string EditPlanUrl { get; private set; }

        public string DeletePlanUrl { get; private set; }

        public string ViewPlanUrl { get; private set; }

        public string DownloadUrl { get; private set; }

        public string UpdatePlanUrl { get; private set; }

        public string AuthPlanUrl { get; private set; }

        public string Title { get; private set; }

        public string Author { get; private set; }

        public string Description { get; private set; }

        public string CreateTime { get; private set; }

        public string PlanType { get; set; }
        public string PlanCategory { get; set; }

        public int ReferenceCount { get; private set; }

        public bool IsCustom { get; set; }

        public int AuthFlag { get; set; }

        public string AuthFlag_S
        {
            get
            {
                switch (AuthFlag)
                {
                    case 0: return "（待审）";
                    case 1: return "（已审）";
                    case -1: return "（驳回）";
                    default: return "";
                }
            }
        }

        private string GetPlanType(int planType)
        {
            //var p = (PlanTypeEnum)planType;
            //switch (p)
            //{
            //    case PlanTypeEnum.Public: return "公共";
            //    case PlanTypeEnum.Protect: return "保护";
            //    case PlanTypeEnum.Private: return "私有";
            //    default:
            //        return "公共";
            //}
            return "公共";
        }
    }
}