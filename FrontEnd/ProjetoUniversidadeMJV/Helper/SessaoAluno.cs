using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    public class SessaoAluno : ISessaoUsuario<AlunoModel>
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessaoAluno(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }


        public AlunoModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _httpContext.HttpContext.Session.GetString("sessaoAluno");
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<AlunoModel>(sessaoUsuario);
        }

        public void CriarSessaoDoUsuario(AlunoModel aluno)
        {
            _httpContext.HttpContext.Session.SetString("sessaoAluno", JsonConvert.SerializeObject(aluno));
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoAluno");
        }
    }
}
