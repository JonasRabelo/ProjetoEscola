using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjetoEscolaMJV.Extensions;
using ProjetoEscolaMJV.Helper;
using ProjetoEscolaMJV.Models;

namespace ProjetoUniversidadeMJV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Sessão de usuário
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ISessaoUsuario<AlunoModel>, SessaoAluno>();
            builder.Services.AddScoped<ISessaoUsuario<ProfessorModel>, SessaoProfessor>();
            builder.Services.AddScoped<ISessaoUsuario<SuperUserModel>, SessaoSuperUser>();
            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });
            //Extensão - Configurando HttpClient
            builder.Services.ConfigurarHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.MapControllerRoute(name: "aluno_index",
                pattern: "aluno/index/{id}",
                defaults: new { controller = "Aluno", action = "Index" });

            app.Run();
        }
    }
}