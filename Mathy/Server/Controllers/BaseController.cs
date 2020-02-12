using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mathy.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mathy.Server.Controllers
{
    public class BaseController : Controller
    {
        // GET: /<controller>/
        public ResponseResult<T> ToResponse<T>(Func<T> logic)
        {
            ResponseResult<T> rr;
            try
            {
                rr = new ResponseResult<T>(0, "success", logic.Invoke());
            }
            catch (Exception ex)
            {
                rr = new ResponseResult<T>(500, ex.Message, default);
            }
            return rr;
        }
    }
}
