using BAL.Infrastructure;
using BAL.Services;
using DAL.Database;
using DAL.Repository.Person;
using DAL.Repository.PhoneNumber;
using DAL.Repository.RelatedPerson;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhysicalPersonsApp.ActionFilters;
using PhysicalPersonsApp.Helpers;
using PhysicalPersonsApp.Middlewares;
using Shared.Models.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalActionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("PhysicalPersonsApp")));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("PhysicalPersonsApp")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPersonRepository, SqlPersonRepositoryDapper>();
builder.Services.AddScoped<IRelatedPersonRepository, SqlRelatedPersonRepositoryDapper>();
builder.Services.AddScoped<IPhoneNumberRepository, SqlPhoneNumberRepositoryDapper>();
builder.Services.AddScoped<IFileService>(_ => new FileService(builder.Environment.ContentRootPath));
builder.Services.AddScoped<IFileFactory, FileFactory>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddScoped<IRelatedPersonService, RelatedPersonService>();
builder.Services.AddTransient(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    return new SqlConnection(connectionString);
});


builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: "ka-GE", uiCulture: "ka-GE");
    });

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("User", policy => policy.RequireRole("user"));
});


var app = builder.Build();

app.UseMiddleware<LocalizationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LogUnexpectedErrorMiddleware>();

app.MapControllers();

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions!.Value);

app.Run();
