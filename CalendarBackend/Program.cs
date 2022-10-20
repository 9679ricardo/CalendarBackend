using Capa_Datos;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Capa_Entidad;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Configuration.AddJsonFile("appsettings.json");
var code = builder.Configuration.GetSection("AppSettings:Dev").Get<Dev>();
var keyBytes = Encoding.UTF8.GetBytes(code.Token);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
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
builder.Services.AddScoped<IBD_Usuario, SBD_Usuario>();
builder.Services.AddScoped<IDB_Evento, SDB_Evento>();
builder.Services.AddScoped<ICreateHash, SCreateHash>();
builder.Services.AddScoped<INR_Usuario, SNR_Usuario>();
builder.Services.AddScoped<ITokenCreate, STokenCreate>();
builder.Services.AddScoped<IValidarCampos, SValidarCampos>();
builder.Services.AddScoped<IValidarUsuario, SValidarUsuario>();
builder.Services.AddScoped<IValidarEvento, SValidarEvento>();
builder.Services.AddScoped<INR_Evento, SNR_Evento>();
var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
