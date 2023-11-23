using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using Repository.IRepository;

namespace API.ProjetoEscola.Controllers
{
    [Route("api/superuser")]
    [ApiController]
    public class SuperUserController : ControllerBase
    {
        private readonly ISARepository<SuperUserModel> _saRepository;

        public SuperUserController(ISARepository<SuperUserModel> sARepository)
        {
            _saRepository = sARepository;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetById([FromQuery] SuperUserModel superUser)
        {
             return Ok(_saRepository.Get(superUser.Login, superUser.Senha));
        }

    }
}
