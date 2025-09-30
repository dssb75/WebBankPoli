var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/payments/status", () =>
{
    return Results.Ok(new
    {
        Service = "PaymentService",
        Message = "Estado de pagos en WebBankPoli",
        Payments = new[] { "Pago de Luz", "Pago de Agua" }
    });
});


// Escucha en el puerto 6002
app.Run("http://localhost:6002");
