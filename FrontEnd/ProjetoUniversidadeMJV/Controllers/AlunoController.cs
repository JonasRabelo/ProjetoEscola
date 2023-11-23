using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Filters;
using ProjetoEscolaMJV.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace ProjetoEscolaMJV.Controllers
{
    [PaginaParaAlunoLogado]
    public class AlunoController : Controller
    {
        private readonly IHttpClientFactory _httpClient;

        public AlunoController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index(int id)
        {
            using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
            {

                AlunoModel aluno = await httpClient.GetFromJsonAsync<AlunoModel>($"aluno/getbyid/{id}");
                if (aluno == null)
                {
                    aluno = JsonConvert.DeserializeObject<AlunoModel>(HttpContext.Session.GetString("sessaoAluno"));
                }
                return View(aluno);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            AlunoModel aluno = await GetStudentById(id);
            return View(aluno);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(AlunoSemSenhaModel alunoSemSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AlunoModel aluno = await UpdateStudent(alunoSemSenha);

                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        var response = await httpClient.PutAsJsonAsync($"aluno/update", aluno);

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["MensagemSucesso"] = "Dados do aluno atualizados com sucesso";
                            return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
                        }

                        TempData["MensagemErro"] = "Ops, não foi possível atualizar os dados.";
                        return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in AlunoController.Update: " + ex.Message);
                return RedirectToAction("Index", "Aluno", new { id = alunoSemSenha.Id });
            }
        }

        public async Task<IActionResult> ListarDisciplinas(int id)
        {
            try
            {
                AlunoModel aluno = await GetStudentById(id);
                if (aluno == null)
                {
                    return NotFound();
                }
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var disciplinas = await httpClient.GetFromJsonAsync<List<DisciplinaModel>>(
                    $"disciplina/getallbyserie/{Convert.ToInt32(aluno.Serie)}"
                );

                ViewBag.Id = id.ToString();
                return View(disciplinas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        public async Task<IActionResult> DisciplinasMatriculadas(int id)
        {
            AlunoModel aluno = await GetStudentById(id);

            if (aluno == null)
            {
                return NotFound();
            }

            List<DisciplinaModel> disciplinasMatriculadas = new List<DisciplinaModel>();

            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                foreach (var matricula in aluno.Matriculas!)
                {
                    var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>(
                        $"disciplina/getbyid/{matricula.DisciplinaId}"
                    );

                    disciplinasMatriculadas.Add(disciplina!);
                }

                ViewBag.Id = aluno.Id.ToString();
                return View(disciplinasMatriculadas);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Aluno", new { id = id });
            }
        }


        public async Task<IActionResult> VerDados(int id)
        {
            AlunoModel aluno = await GetStudentById(id);
            return View(aluno);
        }


        public async Task<IActionResult> VerNotas(int disciplineId, int studentId, string nameTeacher)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var matriculas = await httpClient.GetFromJsonAsync<List<MatriculaModel>>(
                    $"matricula/getallbyiddiscipline/{disciplineId}"
                );

                if (matriculas != null || matriculas.Count > 0)
                {
                    ViewBag.NomeDisciplina = matriculas[0].Disciplina!.Nome;
                }

                ViewBag.NomeProfessor = nameTeacher;
                ViewBag.Id = studentId.ToString();

                return View(matriculas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Aluno");
            }
        }


        public async Task<IActionResult> DetalhesDisciplina(int disciplineId, int studentId)
        {
            await Console.Out.WriteLineAsync("DisciplineID : " + disciplineId + "| StudentID: " + studentId);
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>(
                    $"disciplina/getbyid/{disciplineId}"
                );

                if (disciplina == null)
                {
                    ModelState.AddModelError(null, "Erro ao processar a solicitação");
                    return View(new DisciplinaModel());
                }

                bool matriculado = false;
                foreach (var matricula in disciplina.Matriculas!)
                {
                    if (matricula.AlunoId == studentId)
                    {
                        matriculado = true;
                        break;
                    }
                }
                await Console.Out.WriteLineAsync(matriculado.ToString());
                ViewBag.matriculado = matriculado.ToString();
                ViewBag.Id = studentId.ToString();

                return View(disciplina);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Professor");
            }
        }


        public async Task<IActionResult> Matricular(int disciplineId, int studentId)
        {
            try
            {
                MatriculaModel matricula = new MatriculaModel()
                {
                    AlunoId = studentId,
                    DisciplinaId = disciplineId,
                    Nota1 = 0,
                    Nota2 = 0,
                    Nota3 = 0,
                    Nota4 = 0,
                    MediaFinal = 0
                };
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.PostAsJsonAsync($"matricula/create", matricula);

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Matrícula realizada com sucesso";
                    return RedirectToAction("DetalhesDisciplina", "Aluno", new { disciplineId, studentId });
                }

                TempData["MensagemErro"] = "Ops, não foi possível realizar a matrícula.";
                return RedirectToAction("DetalhesDisciplina", "Aluno", new { disciplineId, studentId });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in AlunoController.Matricular: " + ex.Message);
                return RedirectToAction("DetalhesDisciplina", "Aluno", new { disciplineId, studentId });
            }
        }


        public async Task<AlunoModel> GetStudentById(int id)
        {
            var httpClient = _httpClient.CreateClient("APIProjetoEscola");
            try
            {
                return await httpClient.GetFromJsonAsync<AlunoModel>($"aluno/getbyid/{id}");
            }
            catch (Exception)
            {
                return null;
            }
        }


        private async Task<AlunoModel> UpdateStudent(AlunoSemSenhaModel alunoSemSenha)
        {

            AlunoModel aluno = await GetStudentById(alunoSemSenha.Id);
            aluno.Nome = alunoSemSenha.Nome;
            aluno.Email = alunoSemSenha.Email;
            aluno.Login = alunoSemSenha.Login;
            aluno.Serie = alunoSemSenha.Serie;
            return aluno;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

