using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    public class SessaoProfessor : ISessaoUsuario<ProfessorModel>
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessaoProfessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }


        public ProfessorModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoProfessor");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<ProfessorModel>(sessaoUsuario);
        }

        public void CriarSessaoDoUsuario(ProfessorModel professor)
        {
            _httpContext.HttpContext.Session.SetString("sessaoProfessor", JsonConvert.SerializeObject(professor));
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoProfessor");
        }
    }
}
