using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    /// <summary>
    /// Classe responsável por gerenciar a sessão do usuário do tipo Aluno.
    /// Implementa a interface ISessaoUsuario para operações relacionadas à sessão.
    /// </summary>
    public class SessaoAluno : ISessaoUsuario<AlunoModel>
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessaoAluno(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }


        /// <summary>
        /// Recupera a sessão do usuário do tipo Aluno.
        /// </summary>
        /// <returns>Objeto do tipo AlunoModel contendo as informações da sessão.</returns>
        public AlunoModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoAluno");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<AlunoModel>(sessaoUsuario);
        }


        /// <summary>
        /// Cria a sessão do usuário do tipo Aluno.
        /// </summary>
        /// <param name="aluno">Objeto do tipo AlunoModel a ser armazenado na sessão.</param>
        public void CriarSessaoDoUsuario(AlunoModel aluno)
        {
            _httpContext.HttpContext.Session.SetString("sessaoAluno", JsonConvert.SerializeObject(aluno));
        }


        /// <summary>
        /// Remove a sessão do usuário do tipo Aluno.
        /// </summary>
        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoAluno");
        }
    }
}
