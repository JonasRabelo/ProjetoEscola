using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.ViewComponents
{
    public class Menu : ViewComponent
    {
        /// <summary>
        /// Método assíncrono que é chamado para renderizar o componente de visualização.
        /// </summary>
        /// <returns>Resultado do componente de visualização.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Recupera as informações de sessão para Aluno e Professor
            string sessaoALuno = HttpContext.Session.GetString("sessaoAluno");
            string sessaoProfessor = HttpContext.Session.GetString("sessaoProfessor");

            // Verifica se o usuário autenticado é um Aluno
            if (!string.IsNullOrEmpty(sessaoALuno))
            {
                AlunoModel aluno = JsonConvert.DeserializeObject<AlunoModel>(sessaoALuno);
                ViewBag.Id = aluno.Id.ToString();
                ViewBag.Nome = aluno.Nome;
                ViewBag.Perfil = 1.ToString();
                return View();
            }
            // Verifica se o usuário autenticado é um Professor
            else if (!string.IsNullOrEmpty(sessaoProfessor))
            {
                ProfessorModel professor = JsonConvert.DeserializeObject<ProfessorModel>(sessaoProfessor);
                ViewBag.Id = professor.Id.ToString();
                ViewBag.Nome = professor.Nome;
                ViewBag.Perfil = 2.ToString();
                return View();
            }
            // Se nenhum usuário autenticado for encontrado, assume que é um SuperUser
            else
            {
                ViewBag.Nome = "SuperUser";
                ViewBag.Perfil = 3.ToString();
                return View();
            }
        }
    }
}
