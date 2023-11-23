using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using Repository.IRepository;

namespace API.ProjetoEscola.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository<LoginModel> _loginRepository;
        private readonly IMatriculaRepository<MatriculaModel> _matriculaRepository;
        private readonly IDisciplinaRepository<DisciplinaModel> _disciplinaRepository;

        public LoginController(ILoginRepository<LoginModel> loginRepository, IMatriculaRepository<MatriculaModel> matriculaRepository, IDisciplinaRepository<DisciplinaModel> disciplinaRepository)
        {
            _loginRepository = loginRepository;
            _matriculaRepository = matriculaRepository;
            _disciplinaRepository = disciplinaRepository;
        }

        [HttpGet]
        [Route("getuserbylogin")]
        public IActionResult GetUserByLogin([FromQuery] LoginModel loginModel)
        {
            if (loginModel.Tipo == "Aluno")
            {
                AlunoModel aluno = _loginRepository.GetStudentByLogin(loginModel);
                aluno.Matriculas = _matriculaRepository.GetAllByIdStudent(aluno.Id);
                return Ok(aluno);
            }
            ProfessorModel professor = _loginRepository.GetTeacherByLogin(loginModel);
            professor.Disciplinas = _disciplinaRepository.GetAllByIdTeacher(professor.Id);
            return Ok(professor);
        }

        [HttpPost]
        [Route("updatepassword")]
        public IActionResult UpdatePassword([FromQuery] LoginModel loginModel)
        {
            if (loginModel.Tipo == "Aluno") loginModel.Id = _loginRepository.GetStudentByLogin(loginModel).Id;
            else loginModel.Id = _loginRepository.GetTeacherByLogin(loginModel).Id;

            if (_loginRepository.UpdatePassword(loginModel)) return Ok();
            return BadRequest();
        }

    }
}
