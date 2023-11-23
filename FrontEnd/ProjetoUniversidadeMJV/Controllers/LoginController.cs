using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Helper;
using ProjetoEscolaMJV.Models;
using System.Net.Http;
using System.Text;

namespace ProjetoEscolaMJV.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ISessaoUsuario<ProfessorModel> _sessaoProfessor;
        private readonly ISessaoUsuario<AlunoModel> _sessaoAluno;
        private readonly ISessaoUsuario<SuperUserModel> _sessaoSuperUser;

        public LoginController(IHttpClientFactory httpClient, ISessaoUsuario<ProfessorModel> sessaoProfessor, ISessaoUsuario<AlunoModel> sessaoAluno, ISessaoUsuario<SuperUserModel> sessaoSuperUser)
        {
            _httpClient = httpClient;
            _sessaoProfessor = sessaoProfessor;
            _sessaoAluno = sessaoAluno;
            _sessaoSuperUser = sessaoSuperUser;
        }

        public IActionResult Index()
        {
            AlunoModel aluno = _sessaoAluno.BuscarSessaoDoUsuario();
            ProfessorModel professor = _sessaoProfessor.BuscarSessaoDoUsuario();
            SuperUserModel superUser = _sessaoSuperUser.BuscarSessaoDoUsuario();
            //Se o usuário estiver logado:
            if (aluno != null)
            {
                return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
            }
            else if (professor != null)
            {
                return RedirectToAction("Index", "Professor", new { id = professor.Id });
            }
            else if (professor != null)
            {
                return RedirectToAction("Home", "SuperUser");
            }

            return View();
        }

        public IActionResult Sair()
        {
            if (_sessaoProfessor.BuscarSessaoDoUsuario() != null)
            {
                _sessaoProfessor.RemoverSessaoUsuario();
            }
            else if (_sessaoAluno.BuscarSessaoDoUsuario() != null)
            {
                _sessaoAluno.RemoverSessaoUsuario();
            }
            else if (_sessaoSuperUser.BuscarSessaoDoUsuario() != null)
            {
                _sessaoSuperUser.RemoverSessaoUsuario();
            }
            return RedirectToAction("Index", "Login");
        }


        public IActionResult EscolhaPerfil()
        {
            return View();
        }

        public IActionResult CadastrarProfessor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProfessor(ProfessorModel professor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    var response = await httpClient.PostAsJsonAsync($"professor/create", professor);

                    if (response.IsSuccessStatusCode)
                    {
                        var professorLogado = await httpClient.GetFromJsonAsync<ProfessorModel>(
                            $"login/getuserbylogin?Login={professor.Login}&Senha={professor.Senha}&Tipo=Professor"
                        );

                        if (professorLogado != null)
                        {
                            _sessaoProfessor.CriarSessaoDoUsuario(professorLogado);
                            TempData["MensagemSucesso"] = "Professor cadastrado com sucesso";
                            return RedirectToAction("Index", "Professor", new { id = professorLogado.Id });
                        }
                        TempData["MensagemErro"] = "Ops, erro ao buscar o novo professor cadastrado";
                    }
                    TempData["MensagemErro"] = "Ops, não foi possível cadastrar o professor.";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Ops, ocorreu um erro: {ex.Message}";
                }
            }
            return View();
        }



        public IActionResult CadastrarAluno()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CadastrarAluno(AlunoModel aluno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    var response = await httpClient.PostAsJsonAsync($"aluno/create", aluno);

                    if (response.IsSuccessStatusCode)
                    {
                        var alunoLogado = await httpClient.GetFromJsonAsync<AlunoModel>(
                            $"login/getuserbylogin?Login={aluno.Login}&Senha={aluno.Senha}&Tipo=Aluno"
                        );

                        if (alunoLogado != null)
                        {
                            _sessaoAluno.CriarSessaoDoUsuario(alunoLogado);
                            TempData["MensagemSucesso"] = "Aluno cadastrado com sucesso";
                            return RedirectToAction("Index", "Aluno", new { id = alunoLogado.Id });
                        }
                        TempData["MensagemErro"] = "Ops, erro ao buscar o novo aluno cadastrado";
                    }
                    TempData["MensagemErro"] = "Ops, não foi possível cadastrar o aluno.";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Ops, ocorreu um erro: {ex.Message}";
                }
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        if (loginModel.Tipo == "Aluno")
                        {
                            var aluno = await httpClient.GetFromJsonAsync<AlunoModel>($"login/getuserbylogin?Login={loginModel.Login}&Senha={loginModel.Senha}&Tipo={loginModel.Tipo}");
                            if (aluno != null)
                            {
                                if (aluno.SenhaValida(loginModel.Senha))
                                {
                                    _sessaoAluno.CriarSessaoDoUsuario(aluno);
                                    return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
                                }

                                TempData["MensagemErro"] = $"A senha do aluno é inválida. Por Favor, tente novamente.";
                                return RedirectToAction("Index", "Login");
                            }

                        }
                        else if (loginModel.Tipo == "Professor")
                        {
                            var professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"login/getuserbylogin?Login={loginModel.Login}&Senha={loginModel.Senha}&Tipo={loginModel.Tipo}");
                            if (professor != null)
                            {
                                if (professor.SenhaValida(loginModel.Senha))
                                {
                                    _sessaoProfessor.CriarSessaoDoUsuario(professor);
                                    return RedirectToAction("Index", "Professor", new { id = professor.Id });
                                }
                                TempData["MensagemErro"] = $"A senha do professor é inválida. Por Favor, tente novamente.";
                                return RedirectToAction("Index", "Login");
                            }
                            TempData["MensagemErro"] = $"Login incorreto, tente novamente.";
                            return RedirectToAction("Index", "Login");
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login, tente novamente, mais detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


        public IActionResult SuperUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EntrarSuperUser(SuperUserModel superUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        if (await httpClient.GetFromJsonAsync<bool>($"superuser/get?Login={superUser.Login}&Senha={superUser.Senha}"))
                        {
                            _sessaoSuperUser.CriarSessaoDoUsuario(superUser);
                            return RedirectToAction("Home", "SuperUser");
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Dados de login do super usuário estão incorretos. Por favor, tente novamente.";
                            return RedirectToAction("SuperUser", "SuperUser");
                        }
                    }
                }

                return RedirectToAction("SuperUser");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login. Tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("SuperUser");
            }
        }
    }
}