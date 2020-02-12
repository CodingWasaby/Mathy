using Mathy.Shared.Entity;
using Mathy.Shared.Page;
using Mathy.Shared.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Repository.Repo
{
    public class PlanRepo : BaseRepo
    {
        public PlanRepo(DbBase dbBase) : base(dbBase)
        {

        }

        public PageList<Plan> GetPlans(PlanSearch search)
        {
            search.Page.OrderField = "p.CreateTime";
            string sql = @" SELECT p.[AutoID],
                                                   p.[ID],
                                                   p.[Title],
                                                   p.[Author],
                                                   p.[Description],
                                                   p.[CreateTime],
                                                   p.[PlanType],
                                                   p.[DeleteFlag],
                                                   p.[AuthFlag],
                                                   p.[PlanCategory],
                                                   COUNT(jp.JobPlanID) ReferenceCount
                                            FROM [Cherimoya].[dbo].[PlanDB] p (NOLOCK)
                                                LEFT JOIN dbo.JobPlanDB jp (NOLOCK)
                                                    ON p.AutoID = jp.AutoID
                                                       AND jp.DeleteFlag = 0
                                            WHERE p.DeleteFlag = 0 {0}
                                            GROUP BY p.[AutoID],
                                                     p.[ID],
                                                     p.[Title],
                                                     p.[Author],
                                                     p.[Description],
                                                     p.[CreateTime],
                                                     p.[PlanType],
                                                     p.[DeleteFlag],
                                                     p.[AuthFlag],
                                                     p.[PlanCategory] ";
            var param = "";
            if (search.BeginDate.HasValue)
            {
                param += " AND p.CreateTime >= @BeginDate ";
            }
            if (search.EndDate.HasValue)
            {
                param += " AND p.CreateTime <= @EndDate ";
            }
            if (search.PlanType >= 0)
            {
                param += " AND p.PlanType = @PlanType ";
            }
            if (search.AuthFlag >= 0)
            {
                param += " AND p.AuthFlag = @PlanType ";
            }
            if (!string.IsNullOrEmpty(search.PlanCategory))
            {
                param += " AND p.PlanCategory = @PlanCategory ";
            }
            if (!string.IsNullOrEmpty(search.Author))
            {
                param += " AND p.Author = @Author ";
            }
            if (!string.IsNullOrEmpty(search.PlanName))
            {
                param += " AND p.Title Like '%" + search.PlanName + "%' ";
            }
            if (!string.IsNullOrEmpty(search.Desc))
            {
                param += " AND p.Description  Like '%" + search.Desc + "%' ";
            }
            return QueryPage<Plan>(string.Format(sql, param), search, search.Page);
        }

        public Plan GetPlan(int planID)
        {
            string sql = @"SELECT p.[AutoID],
                                                   p.[ID],
                                                   p.[Title],
                                                   p.[Author],
                                                   p.[Description],
                                                   p.[CreateTime],
                                                   p.[PlanType],
                                                   p.[DeleteFlag],
                                                   p.[AuthFlag],
                                                   p.[PlanCategory],
                                                   p.[PlanRepository]
                                                   COUNT(jp.JobPlanID) ReferenceCount
                                            FROM [Cherimoya].[dbo].[PlanDB] p (NOLOCK)
                                                LEFT JOIN dbo.JobPlanDB jp (NOLOCK)
                                                    ON p.AutoID = jp.AutoID
                                                       AND jp.DeleteFlag = 0
                                            WHERE p.DeleteFlag = 0
                                                  AND p.AutoID = @PlanID ";
            return QueryFirst<Plan>(sql, new { PlanID = planID });
        }

        public int AddPlan(Plan plan)
        {
            string sql = @"INSERT INTO dbo.PlanDB
                                    (
                                        ID,
                                        Title,
                                        Author,
                                        Description,
                                        CreateTime,
                                        ReferenceCount,
                                        PlanType,
                                        DeleteFlag,
                                        AuthFlag,
                                        PlanCategory,
                                        PlanRepository
                                    )
                                    VALUES
                                    (@ID, @Title, @Author, @Description, @CreateTime, @ReferenceCount, @PlanType, @DeleteFlag, @AuthFlag, @PlanCategory,
                                     @PlanRepository ) SELECT @@IDENTITY ";
            return ExcuteScalar(sql, plan);
        }

        public bool UpDatePlan(Plan plan)
        {
            string sql = @" UPDATE dbo.PlanDB
                                    SET Title = @Title,
                                        Description = @Description,
                                        PlanType = @PlanType,
                                        PlanCategory = @PlanCategory,
                                        PlanRepository = @PlanRepository
	                                    WHERE AutoID = @AutoID ";
            return Excute(sql, plan);
        }

        public bool DeletePlan(int planID)
        {
            string sql = @" UPDATE dbo.PlanDB
                                    SET DeleteFlag = 1
                                    WHERE AutoID = @PlanID ; ";
            return Excute(sql, new { PlanID = planID });
        }

        public bool UpdatePlanAuthFlag(string planID, int authFlag)
        {
            string sql = @"
                            UPDATE dbo.PlanDB SET AuthFlag =@AuthFlag
                            WHERE ID = @PlanID ";
            return Excute(sql, new { AuthFlag = authFlag, PlanID = planID });
        }
    }
}
