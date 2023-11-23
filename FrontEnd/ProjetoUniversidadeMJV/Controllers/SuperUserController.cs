using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoEscolaMJV.Filters;
using ProjetoEscolaMJV.Models;
using System.Net.Http;
using System.Text;

namespace ProjetoEscolaMJV.Controllers
{
    [PaginaParaSuperUserLogado]
    public class SuperUserController : Controller
    {
        private readonly IHttpClientFactory _httpClient;


        public SuperUserController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Entrar(SuperUserModel superUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                    {
                        if (await httpClient.GetFromJsonAsync<bool>($"superuser/get?Login={superUser.Login}&Senha={superUser.Senha}"))
                        {
                            return RedirectToAction("Home", "SuperUser");
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Dados de login do super usuário estão incorretos. Por favor, tente novamente.";
                            return RedirectToAction("Index", "SuperUser");
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login. Tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Home()
        {
            return View();
        }


        public async Task<IActionResult> Professores()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    var professores = await httpClient.GetFromJsonAsync<List<ProfessorModel>>("professor/getall");

                    if (professores != null)
                    {
                        return View(professores);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<ProfessorModel>());
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }

        public async Task<IActionResult> Alunos()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    var alunos = await httpClient.GetFromJsonAsync<List<AlunoModel>>("aluno/getall");

                    if (alunos != null)
                    {
                        return View(alunos);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<AlunoModel>());
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }


        public async Task<IActionResult> Disciplinas()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    var disciplinas = await httpClient.GetFromJsonAsync<List<DisciplinaModel>>("disciplina/getall");

                    if (disciplinas != null)
                    {
                        return View(disciplinas);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<DisciplinaModel>());
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }


        public async Task<IActionResult> Matriculas()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    var matriculas = await httpClient.GetFromJsonAsync<List<MatriculaModel>>("matricula/getall");

                    if (matriculas != null)
                    {
                        foreach (var matricula in matriculas)
                        {
                            matricula.Disciplina.Professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{matricula.Disciplina.ProfessorId}");
                        }
                        return View(matriculas);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<MatriculaModel>());
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }
        

        public async Task<IActionResult> TeacherDelete(int teacherId, string nameTeacher)
        {
            if (await DeleteTeacherAsync(teacherId))
            {
                TempData["MensagemSucesso"] = $"Professor [Id: {teacherId}: {nameTeacher}] deletado com sucesso!";
                return RedirectToAction("Professores", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar o professor [Id: {teacherId}: {nameTeacher}], tente novamente!";
            return RedirectToAction("Professores", "SuperUser");
        }


        public async Task<IActionResult> StudentDelete(int studentId, string nameStudent)
        {
            if (await DeleteStudentAsync(studentId))
            {
                TempData["MensagemSucesso"] = $"Aluno [Id: {studentId}: {nameStudent}] deletado com sucesso!";
                return RedirectToAction("Alunos", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar o aluno [Id: {studentId}: {nameStudent}], tente novamente!";
            return RedirectToAction("Alunos", "SuperUser");
        }


        public async Task<IActionResult> DisciplineDelete(int disciplineId, string nameDiscipline)
        {
            bool retorno = await DeleteDisciplineAsync(disciplineId);
            if (retorno)
            {
                TempData["MensagemSucesso"] = $"Disciplina [Id: {disciplineId}: {nameDiscipline}] deletada com sucesso!";
                return RedirectToAction("Disciplinas", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar a disciplina [Id: {disciplineId}: {nameDiscipline}], tente novamente!";
            return RedirectToAction("Disciplinas", "SuperUser");
        }


        public async Task<IActionResult> RegistrationDelete(int registrationId)
        {
            if (await DeleteRegistrationAsync(registrationId))
            {
                TempData["MensagemSucesso"] = $"Matricula nº {registrationId} deletada com sucesso!";
                return RedirectToAction("Matriculas", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar a matricula nº {registrationId}, tente novamente!";
            return RedirectToAction("Matriculas", "SuperUser");
        }


        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            try
            {
                await DeleteDisciplineAsync(teacherId);
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var responseTeacher = await httpClient.DeleteAsync($"professor/delete/{teacherId}");
                if (responseTeacher.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in SuperUsuario.DeleteTeacherAsync: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {   
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.DeleteAsync($"matricula/deletebyidstudent/{studentId}");

                var responseStudent = await httpClient.DeleteAsync($"aluno/delete/{studentId}");
                if (responseStudent.IsSuccessStatusCode) return true;
                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in SuperUsuario.DeleteStudentAsync: " + ex.Message);
                return false;
            }
        }

       
        public async Task<bool> DeleteDisciplineAsync(int disciplineId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.DeleteAsync($"matricula/deletebyiddiscipline/{disciplineId}");

                var responseDiscipline = await httpClient.DeleteAsync($"disciplina/delete/{disciplineId}");
                if (responseDiscipline.IsSuccessStatusCode) return true;
                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in SuperUsuario.DeleteDisciplineAsync: " + ex.Message);
                return false;
            }
        }

        
        private async Task<bool> DeleteRegistrationAsync(int registrationId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                var response = await httpClient.DeleteAsync($"matricula/delete/{registrationId}");
                if (response.IsSuccessStatusCode) return true;
                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in SuperUsuario.DeleteRegistrarionAsync: " + ex.Message);
                return false;
            }
        }
    }
}
