using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    /// <summary>
    /// Classe responsável por gerenciar a sessão do usuário do tipo Professor.
    /// Implementa a interface ISessaoUsuario para operações relacionadas à sessão.
    /// </summary>
    public class SessaoProfessor : ISessaoUsuario<ProfessorModel>
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessaoProfessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }


        /// <summary>
        /// Recupera a sessão do usuário do tipo Professor.
        /// </summary>
        /// <returns>Objeto do tipo ProfessorModel contendo as informações da sessão.</returns>
        public ProfessorModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoProfessor");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<ProfessorModel>(sessaoUsuario);
        }


        /// <summary>
        /// Cria a sessão do usuário do tipo Professor.
        /// </summary>
        /// <param name="professor">Objeto do tipo ProfessorModel a ser armazenado na sessão.</param>
        public void CriarSessaoDoUsuario(ProfessorModel professor)
        {
            _httpContext.HttpContext.Session.SetString("sessaoProfessor", JsonConvert.SerializeObject(professor));
        }


        /// <summary>
        /// Remove a sessão do usuário do tipo Professor.
        /// </summary>
        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoProfessor");
        }
    }
}
