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


        //Método que retorna a view da tela inicial do professor
        public async Task<IActionResult> Index(int id)
        {   //Chamado de um método auxiliar que solicita os dados do professor para a API
            ProfessorModel professor = await GetTeacherById(id);
            if (professor.Id == 0)
            {
                //Se houver algum erro na solicitação, pega os dados guardados na sessaoProfessor
                professor = JsonConvert.DeserializeObject<ProfessorModel>(HttpContext.Session.GetString("sessaoProfessor"));
            }
            return View(professor);


        }


        //Método que retornar a view com um formulário para cadastro de uma nova disciplina
        public IActionResult Criar(int id)
        {
            ViewBag.ProfessorId = id.ToString();
            return View();
        }


        //Método que recebe os dados do formulário de cadastro de uma disciplina e processa a requisição
        [HttpPost]
        public async Task<IActionResult> Criar(int professorId, DisciplinaModel disciplina)
        {
            disciplina.Status = true; //Define o status da disciplina como true -> Em Andamento

            try
            {
                if (!ModelState.IsValid) //Se a model não for válida, retorna para a view do formulário
                {
                    return View();
                }
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Model válida -> Solicitação para a API cadastrar uma nova disciplina no banco de dados
                var response = await httpClient.PostAsJsonAsync($"disciplina/create", disciplina);

                if (response.IsSuccessStatusCode) //Verifica o status da solicitação de cadastro da disciplina
                {
                    TempData["MensagemSucesso"] = "Disciplina cadastrada com sucesso";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não foi possível cadastrar a disciplina.";
                }
                //Retorna para a view que lista todas as disciplinas do professor
                return RedirectToAction("ListarDisciplinas", "Professor", new { id = disciplina.ProfessorId });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in ProfessorController.Criar: " + ex.Message);
                return RedirectToAction("Index", "Professor", new { id = disciplina.ProfessorId });
            }
        }


        //Método que retornar uma view que lista todas as disciplinas do professor
        public async Task<IActionResult> ListarDisciplinas(int id)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados do professor para a API com base no Id
                var professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{id}");

                if (professor != null)
                {
                    return View(professor); //Retorna a view com a lista de disciplinas do professor, caso dê certo a solicitação
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


        //Método que retorna uma view que exibe os dados do professor e a opção de solicitar a edição dos dados
        public async Task<IActionResult> VerDados(int id)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados do professor para a API com base no Id
                ProfessorModel professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{id}");

                if (professor != null)
                {
                    return View(professor); //Retorna a view com os dados do professor, caso dê certo a solicitação
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
                    

        //Método que retorna uma view que contém um formulário para edição dos dados do professor
        public async Task<IActionResult> Editar(int id)
        {
            ProfessorModel professor = await GetTeacherById(id); //Chamado de um método auxiliar que solicita os dados do professor para a API
            if (professor == null)
            {
                ViewData["MensagemErro"] = "Ops, ocorreu um erro, tente novamente";
            }
            ViewBag.Id = professor!.Id.ToString();
            return View(professor);
        }


        //Método que recebe os dados do formulário de edição das informações do professor e faz o processamento
        [HttpPost]
        public async Task<IActionResult> Editar(ProfessorSemSenhaModel professor)
        {
            try
            {
                if (ModelState.IsValid) //Veridica se a model é válida
                {
                    //Chamada de um método auxiliar que atualiza os dados do professor e retorna uma model já atualizada
                    var professorModel = await UpdateTeacher(professor);

                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    //Solicitação para a API gravar no banco de dados os dados atualizados do professor
                    var response = await httpClient.PutAsJsonAsync($"professor/update", professorModel);

                    if (response.IsSuccessStatusCode)
                    {   //Atualização com sucesso, retorna pra tela inicial do professor
                        TempData["MensagemSucesso"] = "Dados do professor atualizados com sucesso";
                        return RedirectToAction("Index", "Professor", new { id = professorModel.Id });
                    }
                    //Erro na atualização
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


        //Método que retorna uma view que apresenta os dados da disciplina
        public async Task<IActionResult> DetalhesDisciplina(int disciplineId, int teacherId)
        {
            try
            {
                //Chamado de um método auxiliar que solicita os dados de uma disciplina para a API
                DisciplinaModel disciplina = await GetDiscipline(disciplineId);
                ViewBag.teacherId = teacherId.ToString();
                return View(disciplina); //Retorna a view, passando os dados da disciplina
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        //Método que retorna uma view que exibirá a lista de alunos de uma determinada disciplinas e suas respectivas notas 
        public async Task<IActionResult> VerNotas(int disciplineId, int teacherId, string nameTeacher)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados das matrículas de uma determinada disciplina para a API com base no id da disciplina
                List<MatriculaModel> matriculas = await httpClient.GetFromJsonAsync<List<MatriculaModel>>($"matricula/getallbyiddiscipline/{disciplineId}");
                //Chamado de um método auxiliar que solicita os dados de uma disciplina para a API
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


        //Método que retorna uma view que contém um formulário para lançamento de notas de um aluno
        public async Task<IActionResult> LancarNotas(int disciplineId, int teacherId, string nameTeacher, int matriculaId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados de uma matricula para a API com base no id da matrícula
                var matricula = await httpClient.GetFromJsonAsync<MatriculaModel>($"matricula/getbyid/{matriculaId}");
                //Chamado de um método auxiliar que solicita os dados de uma disciplina para a API
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


        //Método que recebe os dados do formulário de atualização de notas e processa a atualização 
        [HttpPost]
        public async Task<IActionResult> LancarNotas(int disciplineId, int teacherId, string nameTeacher, MatriculaModel matricula)
        {
            try
            {
                if (ModelState.IsValid) // Verifica se os dados do formulário são válidos
                {
                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    //Solicitação dos dados de um aluno para a API com base no id do aluno
                    var aluno = await httpClient.GetFromJsonAsync<AlunoModel>($"aluno/getbyid/{matricula.AlunoId}");
                    //Solicitação de atualização para a API dos dados da matrícula
                    var response = await httpClient.PutAsJsonAsync($"matricula/update", matricula);

                    if (response.IsSuccessStatusCode) //Dados atualizados no banco de dados
                    {
                        TempData["MensagemSucesso"] = $"Notas do(a) aluno(a) {aluno.Nome} atualizadas com sucesso";
                        return RedirectToAction("VerNotas", "Professor", new { disciplineId = disciplineId, teacherId = teacherId, nameTeacher = nameTeacher });
                    }
                    //Erro ao atualizar os dados no banco de dados
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


        //Método que processa a consolidação ou a reabertura da disciplina (alteração do status) 
        public async Task<IActionResult> UpdateStatus(int disciplineId, int teacherId, string nameTeacher)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados de uma disciplina para a API com base no id da disciplina
                var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>($"disciplina/getbyid/{disciplineId}");
                //Solicitação a API a atualização do status da disciplina no banco de dados, passando o status atual invertido
                var response = await httpClient.PutAsync($"disciplina/updatestatus?status={!disciplina.Status}&id={disciplineId}", null);

                if (response.IsSuccessStatusCode) //Atualização concluída com sucesso
                {
                    TempData["MensagemSucesso"] = disciplina.Status ? "Disciplina consolidada com sucesso" : "Disciplina reaberta com sucesso";
                    return RedirectToAction("DetalhesDisciplina", "Professor", new { disciplineId = disciplineId, teacherId = teacherId });
                }
                //Erro na atualização do status
                TempData["MensagemErro"] = "Ops, erro ao atualizar o status da disciplina";
                return RedirectToAction("DetalhesDisciplina", "Professor", new { disciplineId = disciplineId, teacherId = teacherId });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in ProfessorController.Criar: " + ex.Message);
                return RedirectToAction("DetalhesDisciplina", "Professor", new { disciplineId = disciplineId, teacherId = teacherId });
            }
        }

 
        //Método auxiliar que solicita a API dos dados de um professor
        private async Task<ProfessorModel> GetTeacherById(int id)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados de um professor para a API, com base no id de um professor
                var professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{id}");

                if (professor != null)
                {
                    return professor; //retorna os dados do professor
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


        //Método auxiliar que solicita a API dos dados de um professor, e atualiza para os dados atuais
        private async Task<ProfessorModel> UpdateTeacher(ProfessorSemSenhaModel professor)
        {
            ProfessorModel professorModel = await GetTeacherById(professor.Id);
            professorModel.Nome = professor.Nome;
            professorModel.Email = professor.Email;
            professorModel.Login = professor.Login;
            return professorModel; //Retorna os dados atualizados do professor
        }


        //Método auxiliar que solicita a API dos dados de um disciplina
        private async Task<DisciplinaModel> GetDiscipline(int disciplineId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação dos dados de uma disciplina para a API, com base no Id 
                var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>($"disciplina/getbyid/{disciplineId}");

                if (disciplina != null)
                {
                    return disciplina; //Retorna os dados da disciplina
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

         
        //View de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
