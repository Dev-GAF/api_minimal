using api_minimal;
using api_minimal.Data;
using api_minimal.models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Entity Framework com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilitar serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Monitoria", Version = "v1" });
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configurar Swagger (somente em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Monitoria v1");
        c.RoutePrefix = string.Empty; // Abre o Swagger direto na raiz
    });
}

app.UseHttpsRedirection();


// Endpoints -----------------------------

// POST: Incluir monitor
app.MapPost("/monitores", async (MonitorDto monitorDto, AppDbContext context) =>
{
    var monitor = new api_minimal.Monitor
    {
        RA = monitorDto.RA,
        Nome = monitorDto.Nome,
        Apelido = monitorDto.Apelido
    };

    context.Monitores.Add(monitor);
    await context.SaveChangesAsync();

    return Results.Created($"/monitores/{monitor.IdMonitor}", monitor);
});

// POST: Incluir horário
app.MapPost("/horarios", async (HorarioDto horarioDto, AppDbContext context) =>
{
    var monitor = await context.Monitores.FindAsync(horarioDto.IdMonitor);
    if (monitor == null) return Results.NotFound("Monitor não encontrado");

    var horario = new Horario
    {
        DiaSemana = horarioDto.DiaSemana,
        HorarioAtendimento = horarioDto.HorarioAtendimento,
        IdMonitor = horarioDto.IdMonitor
    };

    context.Horarios.Add(horario);
    await context.SaveChangesAsync();

    var result = new HorarioComIdDto
    {
        IdHorario = horario.IdHorario,
        DiaSemana = horario.DiaSemana,
        HorarioAtendimento = horario.HorarioAtendimento,
        IdMonitor = horario.IdMonitor
    };

    return Results.Created($"/horarios/{horario.IdHorario}", result);
});

// GET: Todos os monitores
app.MapGet("/monitores", async (AppDbContext context) =>
{
    var monitores = await context.Monitores
    .Include(m => m.Horarios)
    .ToListAsync();

    var resultado = monitores.Select(m => new MonitorComHorariosDto
    {
        IdMonitor = m.IdMonitor,
        RA = m.RA,
        Nome = m.Nome,
        Apelido = m.Apelido,
        Horarios = m.Horarios.Select(h => new HorarioDto
        {
            DiaSemana = h.DiaSemana,
            HorarioAtendimento = h.HorarioAtendimento,
            IdMonitor = h.IdMonitor,
            Monitor = new MonitorSimplesDto
            {
                IdMonitor = m.IdMonitor,
                Nome = m.Nome,
                Apelido = m.Apelido
            }
        }).ToList()
}).ToList();

    return Results.Ok(resultado);
});

// GET: Monitor por apelido com horários
app.MapGet("/monitores/{apelido}", async (string apelido, AppDbContext context) =>
{
    var monitor = await context.Monitores
        .Include(m => m.Horarios)
        .FirstOrDefaultAsync(m => m.Apelido == apelido);

    if (monitor == null) return Results.NotFound("Monitor não encontrado");

    var resultado = new MonitorComHorariosDto
    {
        IdMonitor = monitor.IdMonitor,
        RA = monitor.RA,
        Nome = monitor.Nome,
        Apelido = monitor.Apelido,
        Horarios = monitor.Horarios.Select(h => new HorarioDto
        {
            DiaSemana = h.DiaSemana,
            HorarioAtendimento = h.HorarioAtendimento,
            IdMonitor = h.IdMonitor,
            Monitor = new MonitorSimplesDto   
            {
                IdMonitor = monitor.IdMonitor,
                Nome = monitor.Nome,
                Apelido = monitor.Apelido
            }
        }).ToList()
    };

    return Results.Ok(resultado);
});


// PUT: Atualizar monitor
app.MapPut("/monitores/{id}", async (int id, MonitorDto monitorDto, AppDbContext context) =>
{
    var monitor = await context.Monitores.FindAsync(id);
    if (monitor == null) return Results.NotFound("Monitor não encontrado");

    monitor.RA = monitorDto.RA;
    monitor.Nome = monitorDto.Nome;
    monitor.Apelido = monitorDto.Apelido;

    await context.SaveChangesAsync();
    return Results.Ok(monitor);
});

// PUT: Atualizar horário do monitor
app.MapPut("/horarios/{idMonitor}/{diaSemana}", async (int idMonitor, int diaSemana, HorarioDto horarioDto, AppDbContext context) =>
{
    var horario = await context.Horarios
        .Include(h => h.Monitor)
            .ThenInclude(m => m.Horarios)
        .FirstOrDefaultAsync(h => h.IdMonitor == idMonitor && h.DiaSemana == diaSemana);

    if (horario == null)
        return Results.NotFound("Horário não encontrado");

    // Atualiza o horário
    horario.HorarioAtendimento = horarioDto.HorarioAtendimento;
    await context.SaveChangesAsync();

    // Monta DTO seguro e sem null (motivo 2)
    var result = new HorarioDto
    {
        DiaSemana = horario.DiaSemana,
        HorarioAtendimento = horario.HorarioAtendimento,
        IdMonitor = horario.IdMonitor,
        Monitor = new MonitorSimplesDto
        {
            IdMonitor = horario.Monitor.IdMonitor,
            Nome = horario.Monitor.Nome,
            Apelido = horario.Monitor.Apelido
        }
    };

    return Results.Ok(result);
});

// DELETE: Excluir monitor se não houver horários
app.MapDelete("/monitores/{id}", async (int id, AppDbContext context) =>
{
    var monitor = await context.Monitores
        .Include(m => m.Horarios)
        .FirstOrDefaultAsync(m => m.IdMonitor == id);

    if (monitor == null) return Results.NotFound("Monitor não encontrado");
    if (monitor.Horarios.Any()) return Results.BadRequest("Monitor possui horários associados");

    context.Monitores.Remove(monitor);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

// DELETE: Excluir horário
app.MapDelete("/horarios/{id}", async (int id, AppDbContext context) =>
{
    var horario = await context.Horarios.FindAsync(id);
    if (horario == null) return Results.NotFound("Horário não encontrado");

    context.Horarios.Remove(horario);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
