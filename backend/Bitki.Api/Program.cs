using Bitki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey!);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"]
    };
});

builder.Services.AddAuthorization();

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
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteBitkilitRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteBitkilitRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteCalismaRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteCalismaRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteEtkiRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteEtkiRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteLokaliteRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteLokaliteRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteSaflastirmaRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteSaflastirmaRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteTestYeriRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteTestYeriRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Aktivite.IAktiviteYontemRepository, Bitki.Infrastructure.Repositories.Aktivite.AktiviteYontemRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.IAktiviteService, Bitki.Infrastructure.Services.AktiviteService>();

// Compounds (Bilesikler)
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Compounds.IBilesiklerRepository, Bitki.Infrastructure.Repositories.Compounds.BilesiklerRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Compounds.IBitkiBilesikRepository, Bitki.Infrastructure.Repositories.Compounds.BitkiBilesikRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Compounds.ILiteraturBilesikRepository, Bitki.Infrastructure.Repositories.Compounds.LiteraturBilesikRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Compounds.IUcucuYagBilesikRepository, Bitki.Infrastructure.Repositories.Compounds.UcucuYagBilesikRepository>();

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
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Literatur.ILiteraturHatalariRepository, Bitki.Infrastructure.Repositories.Literatur.LiteraturHatalariRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Literatur.ILiteraturKonularRepository, Bitki.Infrastructure.Repositories.Literatur.LiteraturKonularRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Literatur.IOtorRepository, Bitki.Infrastructure.Repositories.Literatur.OtorRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Services.ILiteraturService, Bitki.Infrastructure.Services.LiteraturService>();

// Cleanup (Batch 6)
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Cleanup.IBitkiResimleriRepository, Bitki.Infrastructure.Repositories.Cleanup.BitkiResimleriRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Cleanup.IEtkilerRepository, Bitki.Infrastructure.Repositories.Cleanup.EtkilerRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Cleanup.IOzellikRepository, Bitki.Infrastructure.Repositories.Cleanup.OzellikRepository>();
builder.Services.AddScoped<Bitki.Core.Interfaces.Repositories.Cleanup.IUcuyagRepository, Bitki.Infrastructure.Repositories.Cleanup.UcuyagRepository>();

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

// Register DbSeeder
builder.Services.AddScoped<Bitki.Infrastructure.Data.DbSeeder>();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<Bitki.Infrastructure.Data.DbSeeder>();
    await seeder.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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
