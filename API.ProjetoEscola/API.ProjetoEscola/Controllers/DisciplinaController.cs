using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using Repository.IRepository;

namespace API.ProjetoEscola.Controllers
{
    [Route("api/disciplina")]
    [ApiController]
    public class DisciplinaController : ControllerBase
    {
        private readonly IDisciplinaRepository<DisciplinaModel> _disciplinaRepository;
        private readonly IUsuarioRepository<ProfessorModel> _professorRepository;
        private readonly IMatriculaRepository<MatriculaModel> _matriculaRepository;

        public DisciplinaController(IDisciplinaRepository<DisciplinaModel> disciplinaRepository, IUsuarioRepository<ProfessorModel> professorRepository, IMatriculaRepository<MatriculaModel> matriculaRepository)
        {
            _disciplinaRepository = disciplinaRepository;
            _professorRepository = professorRepository;
            _matriculaRepository = matriculaRepository;
        }


        // Obtém uma disciplina com base no ID.
        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            DisciplinaModel disciplina = _disciplinaRepository.GetById(id);
            disciplina.Professor = _professorRepository.GetById(disciplina.ProfessorId);
            disciplina.Matriculas = _matriculaRepository.GetAllByIdDiscipline(disciplina.Id);
            return Ok(disciplina);
        }


        // Obtém todas as disciplinas no sistema.
        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            List<DisciplinaModel> disciplinas = _disciplinaRepository.GetAll();
            foreach (var disciplina in disciplinas)
            {
                disciplina.Professor = _professorRepository.GetById(disciplina.ProfessorId);
                disciplina.Matriculas = _matriculaRepository.GetAllByIdDiscipline(disciplina.Id);
            }
            return Ok(disciplinas);
        }


        // Obtém todas as disciplinas associadas a um professor com base no ID do professor.
        [HttpGet]
        [Route("getallbyidteacher/{id}")]
        public IActionResult GetAllByIdTeacher(int id)
        {
            List<DisciplinaModel> disciplinas = _disciplinaRepository.GetAllByIdTeacher(id);
            foreach (var disciplina in disciplinas)
            {
                disciplina.Professor = _professorRepository.GetById(disciplina.ProfessorId);
                disciplina.Matriculas = _matriculaRepository.GetAllByIdDiscipline(disciplina.Id);
            }
            return Ok(disciplinas);
        }


        // Obtém todas as disciplinas de uma determinada série.
        [HttpGet]
        [Route("getallbyserie/{serie}")]
        public IActionResult GetAllBySerie(int serie)
        {
            List<DisciplinaModel> disciplinas = _disciplinaRepository.GetAllBySerie(serie);
            foreach (var disciplina in disciplinas)
            {
                disciplina.Professor = _professorRepository.GetById(disciplina.ProfessorId);
                disciplina.Matriculas = _matriculaRepository.GetAllByIdDiscipline(disciplina.Id);
            }
            return Ok(disciplinas);
        }


        // Cria uma nova disciplina.
        [HttpPost]
        [Route("create")]
        public IActionResult Create(DisciplinaModel disciplina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (_disciplinaRepository.Create(disciplina)) return Ok("ok");
                return Ok("");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in DisciplinaController.Create: {ex.Message}");
            }
        }


        // Atualiza os detalhes de uma disciplina existente.
        [HttpPut]
        [Route("update")]
        public IActionResult Update(DisciplinaModel disciplina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _disciplinaRepository.Update(disciplina);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in DisciplinaController.Update: {ex.Message}");
            }
        }


        // Atualiza o status de uma disciplina.
        [HttpPut]
        [Route("updatestatus")]
        public IActionResult UpdateStatus(bool status, int id)
        {
            try
            {
                if (_disciplinaRepository.UpdateStatus(status, id)) return Ok();
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in DisciplinaController.UpdateStatus: {ex.Message}");
            }
        }


        // Exclui uma disciplina com base no ID.
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (_disciplinaRepository.Delete(id)) return Ok();

            return StatusCode(500, $"Error in DisciplinaController.Delete");
        }

        // Exclui todas as disciplinas associadas a um professor com base no ID do professor.
        [HttpDelete]
        [Route("deletebyidteacher/{id}")]
        public IActionResult DeleteByIdTeacher(int id)
        {
            if (_disciplinaRepository.DeleteByIdTeacher(id)) return Ok();

            return StatusCode(500, $"Error in DisciplinaController.DeleteByIdTeacher");
        }
    }
}
