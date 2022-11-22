using Capa_Datos;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Capa_Entidad;
using CalendarBackend;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Configuration.AddJsonFile("appsettings.json");
var code = builder.Configuration.GetSection("AppSettings:Dev").Get<Dev>();
var keyBytes = Encoding.UTF8.GetBytes(code.Token);

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://calendarapp-ricardo.netlify.app/")
            .WithOrigins("https://calendarapp-ricardo.netlify.app")
            .WithOrigins("http://localhost:5173/")
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();
builder.Services.AddScoped<IBdUsuario, SbdUsuario>();
builder.Services.AddScoped<IDbEvento, SdbEvento>();
builder.Services.AddScoped<ICreateHash, SCreateHash>();
builder.Services.AddScoped<INrUsuario, SnrUsuario>();
builder.Services.AddScoped<ITokenCreate, STokenCreate>();
builder.Services.AddScoped<IValidarCampos, SValidarCampos>();
builder.Services.AddScoped<IValidarUsuario, SValidarUsuario>();
builder.Services.AddScoped<IValidarEvento, SValidarEvento>();
builder.Services.AddScoped<INrEvento, SnrEvento>();
builder.Services.AddScoped<IDbNotificacion, SdbNotificacion>();
builder.Services.AddScoped<INrNotificacion, SnrNotificacion>();
builder.Services.AddScoped<ISendEmail, SSendEmail>();
builder.Services.AddScoped<ICOSetting, ScoSetting>();
builder.Services.AddScoped<IBdAction, SbdAction>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
