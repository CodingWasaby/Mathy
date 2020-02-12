using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mathy.Repository.Repo;
using Mathy.Shared;
using Mathy.Shared.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mathy.Server.Controllers
{
    [ApiController]
    [Route("User/")]
    public class UserController : BaseController
    {
        private readonly UserRepo _UserRepo;
        public UserController(UserRepo userRepo)
        {
            _UserRepo = userRepo;
        }

        [HttpPost("{email}/{pass}")]
        public ResponseResult<User> Login(string email, string pass)
        {
            return ToResponse(() =>
            {
                return _UserRepo.GetUser(email, pass);
            });
        }
    }
}
