global using Blog;
global using Blog.Data;
global using Blog.Extensions;
global using Blog.Models;
global using Blog.services;
global using Blog.ViewModels;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.IdentityModel.Tokens;
global using System.Collections.Generic;
global using System.IO.Compression;
global using System.Text;
global using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMvs(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();

app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smpt").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

void ConfigureMvs(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(o => { o.Providers.Add<GzipCompressionProvider>(); });

    builder.Services.Configure<GzipCompressionProviderOptions>(o => { o.Level = CompressionLevel.Optimal; });

    builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(j =>
    {
        j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //cancela o looping do json
        j.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault; //Ignora os objetos nulos
    });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<BlogDataContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddTransient<TokenService>();
}