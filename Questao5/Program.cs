using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Extensoes;
using Questao5.Business.ServicosAplicacao;
using Questao5.Business.ServicosAplicacao.Interfaces;
using Questao5.Domain.Servicos;
using Questao5.Domain.Servicos.Interfaces;
using Questao5.Infrastructure.Repositorios;
using Questao5.Infrastructure.Repositorios.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiVersioning(options => {
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddControllers(options => {
    options.UseRoutePrefix("v{version:apiVersion}");
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// service dependencies
builder.Services.AddScoped<IContaCorrenteApplicationService, ContaCorrenteApplicationService>();
builder.Services.AddScoped<IIdEmPotenciaApplicationService, IdEmPotenciaApplicationService>();
builder.Services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
builder.Services.AddScoped<IIdEmPotenciaService, IdEmPotenciaService>();
builder.Services.AddScoped<IContaCorrenteRepositorio, ContaCorrenteRepositorio>();
builder.Services.AddScoped<IMovimentoRepositorio, MovimentoRepositorio>();
builder.Services.AddScoped<IIdEmPotenciaRepositorio, IdEmPotenciaRepositorio>();

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

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


