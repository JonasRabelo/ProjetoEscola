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


        // Obtém um usuário (aluno ou professor) com base nas informações de login fornecidas.
        [HttpGet]
        [Route("getuserbylogin")]
        public IActionResult GetUserByLogin([FromQuery] LoginModel loginModel)
        {
            if (loginModel.Tipo == "Aluno")
            {
                // Se o tipo for "Aluno", obtém um objeto AlunoModel com base no login.
                AlunoModel aluno = _loginRepository.GetStudentByLogin(loginModel);
                aluno.Matriculas = _matriculaRepository.GetAllByIdStudent(aluno.Id);
                return Ok(aluno);
            }
            // Se o tipo não for "Aluno", presume-se que seja "Professor".
            // Obtém um objeto ProfessorModel com base no login.
            ProfessorModel professor = _loginRepository.GetTeacherByLogin(loginModel);
            professor.Disciplinas = _disciplinaRepository.GetAllByIdTeacher(professor.Id);
            return Ok(professor);
        }
    }
}
