using AccountService.Data;
using AccountService.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// CRUD con prefijo /api/accounts
app.MapGet("/api/accounts", () =>
{
    return Results.Ok(new
    {
        Source = "Original",
        Data = AccountRepository.GetAll()
    });
});

app.MapGet("/api/accounts/{id}", (int id) =>
{
    var account = AccountRepository.GetById(id);
    return account is not null
        ? Results.Ok(new
        {
            Source = "Original",
            Data = account
        })
        : Results.NotFound();
});

app.MapPost("/api/accounts", (Account account) =>
{
    AccountRepository.Add(account);
    return Results.Created($"/api/accounts/{account.Id}", new
    {
        Source = "Original",
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
        Source = "Original",
        Data = account
    });
});

app.MapDelete("/api/accounts/{id}", (int id) =>
{
    if (AccountRepository.GetById(id) is null) return Results.NotFound();
    AccountRepository.Delete(id);
    return Results.Ok(new
    {
        Source = "Original",
        Message = $"Account {id} deleted"
    });
});

// Nuevo endpoint para balanceo
app.MapGet("/api/balanceo", () =>
{
    return Results.Ok(new
    {
        Source = "Original",
        Timestamp = DateTime.Now,
        Message = "Respuesta desde AccountService (Minimal API)"
    });
});

app.Run("http://localhost:6001");
