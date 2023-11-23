using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Filters
{
    public class PaginaParaAlunoLogado : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessaoAluno = context.HttpContext.Session.GetString("sessaoAluno");
            string sessaoProfessor = context.HttpContext.Session.GetString("sessaoProfessor");
            string sessaoSuperUSer = context.HttpContext.Session.GetString("sessaoSA");
            if (string.IsNullOrEmpty(sessaoSuperUSer) && string.IsNullOrEmpty(sessaoProfessor) && string.IsNullOrEmpty(sessaoAluno))
            {
                // Usuário não autenticado, redirecionar para a página de login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "login" }, { "action", "Index" } });
            }
            else
            {
                AlunoModel aluno = JsonConvert.DeserializeObject<AlunoModel>(sessaoAluno);

                if (aluno != null)
                {
                    // Aluno autenticado, permitir acesso à página
                    base.OnActionExecuting(context);
                }
                else
                {
                    // Redirecionar para páginas específicas para SuperUser ou Professor
                    SuperUserModel superUser = JsonConvert.DeserializeObject<SuperUserModel>(sessaoSuperUSer);
                    ProfessorModel professor = JsonConvert.DeserializeObject<ProfessorModel>(sessaoProfessor);

                    if (professor != null)
                    {
                        // Redirecionar para a página do Professor
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Professor" }, { "action", "Index" } });
                    }
                    else if (superUser != null)
                    {
                        // Redirecionar para a página do SuperUser
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "SuperUser" }, { "action", "Home" } });
                    }
                }
            }
        }
    }
}
