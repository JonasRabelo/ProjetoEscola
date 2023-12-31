﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Filters
{
    // <summary>
    /// Filtro de ação que redireciona para a página de login se o usuário não estiver autenticado como professor.
    /// </summary>
    public class PaginaParaProfessorLogado : ActionFilterAttribute
    {
        /// <summary>
        /// Executado antes de uma ação ser executada. Verifica se um professor está autenticado e redireciona conforme necessário.
        /// </summary>
        /// <param name="context">Contexto da execução da ação.</param>
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
                ProfessorModel professor = null;
                if (!string.IsNullOrEmpty(sessaoProfessor)) professor = JsonConvert.DeserializeObject<ProfessorModel>(sessaoProfessor);

                if (professor != null)
                {
                    // Professor autenticado, permitir acesso à página
                    base.OnActionExecuting(context);
                }
                else
                {
                    if (!string.IsNullOrEmpty(sessaoAluno))
                    {
                        // Redirecionar para a página do Aluno
                        if (JsonConvert.DeserializeObject<AlunoModel>(sessaoAluno) != null) context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Aluno" }, { "action", "Index" } });
                    }
                    else if (!string.IsNullOrEmpty(sessaoSuperUSer))
                    {
                        // Redirecionar para a página do SuperUser
                        if (JsonConvert.DeserializeObject<SuperUserModel>(sessaoSuperUSer) != null) context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "SuperUser" }, { "action", "Home" } });
                    }
                }
            }
        }
    }
}
