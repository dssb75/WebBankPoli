using AccountServiceReplica.Data;
using AccountServiceReplica.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// CRUD con prefijo /api/accounts
app.MapGet("/api/accounts", () =>
{
    return Results.Ok(new
    {
        Source = "Replica",
        Timestamp = DateTime.Now,
        Data = AccountRepository.GetAll()
    });
});

app.MapGet("/api/accounts/{id}", (int id) =>
{
    var account = AccountRepository.GetById(id);
    return account is not null
        ? Results.Ok(new
        {
            Source = "Replica",
            Timestamp = DateTime.Now,
            Data = account
        })
        : Results.NotFound();
});

app.MapPost("/api/accounts", (Account account) =>
{
    AccountRepository.Add(account);
    return Results.Created($"/api/accounts/{account.Id}", new
    {
        Source = "Replica",
        Timestamp = DateTime.Now,
        Data = account
    });
});

app.MapPut("/api/accounts/{id}", (int id, Account account) =>
{
    if (AccountRepository.GetById(id) is null) return Results.NotFound();
    account.Id = id;
    AccountRepository.Update(account);
    return Results.Ok(new
    {
        Source = "Replica",
        Timestamp = DateTime.Now,
        Data = account
    });
});

app.MapDelete("/api/accounts/{id}", (int id) =>
{
    if (AccountRepository.GetById(id) is null) return Results.NotFound();
    AccountRepository.Delete(id);
    return Results.Ok(new
    {
        Source = "Replica",
        Timestamp = DateTime.Now,
        Message = $"Cuenta {id} eliminada en Replica"
    });
});

// Endpoint de balanceo para pruebas
app.MapGet("/api/balanceo", () =>
{
    return Results.Ok(new
    {
        Source = "Replica",
        Timestamp = DateTime.Now,
        Message = "Respuesta desde AccountServiceReplica (Minimal API)"
    });
});

// Puerto distinto al original
app.Run("http://localhost:6003");
