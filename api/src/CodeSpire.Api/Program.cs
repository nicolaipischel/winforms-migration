global using FastEndpoints;
using System.Text.Json.Serialization;
using CodeSpire.Infrastructure;
using FastEndpoints.ClientGen;
using FastEndpoints.Swagger;
using NJsonSchema.CodeGeneration.CSharp;

var apiVersion = "v1";
var projectName = "CodeSpire";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(s =>
{
    s.DocumentName = apiVersion;
    s.Title = $"{projectName} API";
    s.Version = apiVersion;
    s.Description = "An ASP.NET Core Web API for managing insurance policies";
}, serializerSettings: o =>
{
    o.PropertyNamingPolicy = null;
    o.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}, shortSchemaNames: true, removeEmptySchemas: true);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
});

const string apiClientName = "ApiClient";
var nameSpace = $"{projectName}.Client";

app.MapCSharpClientEndpoint("/csharp", apiVersion, s =>
{
    s.ClassName = apiClientName;
    s.GenerateClientInterfaces = true;
    s.CodeGeneratorSettings.PropertyNameGenerator = new CSharpPropertyNameGenerator();
    s.CSharpGeneratorSettings.Namespace = nameSpace;
});

app.MapTypeScriptClientEndpoint("/typescript", apiVersion, s =>
{
    s.ClassName = apiClientName;
    s.GenerateClientInterfaces = true;
    s.TypeScriptGeneratorSettings.Namespace = nameSpace;
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerGen();

await app.RunAsync();

// Workaround for https://github.com/dotnet/aspnetcore/issues/38474
public partial class Program { }