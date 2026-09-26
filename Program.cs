using System.Data.Common;
using System.Security.Claims;
using ContractTracking.Data;
using ContractTracking.Models;
using ContractTracking.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
builder.Services.AddHostedService<ContractExpiryAlertService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "ContractTrackingAuth";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.EnsureCreated();
        db.Database.ExecuteSqlRaw(
            "IF COL_LENGTH(N'dbo.Contratos', N'Gerencia') IS NULL " +
            "ALTER TABLE dbo.Contratos ADD Gerencia nvarchar(200) NOT NULL CONSTRAINT DF_Contratos_Gerencia DEFAULT N'' WITH VALUES; " +
            "IF COL_LENGTH(N'dbo.Contratos', N'TipoContrato') IS NULL " +
            "ALTER TABLE dbo.Contratos ADD TipoContrato nvarchar(100) NOT NULL CONSTRAINT DF_Contratos_TipoContrato DEFAULT N'No especificado' WITH VALUES; " +
            "IF OBJECT_ID(N'dbo.Adendas', N'U') IS NULL " +
            "CREATE TABLE dbo.Adendas (Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ContratoId int NOT NULL, NumeroAdenda nvarchar(50) NOT NULL, TipoAdenda nvarchar(100) NOT NULL, Descripcion nvarchar(max) NOT NULL, FechaAdenda date NOT NULL, MontoAdicional decimal(18,2) NOT NULL, DiasAmpliacion int NOT NULL, CONSTRAINT FK_Adendas_Contratos_ContratoId FOREIGN KEY (ContratoId) REFERENCES dbo.Contratos(Id) ON DELETE CASCADE);");

        if (!db.Usuarios.Any())
        {
            db.Usuarios.AddRange(
                new Usuario
                {
                    Username = "admin",
                    Password = "admin123",
                    Role = "Administrador",
                    Dni = "12345678",
                    Nombre = "Administrador Principal",
                    Telefono = "3000000000",
                    Correo = "admin@contratos.com"
                },
                new Usuario
                {
                    Username = "seguimiento",
                    Password = "seg123",
                    Role = "Seguimiento",
                    Dni = "87654321",
                    Nombre = "Usuario Seguimiento",
                    Telefono = "3010000000",
                    Correo = builder.Configuration["UsuarioSeguimiento:Correo"] ?? "seguimiento@contratos.com"
                });
            db.SaveChanges();
        }

        if (!db.Contratos.Any())
        {
            db.Contratos.AddRange(
                new Contrato
                {
                    NumeroContrato = "CT-001",
                    TipoContrato = "Servicios",
                    Contratista = "ACME",
                    Objeto = "Servicio de mantenimiento",
                    FechaInicio = DateTime.Today.AddDays(-10),
                    FechaFin = DateTime.Today.AddDays(15),
                    Valor = 150000m,
                    Estado = "Activo",
                    Observaciones = "Contrato en ejecución"
                },
                new Contrato
                {
                    NumeroContrato = "CT-002",
                    TipoContrato = "Consultoría",
                    Contratista = "Beta SAS",
                    Objeto = "Consultoría",
                    FechaInicio = DateTime.Today.AddDays(-30),
                    FechaFin = DateTime.Today.AddDays(5),
                    Valor = 90000m,
                    Estado = "Activo",
                    Observaciones = "Próximo a vencer"
                });

            db.SaveChanges();
        }
    }
    catch (DbException exception)
    {
        app.Logger.LogError(exception, "No se pudo conectar con la base de datos durante el inicio.");
    }
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
