using CodeSpire.Api.Serialization;
using CodeSpire.Application;
using CodeSpire.Domain;
using CodeSpire.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// needed for swashbuckle to find minimal api endpoints
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CodeSpire API",
        Description = "An ASP.NET Core Web API for managing insurance policies"
    });
});

builder.Services.Configure<JsonOptions>(c =>
{
    c.JsonSerializerOptions.ConfigureJsonConverters();
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.MapGet("/documents", (IRepository repo) => repo.List());
app.MapGet("/documents/{id:guid}", (IRepository repo, Guid id) => repo.Find(id));
app.MapPost("/documents", (IRepository repo, Dokument dokument) => repo.Add(dokument));
app.MapPut("/documents", (IRepository repo) => repo.Save());

app.Run();