using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.IRepository;

namespace API.ProjetoEscola.Controllers
{
    [Route("api/professor")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IUsuarioRepository<ProfessorModel> _professorRepository;
        private readonly IDisciplinaRepository<DisciplinaModel> _disciplinaRepository;

        public ProfessorController(IUsuarioRepository<ProfessorModel> modelRepository, IDisciplinaRepository<DisciplinaModel> disciplinaRepository)
        {
            _professorRepository = modelRepository;
            _disciplinaRepository = disciplinaRepository;
        }

        // Busca um professor no banco de dados com base no ID.
        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            ProfessorModel professor = _professorRepository.GetById(id);
            professor.Disciplinas = _disciplinaRepository.GetAllByIdTeacher(professor.Id);
            return Ok(professor);
        }


        // Busca todas os alunos do sistema.
        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            List<ProfessorModel> professores = _professorRepository.GetAll();
            foreach (var professor in professores)
            {
                professor.Disciplinas = _disciplinaRepository.GetAllByIdTeacher(professor.Id);
            }
            return Ok(professores);
        }


        // Cria um novo professor no sistema.
        [HttpPost]
        [Route("create")]
        public IActionResult Create(ProfessorModel professor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (_professorRepository.Create(professor)) return Ok();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in ProfessorController.Create: {ex.Message}");
            }
        }


        // Atualiza os detalhes de um professor existente.
        [HttpPut]
        [Route("update")]
        public IActionResult Update(ProfessorModel professor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _professorRepository.Update(professor);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in ProfessorController.Update: {ex.Message}");
            }
        }


        // Exclui um professor com base no ID.
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (_professorRepository.Delete(id)) return Ok(true);

            return StatusCode(500, $"Error in ProfessorController.Delete: ErroInUpdate");
        }
    }
}
