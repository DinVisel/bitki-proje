using Bitki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register IDbConnectionFactory
builder.Services.AddSingleton<Bitki.Core.Interfaces.IDbConnectionFactory, Bitki.Infrastructure.Data.DbConnectionFactory>();

// Register Repositories and Services
// Bitki
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.IBitkiRepository, Bitki.Infrastructure.Repositories.BitkiRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IBitkiService, Bitki.Infrastructure.Services.BitkiService>();

// Auth
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Auth.IAuthUserRepository, Bitki.Infrastructure.Repositories.Auth.AuthUserRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IAuthService, Bitki.Infrastructure.Services.AuthService>();

// System
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.System.IDjangoAdminLogRepository, Bitki.Infrastructure.Repositories.System.DjangoAdminLogRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.ISystemService, Bitki.Infrastructure.Services.SystemService>();

// Taxonomy
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Taxonomy.IFamilyaRepository, Bitki.Infrastructure.Repositories.Taxonomy.FamilyaRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Taxonomy.IGenusRepository, Bitki.Infrastructure.Repositories.Taxonomy.GenusRepository>();

// MasterData
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.MasterData.IUlkeRepository, Bitki.Infrastructure.Repositories.MasterData.UlkeRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.MasterData.ISehirRepository, Bitki.Infrastructure.Repositories.MasterData.SehirRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.MasterData.IIlceRepository, Bitki.Infrastructure.Repositories.MasterData.IlceRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.MasterData.IKullanimRepository, Bitki.Infrastructure.Repositories.MasterData.KullanimRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.MasterData.IKisilerRepository, Bitki.Infrastructure.Repositories.MasterData.KisilerRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IMasterDataService, Bitki.Infrastructure.Services.MasterDataService>();

// Aktivite
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IAktiviteService, Bitki.Infrastructure.Services.AktiviteService>();

// Etnobotanik
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Etnobotanik.IEtnobitkilitRepository, Bitki.Infrastructure.Repositories.Etnobotanik.EtnobitkilitRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Etnobotanik.IEtnokullanimRepository, Bitki.Infrastructure.Repositories.Etnobotanik.EtnokullanimRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Etnobotanik.IEtnolokaliteRepository, Bitki.Infrastructure.Repositories.Etnobotanik.EtnolokaliteRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IEtnobotanikService, Bitki.Infrastructure.Services.EtnobotanikService>();

// Ucuyag
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Ucuyag.IUcuyagRepository, Bitki.Infrastructure.Repositories.Ucuyag.UcuyagRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IUcuyagService, Bitki.Infrastructure.Services.UcuyagService>();

// Literatur
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Literatur.ILiteraturRepository, Bitki.Infrastructure.Repositories.Literatur.LiteraturRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.ILiteraturService, Bitki.Infrastructure.Services.LiteraturService>();

// Ozellik
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Ozellik.IOzellikRepository, Bitki.Infrastructure.Repositories.Ozellik.OzellikRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IOzellikService, Bitki.Infrastructure.Services.OzellikService>();

// Rapor
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Rapor.IRaporRepository, Bitki.Infrastructure.Repositories.Rapor.RaporRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IRaporService, Bitki.Infrastructure.Services.RaporService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health/db", async (AppDbContext db) =>
{
    try
    {
        await db.Database.ExecuteSqlRawAsync("SELECT 1");
        return Results.Ok("Database connection is healthy");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database connection failed: {ex.Message}");
    }
});

app.Run();
