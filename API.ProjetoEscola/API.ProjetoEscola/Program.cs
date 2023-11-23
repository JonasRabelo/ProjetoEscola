using Models;
using Repository;
using Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUsuarioRepository<ProfessorModel>, ProfessorRepository>();
builder.Services.AddScoped<IUsuarioRepository<AlunoModel>, AlunoRepository>();
builder.Services.AddScoped<IDisciplinaRepository<DisciplinaModel>, DisciplinaRepository>();
builder.Services.AddScoped<IMatriculaRepository<MatriculaModel>, MatriculaRepository>();
builder.Services.AddScoped<ILoginRepository<LoginModel>, LoginRepository>();
builder.Services.AddScoped<ISARepository<SuperUserModel>, SARepository>();

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
