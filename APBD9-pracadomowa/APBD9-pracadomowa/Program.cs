using System.Text;
using APBD9_pracadomowa.Context;
using APBD9_pracadomowa.Middlewares;
using APBD9_pracadomowa.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<APBDContext>(options => 
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("Default")));

builder.Services.AddScoped<IDbServices, DbServices>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,   //by who
        ValidateAudience = true, //for whom
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        ValidIssuer = "https://localhost:5001", //should come from configuration
        ValidAudience = "https://localhost:5001", //should come from configuration
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tajnyklucz"))
    };

    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }
            return Task.CompletedTask;
        }
    };
}).AddJwtBearer("IgnoreTokenExpirationScheme",opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,   //by who
        ValidateAudience = true, //for whom
        ValidateLifetime = false,
        ClockSkew = TimeSpan.FromMinutes(2),
        ValidIssuer = "https://localhost:5001", //should come from configuration
        ValidAudience = "https://localhost:5001", //should come from configuration
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

