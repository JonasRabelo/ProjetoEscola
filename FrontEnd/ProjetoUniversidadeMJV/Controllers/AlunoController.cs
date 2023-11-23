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


        // Método que retorna uma view que exibe a página inicial do aluno
        public async Task<IActionResult> Index(int id)
        {
            using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
            {
                //Obtém o aluno pelo Id, junto a API
                AlunoModel aluno = await httpClient.GetFromJsonAsync<AlunoModel>($"aluno/getbyid/{id}");
                if (aluno.Id == 0)
                {
                    //Se der algum erro ao buscar o aluno, pega os dados armazenados na sessão 
                    aluno = JsonConvert.DeserializeObject<AlunoModel>(HttpContext.Session.GetString("sessaoAluno"));
                }
                return View(aluno);
            }
        }


        // Método que retorna uma view que exibe um formulário de edição dos dados do aluno
        public async Task<IActionResult> Editar(int id)
        {
            //Obtém o aluno pelo Id, junto a API
            AlunoModel aluno = await GetStudentById(id);
            return View(aluno);
        }


        // Método que atualiza os detalhes do aluno com base no formulário submetido
        [HttpPost]
        public async Task<IActionResult> Editar(AlunoSemSenhaModel alunoSemSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Atualiza a entidade aluno com os dados atualizados
                    AlunoModel aluno = await UpdateStudent(alunoSemSenha);

                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        //Envia para a API a solicitação de atualização
                        var response = await httpClient.PutAsJsonAsync($"aluno/update", aluno);

                        if (response.IsSuccessStatusCode)
                        {
                            //Se atualizado com sucesso, cria uma mensagem de sucesso e redireciona para tela inicial do aluno.
                            TempData["MensagemSucesso"] = "Dados do aluno atualizados com sucesso";
                            return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
                        }
                        //Se deu erro na atualização, cria uma mensagem de erro e redireciona para tela inicial do aluno.
                        TempData["MensagemErro"] = "Ops, não foi possível atualizar os dados.";
                        return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
                    }
                }
                //Se a model que vir do formulário não for válida, retorna para a view do formulário.
                return View();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in AlunoController.Update: " + ex.Message);
                return RedirectToAction("Index", "Aluno", new { id = alunoSemSenha.Id });
            }
        }


        //Método que retorna uma view que lista todas as disciplinas da série do aluno logado no sistema
        public async Task<IActionResult> ListarDisciplinas(int id)
        {
            try
            {
                //Chamada de um método auxiliar que obtém o aluno pelo Id, junto a API
                AlunoModel aluno = await GetStudentById(id);
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Busca todas as disciplinas com base na serie do aluno
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


        //Método que retorna uma view que lista todas as disciplinas que o aluno está matriculado
        public async Task<IActionResult> DisciplinasMatriculadas(int id)
        {
            //Obtém o aluno pelo Id, junto a API
            AlunoModel aluno = await GetStudentById(id);

            if (aluno == null)
            {
                return NotFound();
            }

            List<DisciplinaModel> disciplinasMatriculadas = new List<DisciplinaModel>();

            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Percorre todas as matriculas do aluno
                foreach (var matricula in aluno.Matriculas!)
                {
                    // E solicita para a API os dados da matricula
                    var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>(
                        $"disciplina/getbyid/{matricula.DisciplinaId}"
                    );
                    // Os dados são armazenos em uma lista
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


        //Método que retorna uma view que contém uma tabela com os dados do aluno
        public async Task<IActionResult> VerDados(int id)
        {
            //Obtém o aluno pelo Id, junto a API
            AlunoModel aluno = await GetStudentById(id);
            return View(aluno);
        }


        //Método que retorna uma view que exibe as notas de todos os alunos matriculados em uma disciplina
        public async Task<IActionResult> VerNotas(int disciplineId, int studentId, string nameTeacher)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Busca todas as matriculas que sejam da disciplina com id informado.
                var matriculas = await httpClient.GetFromJsonAsync<List<MatriculaModel>>(
                    $"matricula/getallbyiddiscipline/{disciplineId}"
                );

                if (matriculas != null || matriculas.Count > 0)
                {
                    ViewBag.NomeDisciplina = matriculas[0].Disciplina!.Nome;
                }

                ViewBag.NomeProfessor = nameTeacher;
                ViewBag.Id = studentId.ToString();
                //Passa a lista de matriculas para a View
                return View(matriculas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Aluno");
            }
        }


        //Método que retorna uma view que exibe os detalhes de uma disciplina | Nome, professor, série, quantidade de alunos matriculados e a possibilidade do aluno se matricular ou ver as notas
        public async Task<IActionResult> DetalhesDisciplina(int disciplineId, int studentId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicita a API os dados da disciplina com o id informado
                var disciplina = await httpClient.GetFromJsonAsync<DisciplinaModel>(
                    $"disciplina/getbyid/{disciplineId}"
                );

                if (disciplina == null)
                {
                    //Se não encontar nenhuma disciplina, retorna a view com uma model vazia
                    ModelState.AddModelError(null, "Erro ao processar a solicitação");
                    return View(new DisciplinaModel());
                }

                bool matriculado = false;
                //Verificação para saber se o aluno está matriculado em determinado disciplina
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
                //Retorna para a view a disciplina
                return View(disciplina);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Aluno");
            }
        }


        // Método que realiza a matricula de um aluno em determinada disciplina
        public async Task<IActionResult> Matricular(int disciplineId, int studentId)
        {
            try
            {
                //Gera uma matricula preenchendo com os IDs recebidos de Aluno e Disciplina, e as notas como 0 
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
                //Solicita a API o registro da matricula no banco de dados
                var response = await httpClient.PostAsJsonAsync($"matricula/create", matricula);

                if (response.IsSuccessStatusCode)
                {
                    //Se a resposta for positiva, gera mensagem de sucesso e redireciona para a tela de detalhes da disciplina.
                    TempData["MensagemSucesso"] = "Matrícula realizada com sucesso";
                    return RedirectToAction("DetalhesDisciplina", "Aluno", new { disciplineId, studentId });
                }

                //Se houve erro ao cadastrar a matricula, gera mensagem de erro e redireciona para a tela de detalhes da disciplina.
                TempData["MensagemErro"] = "Ops, não foi possível realizar a matrícula.";
                return RedirectToAction("DetalhesDisciplina", "Aluno", new { disciplineId, studentId });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in AlunoController.Matricular: " + ex.Message);
                return RedirectToAction("DetalhesDisciplina", "Aluno", new { disciplineId, studentId });
            }
        }


        //Método auxiliar que solicita os dados de um aluno para a API com base no id informado
        private async Task<AlunoModel> GetStudentById(int id)
        {
            var httpClient = _httpClient.CreateClient("APIProjetoEscola");
            try
            {
                //Retorna a resposta da solicitação convertida em uma entidade AlunoModel
                return await httpClient.GetFromJsonAsync<AlunoModel>($"aluno/getbyid/{id}");
            }
            catch (Exception)
            {
                return null;
            }
        }


        //Método auxiliar para edição de dados do aluno
        private async Task<AlunoModel> UpdateStudent(AlunoSemSenhaModel alunoSemSenha)
        {
            //Solicita os dados do aluno gravados no banco de dados e os atualiza
            AlunoModel aluno = await GetStudentById(alunoSemSenha.Id);
            aluno.Nome = alunoSemSenha.Nome;
            aluno.Email = alunoSemSenha.Email;
            aluno.Login = alunoSemSenha.Login;
            aluno.Serie = alunoSemSenha.Serie;
            return aluno;
        }


        //Página de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

