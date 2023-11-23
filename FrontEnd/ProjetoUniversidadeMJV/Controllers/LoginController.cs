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


        //Método inicial do sistema, retorna a view de login se não houver um usuário logado
        public IActionResult Index()
        {
            AlunoModel aluno = _sessaoAluno.BuscarSessaoDoUsuario();
            ProfessorModel professor = _sessaoProfessor.BuscarSessaoDoUsuario();
            SuperUserModel superUser = _sessaoSuperUser.BuscarSessaoDoUsuario();
            //Se o usuário estiver logado, redireciona para sua tela inicial correspondente:
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
            //Se não houver usuário logado, retorna a view de login
            return View();
        }


        //Método para os usuários se deslogarem do sistema e retornarem a tela de login
        public IActionResult Sair()
        {
            //Verifica qual tipo de usuário está logado
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
                return RedirectToAction("SuperUser", "Login"); //Se o usuário for um super usuário, retorna para o login de superusuário
            }
            //Se o usuário era Aluno ou Professor, retorna para tela de login correspondente
            return RedirectToAction("Index", "Login");
        }


        //Método que retorna a view onde é possível escolher o tipo de perfil a ser cadastrado
        public IActionResult EscolhaPerfil()
        {
            return View();
        }


        //Método que retorna a view onde contém um formulário para cadastro de um professor
        public IActionResult CadastrarProfessor()
        {
            return View();
        }


        //Método que recebe os dados do formulário e processa o cadastro de um professor e o inicio da sessao do professor
        [HttpPost]
        public async Task<IActionResult> CadastrarProfessor(ProfessorModel professor)
        {
            if (ModelState.IsValid) // Verifica se a model recebida é válida
            {
                try
                {
                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    //Solicitação para a API cadastrar o professor no Banco de Dados
                    var response = await httpClient.PostAsJsonAsync($"professor/create", professor);

                    if (response.IsSuccessStatusCode) //Professor cadastrado
                    {
                        //Faz login com esse professor, solicitando que a API busque um professor por login e senha e recebe de volta os dados desse professor
                        var professorLogado = await httpClient.GetFromJsonAsync<ProfessorModel>(
                            $"login/getuserbylogin?Login={professor.Login}&Senha={professor.Senha}&Tipo=Professor"
                        );

                        if (professorLogado != null)
                        {
                            //Cria a sessão do professor e redireciona para rela inicial do professor
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


        //Método que retorna a view onde contém um formulário para cadastro de um aluno
        public IActionResult CadastrarAluno()
        {
            return View();
        }


        //Método que recebe os dados do formulário e processa o cadastro de um aluno e o inicio da sessao do aluno
        [HttpPost]
        public async Task<IActionResult> CadastrarAluno(AlunoModel aluno)
        {
            if (ModelState.IsValid) // Verifica se a model recebida é válida
            {
                try
                {
                    var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                    //Solicitação para a API cadastrar o aluno no Banco de Dados
                    var response = await httpClient.PostAsJsonAsync($"aluno/create", aluno);

                    if (response.IsSuccessStatusCode) //Aluno cadastrado
                    {
                        //Faz login com esse aluno, solicitando que a API busque um aluno por login e senha e recebe de volta os dados desse aluno
                        var alunoLogado = await httpClient.GetFromJsonAsync<AlunoModel>(
                            $"login/getuserbylogin?Login={aluno.Login}&Senha={aluno.Senha}&Tipo=Aluno"
                        );

                        if (alunoLogado != null)
                        {
                            //Cria a sessão do aluno e redireciona para rela inicial do aluno
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


        //Método que recebe os dados do formulário de login e processa a criação de sessão para o usuário
        [HttpPost]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid) // verifica se a model de login recebida é válida
                {
                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        if (loginModel.Tipo == "Aluno") //Verifica qual o tipo de tentativa de login (Nesse caso aluno)
                        {
                            //Solicita a API os dados de um aluno, que contém determinado login e senha
                            var aluno = await httpClient.GetFromJsonAsync<AlunoModel>($"login/getuserbylogin?Login={loginModel.Login}&Senha={loginModel.Senha}&Tipo={loginModel.Tipo}");
                            if (aluno != null)
                            {
                                //Se encontrou o aluno, faz novamente a verificação da senha
                                if (aluno.SenhaValida(loginModel.Senha))
                                {
                                    //Cria sessão para o aluno e redireciona para tela inicial do aluno
                                    _sessaoAluno.CriarSessaoDoUsuario(aluno);
                                    return RedirectToAction("Index", "Aluno", new { id = aluno.Id });
                                }
                                TempData["MensagemErro"] = $"A senha do aluno é inválida. Por Favor, tente novamente.";
                                return RedirectToAction("Index", "Login");
                            }

                        }
                        else if (loginModel.Tipo == "Professor") // Login como professor
                        {
                            //Solicita a API os dados de um professor, que contém determinado login e senha
                            var professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"login/getuserbylogin?Login={loginModel.Login}&Senha={loginModel.Senha}&Tipo={loginModel.Tipo}");
                            if (professor != null)
                            {
                                //Se encontrou o professor, faz novamente a verificação da senha
                                if (professor.SenhaValida(loginModel.Senha))
                                {
                                    //Cria sessão para o professor e redireciona para tela inicial do professor
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


        //Método que retorna a view de login do superUser
        public IActionResult SuperUser()
        {
            return View();
        }


        //Método que faz o processamento de login do superusuário
        [HttpPost]
        public async Task<IActionResult> EntrarSuperUser(SuperUserModel superUser)
        {
            try
            {
                if (ModelState.IsValid) //Verifica se a model recebida do formulario de login é válida
                {
                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        //Solitação para a API verificar se os dados correspondem aos dados de acesso como superusuário
                        if (await httpClient.GetFromJsonAsync<bool>($"superuser/get?Login={superUser.Login}&Senha={superUser.Senha}"))
                        {
                            //Se positivo, cria uma sessão como super usuário e redireciona para tela inicial de super usuário
                            _sessaoSuperUser.CriarSessaoDoUsuario(superUser);
                            return RedirectToAction("Home", "SuperUser");
                        }
                        else
                        {   //Informa erro ao tentar logar como superusuário
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