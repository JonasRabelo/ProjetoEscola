using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using Repository.IRepository;
using System.ComponentModel;

namespace API.ProjetoEscola.Controllers
{
    [Route("api/matricula")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaRepository<MatriculaModel> _matriculaRepository;
        private readonly IUsuarioRepository<AlunoModel> _alunoRepository;
        private readonly IDisciplinaRepository<DisciplinaModel> _disciplinaRepository;

        public MatriculaController(IMatriculaRepository<MatriculaModel> matriculaRepository, IUsuarioRepository<AlunoModel> alunoRepository, IDisciplinaRepository<DisciplinaModel> disciplinaRepository)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _disciplinaRepository = disciplinaRepository;
        }


        // Obtém uma matrícula pelo ID, incluindo detalhes do aluno e disciplina associados.
        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            MatriculaModel matricula = _matriculaRepository.GetById(id);
            matricula.Aluno = _alunoRepository.GetById(matricula.AlunoId);
            matricula.Disciplina = _disciplinaRepository.GetById(matricula.DisciplinaId);
            return Ok(matricula);
        }


        // Obtém todas as matrículas, incluindo detalhes do aluno e disciplina associados.
        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            List<MatriculaModel> matriculas = _matriculaRepository.GetAll();
            foreach (var matricula in matriculas)
            {
                matricula.Aluno = _alunoRepository.GetById(matricula.AlunoId);
                matricula.Disciplina = _disciplinaRepository.GetById(matricula.DisciplinaId);
            }
            return Ok(matriculas);
        }


        // Obtém todas as matrículas de um aluno específico, incluindo detalhes do aluno e disciplina associados.
        [HttpGet]
        [Route("getallbyidstudent/{id}")]
        public IActionResult GetAllByIdStudent(int id)
        {
            List<MatriculaModel> matriculas = _matriculaRepository.GetAllByIdStudent(id);
            foreach (var matricula in matriculas)
            {
                matricula.Aluno = _alunoRepository.GetById(matricula.AlunoId);
                matricula.Disciplina = _disciplinaRepository.GetById(matricula.DisciplinaId);
            }
            return Ok(matriculas);
        }


        // Obtém todas as matrículas de uma disciplina específica, incluindo detalhes do aluno e disciplina associados.
        [HttpGet]
        [Route("getallbyiddiscipline/{id}")]
        public IActionResult GetAllByIdDiscipline(int id)
        {
            List<MatriculaModel> matriculas = _matriculaRepository.GetAllByIdDiscipline(id);
            foreach (var matricula in matriculas)
            {
                matricula.Aluno = _alunoRepository.GetById(matricula.AlunoId);
                matricula.Disciplina = _disciplinaRepository.GetById(matricula.DisciplinaId);
            }
            return Ok(matriculas);
        }


        // Cria uma nova matrícula.
        [HttpPost]
        [Route("create")]
        public IActionResult Create(MatriculaModel matricula)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if(_matriculaRepository.Create(matricula)) return Ok();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in MatriculaController.Create: {ex.Message}");
            }
        }


        // Atualiza uma matrícula existente.
        [HttpPut]
        [Route("update")]
        public IActionResult Update(MatriculaModel matricula)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _matriculaRepository.Update(matricula);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in MatriculaController.Update: {ex.Message}");
            }
        }


        // Exclui uma matrícula pelo ID.
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (_matriculaRepository.Delete(id)) return Ok();

            return StatusCode(500, $"Error in MatriculaController.Delete: ErroInUpdate");
        }


        // Exclui todas as matrículas de um aluno pelo ID do aluno.
        [HttpDelete]
        [Route("deletebyidstudent/{id}")]
        public IActionResult DeleteByIdStudent(int id)
        {
            if (_matriculaRepository.DeleteByIdStudent(id)) return Ok();

            return StatusCode(500, $"Error in MatriculaController.DeleteByIdStudent");
        }


        // Exclui todas as matrículas de uma disciplina pelo ID da disciplina.
        [HttpDelete]
        [Route("deletebyiddiscipline/{id}")]
        public IActionResult DeleteByIdDiscipline(int id)
        {
            if (_matriculaRepository.DeleteByIdDiscipline(id)) return Ok();

            return StatusCode(500, $"Error in MatriculaController.DeleteByIdDisciple");
        }
    }
}
