using Mathy.Shared.Entity;
using Mathy.Shared.Page;
using Mathy.Shared.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Repository.Repo
{
    public class JobRepo : BaseRepo
    {
        public JobRepo(DbBase dbBase) : base(dbBase)
        {

        }

        public PageList<Job> GetJobs(JobSearch search)
        {
            search.Page.OrderField = "j.CreateTime";
            string sql = @" SELECT j.AutoID,
                                       j.PlanID,
                                       j.UserAutoID,
                                       j.PlanTitle,
                                       j.IsComplete,
                                       j.CreateTime,
                                       j.UpdateTime,
                                       j.Variables,
                                       j.Name,
                                       j.PlanAutoID,
                                       j.DecimalCount,
                                       j.JobType,
                                       j.DeleteFlag
                                FROM dbo.JobDB j (NOLOCK)
                                WHERE j.DeleteFlag= 0 ";
            var param = "";
            if (search.BeginDate.HasValue)
            {
                param += " AND j.CreateTime >= @BeginDate ";
            }
            if (search.EndDate.HasValue)
            {
                param += " AND j.CreateTime <= @EndDate ";
            }
            if (search.IsComplete >= 0)
            {
                param += " AND j.IsComplete = @IsComplete ";
            }
            if (!string.IsNullOrEmpty(search.Author))
            {
                param += " AND j.UserAutoID = @Author ";
            }
            if (!string.IsNullOrEmpty(search.PlanName))
            {
                param += " AND j.PlanTitle Like '%" + search.PlanName + "%' ";
            }
            if (!string.IsNullOrEmpty(search.JobName))
            {
                param += " AND j.Name  Like '%" + search.JobName + "%' ";
            }
            return QueryPage<Job>(sql, search, search.Page);
        }

        public Job GetJob(int jobID)
        {
            string sql = @" SELECT j.AutoID,
                                   j.PlanID,
                                   j.UserAutoID,
                                   j.PlanTitle,
                                   j.IsComplete,
                                   j.CreateTime,
                                   j.UpdateTime,
                                   j.Variables,
                                   j.Name,
                                   j.PlanAutoID,
                                   j.DecimalCount,
                                   j.JobType,
                                   j.DeleteFlag
                            FROM dbo.JobDB j (NOLOCK)
                            WHERE j.DeleteFlag= 0
                            AND j.AutoID = @JobID ";
            return QueryFirst<Job>(sql, new { JobID = jobID });
        }

        public int AddJob(Job job)
        {
            string sql = @" INSERT INTO dbo.JobDB
                                        (
                                            PlanID,
                                            UserAutoID,
                                            PlanTitle,
                                            IsComplete,
                                            CreateTime,
                                            UpdateTime,
                                            Variables,
                                            Name,
                                            PlanAutoID,
                                            DecimalCount,
                                            JobType,
                                            DeleteFlag
                                        )
                                        VALUES
                                        (@PlanID, @UserAutoID, @PlanTitle, @IsComplete, @CreateTime, @UpdateTime, @Variables, @Name, @PlanAutoID,
                                         @DecimalCount, @JobType, @DeleteFlag); ";
            return ExcuteScalar(sql, job);
        }

        public bool UpDateJob(Job job)
        {
            string sql = @" UPDATE dbo.JobDB
                                    SET IsComplete = @IsComplete,
                                        Name = @Name,
                                        DecimalCount = @DecimalCount,
                                        JobType = @JobType,
                                        Variables = @Variables,
                                        UpdateTime = GETDATE()
                                    WHERE AutoID = @JobID; ";
            return Excute(sql, job) ;
        }

        public bool DeleteJob(int jobID)
        {
            string sql = @" UPDATE dbo.JobDB
                                SET DeleteFlag = 1
                                WHERE AutoID = @JobID;  ";
            return Excute(sql, new { JobID = jobID });
        }

        public int AddJobPlan(JobPlan jobPlan)
        {
            string sql = @" INSERT INTO dbo.JobPlanDB
                                    (
                                        AutoID,
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
                                    (@AutoID, @ID, @Title, @Author, @Description, @CreateTime, @ReferenceCount, @PlanType, @DeleteFlag, @AuthFlag,
                                     @PlanCategory,  @PlanRepository) SELECT @@IDENTITY ";
            return ExcuteScalar(sql, jobPlan);
        }

        public JobPlan GetJobPlan(string jobPlanID)
        {
            string sql = @" SELECT * FROM  [dbo].[JobPlanDB] (nolock) where JobPlanID = @JobPlanID ";
            return QueryFirst<JobPlan>(sql, new { JobPlanID = jobPlanID });
        }

        public int CopyJobPlan(string oldID)
        {
            string sql = @" INSERT INTO dbo.JobPlanDB
                                    (
                                        AutoID,
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
                                    SELECT AutoID,
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
                                    FROM dbo.JobPlanDB
                                    WHERE JobPlanID = @JobPlanID;  ";
            return ExcuteScalar(sql, new { JobPlanID = oldID });
        }
    }
}
