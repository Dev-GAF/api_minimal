using api_minimal;
using api_minimal.Data;
using api_minimal.models.DTOs;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// COnfig Entity Framework
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Endpoints


// Post: Incluir monitor
app.MapPost("/monitores", async (MonitorDto monitorDto, AppDbContext context) =>
{
    var monitor = new Monitor
    {
        RA = monitorDto.RA,
        Nome = monitorDto.Nome,
        Apelido = monitorDto.Apelido
    };

    context.Monitores.Add(monitor);
    await context.SaveChangesAsync();

    return Results.Created($"/monitores/{monitor.IdMonitor}", monitor);
})
.WithName("CriarMonitor")
.WithOpenApi();


app.Run();


