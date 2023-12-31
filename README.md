﻿
# Projeto Escola

## Introdução
Bem-vindo ao README do Projeto Escola, uma aplicação desenvolvida para gerencialmento de uma escola de ensino médio. Este documento fornece uma visão geral das funcionalidades, estrutura e tecnologias utilizadas no projeto.

## Visão Geral
O Projeto Escola visa simplificar a administração escolar, proporcionando uma plataforma onde Professores, Alunos e Superusuários podem gerenciar suas atividades de maneira eficiente.

## Tipos de Usuários
  **Professor:**

    Cadastrar, listar, e ver detalhes de disciplinas.
    Visualizar notas, lançar notas, consolidar e reabrir disciplinas.
    Gerenciar dados pessoais.

  **Aluno:**

    Visualizar disciplinas matriculadas e suas informações.
    Ver detalhes de disciplinas, notas e realizar matrículas.
    Gerenciar dados pessoais.

  **Superusuário:**

    Gerenciar matrículas, disciplinas, alunos e professores.
    Acesso a informações privilegiadas.
    Exclusões com impacto em cascata.

## API (Backend)

A API do Projeto Escola é responsável por gerenciar dados e lógica de negócios. Ela é construída utilizando .Net 7.0 e SQL Server.

### Estrutura de Diretórios

- **API WEB:** Contém os arquivos principais da API, como controllers, middleware, etc.
- **Models:** Biblioteca de classe contendo os modelos de dados utilizados pela API.
- **Repository:** Biblioteca de classe responsável por interagir com o banco de dados.

### Configuração

A configuração da API envolve a declaração e configuração de classes de repositório com injeção de dependência. Certifique-se de ter o ASP.NET MVC e outros pacotes necessários instalados. Você pode usar o NuGet Package Manager para instalar pacotes específicos do projeto.

-System.Data.SqlClient 4.8.5
-Swashbuckle.AspNertCore 6.5.0
-Microsoft.AspNetCore.OpenApi

A conexão com o banco de dados é estabelecida através da configuração da string de conexão no arquivo de configuração (`appsettings.json`). Abaixo está um exemplo de configuração para o repositório de professores:

```csharp
builder.Services.AddScoped<IUsuarioRepository<ProfessorModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new ProfessorRepository(connectionString);
});
```
##Uso:

A API do Projeto Escola oferece os seguintes endpoints:

#### Aluno

- **GetAll:** Obtém a lista de todos os alunos.
- **GetById:** Obtém informações de um aluno específico.
- **Create:** Cria um novo aluno.
- **Delete:** Exclui um aluno.
- **Update:** Atualiza informações de um aluno.
  
![EndPoint_Aluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/003%20-%20alunos.png?raw=true)

#### Professor

- **GetAll:** Obtém a lista de todos os professores.
- **GetById:** Obtém informações de um professor específico.
- **Create:** Cria um novo professor.
- **Delete:** Exclui um professor.
- **Update:** Atualiza informações de um professor.
  
![EndPoint_Professor](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/002%20-%20professores.png?raw=true)

#### Disciplina

- **Create:** Cria uma nova disciplina.
- **GetById:** Obtém informações de uma disciplina específica.
- **GetAllByIdTeacher:** Obtém todas as disciplinas associadas a um professor específico.
- **GetAllBySerie:** Obtém todas as disciplinas de uma série específica.
- **Update:** Atualiza informações de uma disciplina.
- **UpdateSerie:** Atualiza a série associada a uma disciplina.
- **Delete:** Exclui uma disciplina.
- **DeleteByIdTeacher:** Exclui todas as disciplinas associadas a um professor específico.
  
![EndPoint_Disciplina](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/005%20-%20disciplinas.png?raw=true)

#### Matricula

- **Create:** Cria uma nova matrícula.
- **GetById:** Obtém informações de uma matrícula específica.
- **GetByIdDiscipline:** Obtém todas as matrículas associadas a uma disciplina específica.
- **GetByIdStudent:** Obtém todas as matrículas associadas a um aluno específico.
- **Update:** Atualiza informações de uma matrícula.
- **Delete:** Exclui uma matrícula.
- **DeleteByIdStudent:** Exclui todas as matrículas associadas a um aluno específico.
- **DeleteByIdDiscipline:** Exclui todas as matrículas associadas a uma disciplina específica.
  
![EndPoint_Matricula](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/004%20-%20matricula.png?raw=true)

#### Login

- **GetByLogin:** Obtém informações de login.
  
![EndPoint_Login](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/001%20-%20login.png?raw=true)

#### SuperUser

- **Get (Login):** Obtém informações de superusuário pelo login.
  
![EndPoint_SuperUser](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/006%20-%20superuser.png?raw=true)

A API do Projeto Escola utiliza ADO.NET para acessar o banco de dados. O acesso é implementado através das classes SqlConnection, SqlCommand, e SqlDataReader. Abaixo está um exemplo simplificado da função GetAllBySerie na classe DisciplinaRepository:
```csharp
public List<DisciplinaModel> GetAllBySerie(int serie)
{
    List<DisciplinaModel> disciplinas = new List<DisciplinaModel>();
    string query = "SELECT Id, nome, serie, status, professorId, dataDeCadastro, dataDeAtualizacao FROM Disciplinas WHERE serie = @serie";
    try
    {
        using (SqlConnection connection = new SqlConnection(cs))
        {

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@serie", serie);

            connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DisciplinaModel disciplina = new DisciplinaModel()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["nome"].ToString()!,
                    Serie = (Series)Enum.Parse(typeof(Series), reader["serie"].ToString()!),
                    Status = bool.Parse(reader["status"].ToString()!),
                    ProfessorId = Convert.ToInt32(reader["professorId"]),
                    DataDeCadastro = DateTime.Parse(reader["dataDeCadastro"].ToString()!)
                };
                if (reader["dataDeAtualizacao"].ToString()! != "") disciplina.DataDeAtualizacao = DateTime.Parse(reader["dataDeAtualizacao"].ToString()!);
                disciplinas.Add(disciplina);
            }

        }
        return disciplinas;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in DisciplinaRepositry.GetAllByIdTeacher: {ex.Message}");
    }
    return disciplinas;
}
```
## ASP.NET MVC Frontend

O Frontend do Projeto Escola é construído com ASP.NET MVC para fornecer uma interface de usuário amigável e interativa.

### Configuração

Certifique-se de ter o ASP.NET MVC e outros pacotes necessários instalados. Você pode usar o NuGet Package Manager para instalar pacotes específicos do projeto:
-Microsoft.EntityFrameworkCore 7.0.13
-Microsoft.EntityFrameworkCore.Design 7.0.13
-Microsoft.EntityFrameworkCore.SqlServer 7.0.13
-Microsoft.EntityFrameworkCore.Tools 7.0.13
-Newtonsoft.Json 13.0.3

### Estrutura de Diretórios

- **Controllers:** Responsável por gerenciar o fluxo de requisições e respostas.
- **Enums:** Biblioteca de classe contendo enumerações utilizadas no projeto.
- **Extensions:** Biblioteca de classe contendo extensões úteis para o projeto.
- **Filters:** Contém filtros para controlar as sessões dos usuários e evitar o acesso à área de outros candidatos.
  - **SessionFilter.cs:** Implementa a lógica de filtro para verificar e controlar as sessões dos usuários.
- **Sessions:** Biblioteca de classe para gerenciamento de sessões.
- **Models:** Contém modelos específicos para o Frontend.
- **ViewComponents:** Componentes reutilizáveis para a construção de views.
- **Views:** Contém as views utilizadas para renderizar a interface do usuário.

### Comunicação com a API

O Frontend se comunica com a API do Projeto Escola utilizando o padrão `HttpClientFactory`, que é injetado em cada controlador quando necessário. A configuração do `HttpClient` é feita através da extensão `ConfigurarHttpClient` no arquivo `Startup.cs`, como mostrado abaixo:

```csharp
public static void ConfigurarHttpClient(this IServiceCollection services)
{
    string endpoint = @"https://localhost:7180/api/";

    services.AddHttpClient("APIProjetoEscola", c =>
    {
        c.BaseAddress = new Uri(endpoint);
        c.DefaultRequestHeaders.Add("Accept", "application/json");
    });
}
```
Quando os métodos IActionResult precisam se comunicar com a API, um HttpClient é criado usando httpClient.CreateClient("APIProjetoEscola"), utilizando o nome definido na extensão. Métodos como GetFromJsonAsync, PostAsJsonAsync, PutAsJsonAsync e DeleteAsync são utilizados para realizar operações na API. O JsonConvert é utilizado para desserializar objetos quando necessário, garantindo uma comunicação eficiente entre o Frontend e a API.

### Filtros e Sessões de Usuário
Filtros foram implementados para controlar o acesso dos usuários às diferentes áreas da aplicação. Esses filtros são configurados usando a classe ActionFilterAttribute, sobrescrevendo o método OnActionExecuting(ActionExecutingContext context). Eles verificam se há um usuário logado na sessão e se a página que está sendo acessada está dentro do escopo permitido para esse usuário. Caso contrário, o usuário é redirecionado para a tela inicial.

Para as sessões de usuário, foi utilizado o IHttpContextAccessor para gerenciar as sessões. Cada tipo de usuário (Aluno, Professor, SuperUser) tem sua própria classe de sessão que implementa a interface ISessaoUsuario<T>. As sessões são configuradas no arquivo Startup.cs da seguinte maneira:
```csharp

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ISessaoUsuario<AlunoModel>, SessaoAluno>();
builder.Services.AddScoped<ISessaoUsuario<ProfessorModel>, SessaoProfessor>();
builder.Services.AddScoped<ISessaoUsuario<SuperUserModel>, SessaoSuperUser>();
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

app.UseSession();
```
Cada classe de sessão do usuário pode criar, buscar e remover a sessão conforme necessário. A sessão é criada da seguinte forma:
```csharp
_httpContext.HttpContext.Session.SetString("sessaoAluno", JsonConvert.SerializeObject(aluno));
```
Isso define uma string como chave e serializa a classe AlunoModel para o valor, semelhante a um dicionário.

### Views - Navegação dos Usuários

As páginas aos usuários são várias, vou listar algumas:
**Tela de Login**
![TelaLogin](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/01%20-%20TelaLogin.png?raw=true)

**Tela de escolha de perfil para cadastrar**
![EndPoint_Disciplina](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/02%20-%20telaEscolhaCadastrarPerfil.png?raw=true)

**Tela de cadastro professor**
![CadastroProfessor](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/03%20-%20telaCadastroProfessor.png?raw=true)

**Tela de cadastro aluno**
![CadastroAluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/04%20-%20telaCadastroAluno.png?raw=true)

**Tela inicial - Professor**
![InicialProfessor](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/05%20-%20TelaInicialProfessor.png?raw=true)

**Tela de cadastrar disciplina - Professor**
![cadastrardisciplina](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/06%20-%20TelaCadastrarDisciplinaProfessor.png?raw=true)

**Tela de listagem de disciplinas - Professor**
![listardisciplinas](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/07%20-%20TelaListagemDisciplinaProfessor.png?raw=true)

**Tela de detalhes da disciplina - Professor**
![detalhesdisciplina](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/08%20-%20detalhesDeDisciplinaProfessor.png?raw=true)

**Tela de notas - Professor**
![notasprofessor](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/02%20-%20telaEscolhaCadastrarPerfil.png?raw=true)

**Tela de lançar notas - Professor**
![lancarnotas](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/10%20-%20lancarNotasProfessor.png?raw=true)

**Tela de dados - Professor**
![dadosProfessor](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/11%20-%20dadosProfessor.png?raw=true)

**Tela de editar dados - Professor**
![editardadosProfessor](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/12%20-%20editarDadosProfessor.png?raw=true)

**Tela inicial - Aluno**
![TelaInicialAluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/13%20-%20TelaInicialAluno.png?raw=true)

**Tela de disciplinas matriculadas - Aluno**
![disciplinasmatriculadas](https://github.com/JonasRabelo/ProjetoEscola/assets/130485427/e3544a62-993e-4795-bab8-c8b1d39ebc2f)
)

**Tela de Disciplinas da serie - Aluno**
![disciplinasserieAluno](https://github.com/JonasRabelo/ProjetoEscola/assets/130485427/ee3210e4-249d-4d3e-a3fb-53444a9d18bb)
)

**Tela de detalhes disciplina - Aluno**
![detalhesdisciplinaaluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/16%20-%20detalhesDisciplinaAluno.png?raw=true)

**Tela de notas - Aluno**
![notasAluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/17%20-%20telaNotasAluno.png?raw=true)

**Tela de ver dados - Aluno**
![dadosAluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/18%20-%20telaVerDadosAluno.png?raw=true)

**Tela de edição de dados - Alunos**
![edicaoDadosAluno](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/19%20-%20telaEdicaoDadosAluno.png?raw=true)

**Tela de login - SuperUser**
![loginSA](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/20%20-%20telaloginSuperUser.png?raw=true)

**Tela Inicial - SuperUser**
![telaInicialSA](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/21%20-%20TelaInicialSuperUser.png?raw=true)

**Tela de listagem de professores - SuperUser**
![listagemProfessoresSA](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/22%20-%20TelaListagemProfessores.png?raw=true)

**Tela de listagem de alunos - SuperUser**
![listagemAlunosSA](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/23%20-%20TelaListagemAlunos.png?raw=true)

**Tela de listagem de disciplinas - SuperUser**
![listagemDisciplinasSA](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/24%20-%20TelaListagemDisciplinas.png?raw=true)

**Tela de listagem de matriculas - SuperUser**
![listagemMatriculasSA](https://github.com/JonasRabelo/ProjetoEscola/blob/main/FrontEnd/ProjetoUniversidadeMJV/wwwroot/Imagens%20ReadMe/25%20-%20TelaListagemMatriculas.png?raw=true)
