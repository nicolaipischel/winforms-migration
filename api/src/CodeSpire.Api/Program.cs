using CodeSpire.Application;
using CodeSpire.Domain;
using CodeSpire.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/documents", (IRepository repo) => repo.List());
app.MapGet("/documents/{id:guid}", (IRepository repo, Guid id) => repo.Find(id));
app.MapPost("/documents", (IRepository repo, Dokument dokument) => repo.Add(dokument));
app.MapPut("/documents", (IRepository repo) => repo.Save());

app.Run();