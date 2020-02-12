using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mathy.Shared.Page;
using Mathy.Repository.Repo;
using Mathy.Shared;
using Mathy.Shared.Entity;
using Mathy.Shared.Search;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mathy.Server.Controllers
{
    [ApiController]
    [Route("Plan/")]
    public class PlanController : BaseController
    {
        private readonly PlanRepo _PlanRepo;
        public PlanController(PlanRepo planRepo)
        {
            _PlanRepo = planRepo;
        }

        [HttpPost("List")]
        public ResponseResult<PageList<Plan>> GetPlanList([FromBody]PlanSearch search)
        {
            return ToResponse(() =>
            {
                return _PlanRepo.GetPlans(search);
            });
        }

        [HttpGet("{planID}")]
        public ResponseResult<Plan> GetPlan(int planID)
        {
            return ToResponse(() =>
            {
                return _PlanRepo.GetPlan(planID);
            });
        }
    }
}
