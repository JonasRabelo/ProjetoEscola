using Microsoft.Extensions.Configuration;
using Models;
using Repository;
using Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddScoped<IUsuarioRepository<ProfessorModel>, ProfessorRepository>( );
//builder.Services.AddScoped<IUsuarioRepository<AlunoModel>, AlunoRepository>();
//builder.Services.AddScoped<IDisciplinaRepository<DisciplinaModel>, DisciplinaRepository>();
//builder.Services.AddScoped<IMatriculaRepository<MatriculaModel>, MatriculaRepository>();
//builder.Services.AddScoped<ILoginRepository<LoginModel>, LoginRepository>();
//builder.Services.AddScoped<ISARepository<SuperUserModel>, SARepository>();
builder.Services.AddScoped<IUsuarioRepository<ProfessorModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new ProfessorRepository(connectionString);
});

builder.Services.AddScoped<IUsuarioRepository<AlunoModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new AlunoRepository(connectionString);
});

builder.Services.AddScoped<IDisciplinaRepository<DisciplinaModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new DisciplinaRepository(connectionString);
});

builder.Services.AddScoped<IMatriculaRepository<MatriculaModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new MatriculaRepository(connectionString);
});

builder.Services.AddScoped<ILoginRepository<LoginModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new LoginRepository(connectionString);
});

builder.Services.AddScoped<ISARepository<SuperUserModel>>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");
    return new SARepository(connectionString);
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
