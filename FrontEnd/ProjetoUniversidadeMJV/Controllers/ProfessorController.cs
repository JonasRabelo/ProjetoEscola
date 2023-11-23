using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Filters;
using ProjetoEscolaMJV.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ProjetoEscolaMJV.Controllers
{
    [PaginaParaProfessorLogado]
    public class ProfessorController : Controller
    {
        private readonly IHttpClientFactory _httpClient;

        public ProfessorController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index(int id)
        {
            ProfessorModel professor = await GetTeacherById(id);
            if (professor == null)
            {
                professor = JsonConvert.DeserializeObject<ProfessorModel>(HttpContext.Session.GetString("sessaoProfessor"));
            }
            return View(professor);


        }

        public IActionResult Criar(int id)
        {
            ViewBag.ProfessorId = id.ToString();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Criar(int professorId, DisciplinaModel disciplina)
        {
            disciplina.Status = true;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.PostAsJsonAsync($"disciplina/create", disciplina);

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Disciplina cadastrada com sucesso";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não foi possível cadastrar a disciplina.";
                }

                return RedirectToAction("ListarDisciplinas", "Professor", new { id = disciplina.ProfessorId });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in ProfessorController.Criar: " + ex.Message);
                return RedirectToAction("Index", "Professor", new { id = disciplina.ProfessorId });
            }
        }


        public async Task<IActionResult> ListarDisciplinas(int id)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{id}");

                if (professor != null)
                {
                    return View(professor);
                }
                else
                {
                    ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                    return RedirectToAction("Error", "Professor");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        public async Task<IActionResult> VerDados(int id)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                ProfessorModel professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{id}");

                if (professor != null)
                {
                    return View(professor);
                }
                else
                {
                    ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                    return RedirectToAction("Error", "Professor");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }
                    

        public async Task<IActionResult> Editar(int id)
        {
            ProfessorModel professor = await GetTeacherById(id);
            if (professor == null)
            {
                ViewData["MensagemErro"] = "Ops, ocorreu um erro, tente novamente";
            }
            ViewBag.Id = professor!.Id.ToString();
            return View(professor);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(ProfessorSemSenhaModel professor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var professorModel = await UpdateTeacher(professor);

                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    var response = await httpClient.PutAsJsonAsync($"professor/update", professorModel);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Dados do professor atualizados com sucesso";
                        return RedirectToAction("Index", "Professor", new { id = professorModel.Id });
                    }

                    TempData["MensagemErro"] = "Ops, não foi possível atualizar os dados (02).";
                    return RedirectToAction("Index", "Professor", new { id = professorModel.Id });
                }

                return View();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in ProfessorController.Criar: " + ex.Message);
                return RedirectToAction("Index", "Professor", new { id = professor.Id });
            }
        }


        public async Task<IActionResult> DetalhesDisciplina(int disciplineId, int teacherId)
        {
            try
            {
                DisciplinaModel disciplina = await GetDiscipline(disciplineId);
                ViewBag.teacherId = teacherId.ToString();
                return View(disciplina);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        public async Task<IActionResult> VerNotas(int disciplineId, int teacherId, string nameTeacher)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                List<MatriculaModel> matriculas = await httpClient.GetFromJsonAsync<List<MatriculaModel>>($"matricula/getallbyiddiscipline/{disciplineId}");

                var disciplina = await GetDiscipline(disciplineId);

                ViewBag.NomeDisciplina = matriculas.FirstOrDefault()?.Disciplina?.Nome;
                ViewBag.status = disciplina?.Status.ToString() ?? "";
                ViewBag.disciplineId = disciplineId.ToString();
                ViewBag.NomeProfessor = nameTeacher;
                ViewBag.teacherId = teacherId.ToString();

                return View(matriculas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        public async Task<IActionResult> LancarNotas(int disciplineId, int teacherId, string nameTeacher, int matriculaId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var matricula = await httpClient.GetFromJsonAsync<MatriculaModel>($"matricula/getbyid/{matriculaId}");

                var disciplina = await GetDiscipline(disciplineId);

                ViewBag.NomeDisciplina = matricula.Disciplina?.Nome;
                ViewBag.disciplineId = disciplineId.ToString();
                ViewBag.NomeProfessor = nameTeacher;
                ViewBag.teacherId = teacherId.ToString();

                return View(matricula);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        [HttpPost]
        public async Task<IActionResult> LancarNotas(int disciplineId, int teacherId, string nameTeacher, MatriculaModel matricula)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    var aluno = await httpClient.GetFromJsonAsync<AlunoModel>($"aluno/getbyid/{matricula.AlunoId}");

                    var response = await httpClient.PutAsJsonAsync($"matricula/update", matricula);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = $"Notas do(a) aluno(a) {aluno.Nome} atualizadas com sucesso";
                        return RedirectToAction("VerNotas", "Professor", new { disciplineId = disciplineId, teacherId = teacherId, nameTeacher = nameTeacher });
                    }

                    TempData["MensagemErro"] = $"Ops, não foi possível atualizar as notas do(a) aluno(a) {aluno.Nome}.";
                    return RedirectToAction("VerNotas", "Professor", new { disciplineId = disciplineId, teacherId = teacherId, nameTeacher = nameTeacher });
                }

                return View();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in ProfessorController.Criar: " + ex.Message);
                return RedirectToAction("VerNotas", "Professor", new { disciplineId = disciplineId, teacherId = teacherId, nameTeacher = nameTeacher });
            }
        }


        public async Task<IActionResult> UpdateStatus(int disciplineId, int teacherId, string nameTeacher)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>($"disciplina/getbyid/{disciplineId}");
                var response = await httpClient.PutAsync($"disciplina/updatestatus?status={!disciplina.Status}&id={disciplineId}", null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = disciplina.Status ? "Disciplina consolidada com sucesso" : "Disciplina reaberta com sucesso";
                    return RedirectToAction("DetalhesDisciplina", "Professor", new { disciplineId = disciplineId, teacherId = teacherId });
                }

                return RedirectToAction("DetalhesDisciplina", "Professor", new { disciplineId = disciplineId, teacherId = teacherId });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in ProfessorController.Criar: " + ex.Message);
                return RedirectToAction("DetalhesDisciplina", "Professor", new { disciplineId = disciplineId, teacherId = teacherId });
            }
        }

 
        private async Task<ProfessorModel> GetTeacherById(int id)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{id}");

                if (response != null)
                {
                    return response;
                }
                else
                {
                    ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        private async Task<ProfessorModel> UpdateTeacher(ProfessorSemSenhaModel professor)
        {
            ProfessorModel professorModel = await GetTeacherById(professor.Id);
            professorModel.Nome = professor.Nome;
            professorModel.Email = professor.Email;
            professorModel.Login = professor.Login;
            return professorModel;
        }


        private async Task<DisciplinaModel> GetDiscipline(int disciplineId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.GetFromJsonAsync<DisciplinaModel>($"disciplina/getbyid/{disciplineId}");

                if (response != null)
                {
                    return response;
                }
                else
                {
                    ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

         
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
