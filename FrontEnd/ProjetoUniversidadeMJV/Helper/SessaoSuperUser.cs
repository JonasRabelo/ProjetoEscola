using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    /// <summary>
    /// Classe responsável por gerenciar a sessão do usuário do tipo SuperUser.
    /// Implementa a interface ISessaoUsuario para operações relacionadas à sessão.
    /// </summary>
    public class SessaoSuperUser : ISessaoUsuario<SuperUserModel>
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessaoSuperUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }


        /// <summary>
        /// Recupera a sessão do usuário do tipo SuperUser.
        /// </summary>
        /// <returns>Objeto do tipo SuperUserModel contendo as informações da sessão.</returns>
        public SuperUserModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoSA");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<SuperUserModel>(sessaoUsuario);
        }


        /// <summary>
        /// Cria a sessão do usuário do tipo SuperUser.
        /// </summary>
        /// <param name="superUser">Objeto do tipo SuperUserModel a ser armazenado na sessão.</param>
        public void CriarSessaoDoUsuario(SuperUserModel superUser)
        {
            _httpContext.HttpContext.Session.SetString("sessaoSA", JsonConvert.SerializeObject(superUser));
        }


        /// <summary>
        /// Remove a sessão do usuário do tipo SuperUser.
        /// </summary>
        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoSA");
        }
    }
}
