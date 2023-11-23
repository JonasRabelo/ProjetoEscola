using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.IRepository;

namespace API.ProjetoEscola.Controllers
{
    [Route("api/aluno")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly IUsuarioRepository<AlunoModel> _alunoRepository;
        private readonly IMatriculaRepository<MatriculaModel> _matriculaRepository;
  
        public AlunoController(IUsuarioRepository<AlunoModel> modelRepository, IMatriculaRepository<MatriculaModel> matriculaRepository)
        {
            _alunoRepository = modelRepository;
            _matriculaRepository = matriculaRepository;
        }


        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            AlunoModel aluno = _alunoRepository.GetById(id);
            aluno.Matriculas = _matriculaRepository.GetAllByIdStudent(aluno.Id);
            return Ok(aluno);
        }


        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            List<AlunoModel> alunos = _alunoRepository.GetAll();
            foreach (var aluno in alunos)
            {
                aluno.Matriculas = _matriculaRepository.GetAllByIdStudent(aluno.Id);
            }
            return Ok(alunos);
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create(AlunoModel aluno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (_alunoRepository.Create(aluno)) return Ok();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in AlunoController.Create: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("update")]
        public IActionResult Update(AlunoModel aluno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _alunoRepository.Update(aluno);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in AlunoController.Update: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (_alunoRepository.Delete(id)) return Ok();

            return StatusCode(500, $"Error in AlunoController.Delete: ErroInDelete");
        }
    }
}
