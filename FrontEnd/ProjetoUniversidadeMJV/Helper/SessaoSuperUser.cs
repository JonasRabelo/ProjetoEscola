using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    public class SessaoSuperUser : ISessaoUsuario<SuperUserModel>
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessaoSuperUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }


        public SuperUserModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoSA");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<SuperUserModel>(sessaoUsuario);
        }

        public void CriarSessaoDoUsuario(SuperUserModel superUser)
        {
            _httpContext.HttpContext.Session.SetString("sessaoSA", JsonConvert.SerializeObject(superUser));
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoSA");
        }
    }
}
