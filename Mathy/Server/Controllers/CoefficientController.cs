using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mathy.Repository.Repo;
using Mathy.Shared;
using Mathy.Shared.Entity;
using Mathy.Shared.Page;
using Mathy.Shared.Search;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mathy.Server.Controllers
{
    [ApiController]
    [Route("Coefficient/")]
    public class CoefficientController : BaseController
    {
        private readonly CoefficientRepo coefficientRepo;
        public CoefficientController(CoefficientRepo coefficientRepo)
        {
            this.coefficientRepo = coefficientRepo;
        }

        [HttpPost("List")]
        public ResponseResult<PageList<Coefficient>> Index([FromBody]CoefficientSearch search)
        {
            return ToResponse(() =>
             {
                 return coefficientRepo.GetCoefficients(search.Page, search.Name);
             });
        }

        [HttpPost("Details/{coefficientID}")]
        public ResponseResult<List<CoefficientDetail>> GetDetails(int coefficientID)
        {
            return ToResponse(() =>
            {
                return coefficientRepo.GetCoefficientDetail(coefficientID);
            });
        }
    }
}
