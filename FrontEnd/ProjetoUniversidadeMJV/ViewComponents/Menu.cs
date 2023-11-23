using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoALuno = HttpContext.Session.GetString("sessaoAluno");
            string sessaoProfessor = HttpContext.Session.GetString("sessaoProfessor");
            if (!string.IsNullOrEmpty(sessaoALuno))
            {
                AlunoModel aluno = JsonConvert.DeserializeObject<AlunoModel>(sessaoALuno);
                ViewBag.Id = aluno.Id.ToString();
                ViewBag.Nome = aluno.Nome;
                ViewBag.Perfil = 1.ToString();
                return View();
            }
            else if(!string.IsNullOrEmpty(sessaoProfessor))
            {
                ProfessorModel professor = JsonConvert.DeserializeObject<ProfessorModel>(sessaoProfessor);
                ViewBag.Id = professor.Id.ToString();
                ViewBag.Nome = professor.Nome;
                ViewBag.Perfil = 2.ToString();
                return View();
            }
            else
            {
                ViewBag.Nome = "SuperUser";
                ViewBag.Perfil = 3.ToString();
                return View();
            }
        }
    }
}
