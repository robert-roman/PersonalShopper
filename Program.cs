using PersonalShopper.DAL.Models.Constants;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.AuthWrapperRepository;
using PersonalShopper.DAL.Seeders;
using PersonalShopper.DAL.Utils;
using PersonalShopper.DAL;
using PersonalShopper.Repositories.UnitOfWork;
using PersonalShopper.Repositories.CartRepository;
using PersonalShopper.Services;
using PersonalShopper.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using PersonalShopper.Services.CartProductService;
using PersonalShopper.Services.OrderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Config for EFCore Identity
builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserRoleType.Admin, policy => policy.RequireRole(UserRoleType.Admin));
    options.AddPolicy(UserRoleType.User, policy => policy.RequireRole(UserRoleType.User));
});

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom secret key for auth")),
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents()
    {
        OnTokenValidated = SessionTokenValidator.ValidateSessionToken
    };
});

// Services for AuthWrapper and Services folders
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthWrapperRepository, AuthWrapperRepository>();
builder.Services.AddScoped<InitialSeed>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICartService, CartService>();
//builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


string constr = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(constr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var userRoles = scope.ServiceProvider.GetRequiredService<InitialSeed>();

    await userRoles.SeedRoles();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
