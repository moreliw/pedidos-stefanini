using FluentValidation;
using FluentValidation.AspNetCore;
using Pedidos.Infrastructure;
using Pedidos.API.Validators;
using Pedidos.API.Middleware;
using Microsoft.EntityFrameworkCore;
using Pedidos.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CriarPedidoRequestValidator>();

// CORS aberto (qualquer origem/cabeçalho/método). Use com cautela em produção.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Aplica migrações automaticamente na inicialização (safe para ambientes controlados)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PedidosDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

// Middleware de exceções padrão
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
