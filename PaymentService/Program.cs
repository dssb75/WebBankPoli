using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Clave secreta para firmar el token
var jwtKey = "CLAVE_SECRETA_SUPER_SEGURA_12345"; // mínimo 16 caracteres
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

// ⚙️ Agregar autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

// ⚙️ 👇 ESTA LÍNEA ES LA QUE FALTABA
builder.Services.AddAuthorization();

var app = builder.Build();

// 🧩 Middleware de autenticación/autorización
app.UseAuthentication();
app.UseAuthorization();

// Endpoint público → genera token
app.MapPost("/login", (string user, string password) =>
{
    if (user == "admin" && password == "1234")
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user),
            new Claim(ClaimTypes.Role, "Administrador")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256
            )
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = jwt });
    }

    return Results.Unauthorized();
});

// Endpoint protegido con JWT
app.MapGet("/payments/status", () =>
{
    return Results.Ok(new
    {
        Service = "PaymentService",
        Message = "Estado de pagos en WebBankPoli",
        Payments = new[] { "Pago de Luz", "Pago de Agua" }
    });
}).RequireAuthorization();

// Escucha en el puerto 6002
app.Run("http://localhost:6002");
