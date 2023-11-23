using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Filters
{
    public class PaginaParaSuperUserLogado : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessaoAluno = context.HttpContext.Session.GetString("sessaoAluno");
            string sessaoProfessor = context.HttpContext.Session.GetString("sessaoProfessor");
            string sessaoSuperUSer = context.HttpContext.Session.GetString("sessaoSA");
            if (string.IsNullOrEmpty(sessaoSuperUSer)  && string.IsNullOrEmpty(sessaoProfessor) && string.IsNullOrEmpty(sessaoAluno))
            {
                // Usuário não autenticado, redirecionar para a página de login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "login" }, { "action", "Index" } });
            }
            else
            {
                SuperUserModel superUser = JsonConvert.DeserializeObject<SuperUserModel>(sessaoSuperUSer);

                if (superUser != null)
                {
                    // SuperUser autenticado, permitir acesso à página
                    base.OnActionExecuting(context);
                }
                else
                {
                    // Redirecionar para páginas específicas para Professor ou Aluno
                    ProfessorModel professor = JsonConvert.DeserializeObject<ProfessorModel>(sessaoProfessor);
                    AlunoModel aluno = JsonConvert.DeserializeObject<AlunoModel>(sessaoAluno);

                    if (professor != null)
                    {
                        // Redirecionar para a página do Professor
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Professor" }, { "action", "Index" } });
                    }
                    else if (superUser != null)
                    {
                        // Redirecionar para a página do Aluno
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Aluno" }, { "action", "Index" } });
                    }
                }
            }
        }

    }
}
