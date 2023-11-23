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


        //Método que retorna a página inicial do SuperUser
        public IActionResult Home()
        {
            return View();
        }


        //Método que busca solicita os dados de todos os professores para a API e retorna uma View que apresenta a lista desses professores em uma tabela.
        public async Task<IActionResult> Professores()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    // Solicitação de todos os dados de professores para a API
                    var professores = await httpClient.GetFromJsonAsync<List<ProfessorModel>>("professor/getall");

                    // Se tiver mais de um professor na lista, repassa a lista de professores para a View
                    if (professores != null)
                    {
                        return View(professores);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<ProfessorModel>()); //Caso não tenha nenhum professor na lista, repassa uma lista vazia para a view.
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }


        //Método que busca solicita os dados de todos os alunos para a API e retorna uma View que apresenta a lista desses alunos em uma tabela.
        public async Task<IActionResult> Alunos()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    // Solicitação de todos os dados de alunos para a API
                    var alunos = await httpClient.GetFromJsonAsync<List<AlunoModel>>("aluno/getall");
                    // Se tiver mais de um aluno na lista, repassa a lista de alunos para a View
                    if (alunos != null)
                    {
                        return View(alunos);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<AlunoModel>()); //Caso não tenha nenhum aluno na lista, repassa uma lista vazia para a view.
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }


        //Método que busca solicita os dados de todas as disciplinas para a API e retorna uma View que apresenta a lista dessas disciplinas em uma tabela.
        public async Task<IActionResult> Disciplinas()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    // Solicitação de todos os dados de disciplinas para a API
                    var disciplinas = await httpClient.GetFromJsonAsync<List<DisciplinaModel>>("disciplina/getall");
                    // Se tiver mais de uma disciplina na lista, repassa a lista de disciplinas para a View
                    if (disciplinas != null)
                    {
                        return View(disciplinas);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<DisciplinaModel>()); //Caso não tenha nenhuma disciplina na lista, repassa uma lista vazia para a view.
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }

        //Método que busca solicita os dados de todas as matrículas para a API e retorna uma View que apresenta a lista dessas matrículas em uma tabela.
        public async Task<IActionResult> Matriculas()
        {
            try
            {
                using (var httpClient = _httpClient.CreateClient("APIProjetoEscola"))
                {
                    // Solicitação de todos os dados de matriculas para a API
                    var matriculas = await httpClient.GetFromJsonAsync<List<MatriculaModel>>("matricula/getall");
                    // Se tiver mais de uma matrícula na lista, repassa a lista de matrículas para a View
                    if (matriculas != null)
                    {
                        foreach (var matricula in matriculas)
                        {
                            // Solicita para a API os dados dos professores que são responsáveis por cada disciplina que possue matrícula
                            matricula.Disciplina.Professor = await httpClient.GetFromJsonAsync<ProfessorModel>($"professor/getbyid/{matricula.Disciplina.ProfessorId}");
                        }
                        return View(matriculas);
                    }
                    else
                    {
                        ModelState.AddModelError(null!, "Erro ao processar a solicitação");
                        return View(new List<MatriculaModel>()); //Caso não tenha nenhuma matrícula na lista, repassa uma lista vazia para a view.
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "SuperUser");
            }
        }
        

        //Método para deletar um professor com base no Id recebido
        public async Task<IActionResult> TeacherDelete(int teacherId, string nameTeacher)
        {
            //Chamada de um método auxiliar para deletar um professor
            if (await DeleteTeacherAsync(teacherId))
            {
                TempData["MensagemSucesso"] = $"Professor [Id: {teacherId}: {nameTeacher}] deletado com sucesso!";
                return RedirectToAction("Professores", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar o professor [Id: {teacherId}: {nameTeacher}], tente novamente!";
            return RedirectToAction("Professores", "SuperUser");
        }


        //Método para deletar um aluno com base no Id recebido
        public async Task<IActionResult> StudentDelete(int studentId, string nameStudent)
        {
            //Chamada de um método auxiliar para deletar um aluno
            if (await DeleteStudentAsync(studentId))
            {
                TempData["MensagemSucesso"] = $"Aluno [Id: {studentId}: {nameStudent}] deletado com sucesso!";
                return RedirectToAction("Alunos", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar o aluno [Id: {studentId}: {nameStudent}], tente novamente!";
            return RedirectToAction("Alunos", "SuperUser");
        }


        //Método para deletar uma disciplina com base no Id recebido
        public async Task<IActionResult> DisciplineDelete(int disciplineId, string nameDiscipline)
        {
            //Chamada do método auxiliar para deletar a disciplina
            if (await DeleteDisciplineAsync(disciplineId))
            {
                TempData["MensagemSucesso"] = $"Disciplina [Id: {disciplineId}: {nameDiscipline}] deletada com sucesso!";
                return RedirectToAction("Disciplinas", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar a disciplina [Id: {disciplineId}: {nameDiscipline}], tente novamente!";
            return RedirectToAction("Disciplinas", "SuperUser");
        }


        //Método para deletar um aluno com base no Id recebido
        public async Task<IActionResult> RegistrationDelete(int registrationId)
        {
            //Chamada de um método auxiliar para deletar uma matrícula
            if (await DeleteRegistrationAsync(registrationId))
            {
                TempData["MensagemSucesso"] = $"Matricula nº {registrationId} deletada com sucesso!";
                return RedirectToAction("Matriculas", "SuperUser");
            }
            TempData["MensagemSucesso"] = $"Erro ao deletar a matricula nº {registrationId}, tente novamente!";
            return RedirectToAction("Matriculas", "SuperUser");
        }


        //Metodo auxiliar que deleta um professor
        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicita a API os dados das disciplinas desse professor.
                List<DisciplinaModel> disciplinas = await httpClient.GetFromJsonAsync<List<DisciplinaModel>>($"disciplina/getallbyidteacher/{teacherId}");
                foreach (DisciplinaModel disciplina in disciplinas)
                {
                    //Solicitação para a API deletar todas as matrículas das disciplinas do professor
                    await httpClient.DeleteAsync($"matricula/deletebyiddiscipline/{disciplina.Id}");
                }
                //Solicitação para a API deletar todas as disciplinas desse professor
                await httpClient.DeleteAsync($"disciplina/deletebyidteacher/{teacherId}");
                //Solicitação para a API deletar o professor
                var responseTeacher = await httpClient.DeleteAsync($"professor/delete/{teacherId}");
                if (responseTeacher.IsSuccessStatusCode)
                {
                    return true; //Professor deletado
                }
                return false; //Professor não deletado
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in SuperUsuario.DeleteTeacherAsync: " + ex.Message);
                return false;
            }
        }


        //Método auxiliar para apagar um aluno
        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {   
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação para a API deletar todas as matriculas desse aluno
                var response = await httpClient.DeleteAsync($"matricula/deletebyidstudent/{studentId}");
                //Solicitação para a API deletar o aluno
                var responseStudent = await httpClient.DeleteAsync($"aluno/delete/{studentId}");
                if (responseStudent.IsSuccessStatusCode) return true; //Aluno deletado
                return false; // Erro não deletado
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error in SuperUsuario.DeleteStudentAsync: " + ex.Message);
                return false;
            }
        }

       
        //Método auxiliar para apagar uma disciplina
        public async Task<bool> DeleteDisciplineAsync(int disciplineId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação para a API deletar todas as matrículas dessa disciplina
                var response = await httpClient.DeleteAsync($"matricula/deletebyiddiscipline/{disciplineId}");
                //Solicitação para a API deletar a disciplina
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

        
        //Método auxiliar para apagar uma matrícula
        private async Task<bool> DeleteRegistrationAsync(int registrationId)
        {
            try
            {
                var httpClient = _httpClient.CreateClient("APIProjetoEscola");
                //Solicitação para a API deletar a matrícula
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
