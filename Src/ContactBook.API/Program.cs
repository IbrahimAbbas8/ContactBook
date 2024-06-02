using ContactBook.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Reflection;
QuestPDF.Settings.License = LicenseType.Community;
QuestPDF.Settings.EnableDebugging = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Auth Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme,
        }
    };
    s.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
    s.AddSecurityRequirement(securityRequirement);
});

builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
    ));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.InfrastructureConfigMiddleware();
app.Run();
