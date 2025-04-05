
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;
using System.Text;

namespace Study1CApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Study 1C API",
                Version = "v1",
                Description = "API for 1C training applications.",
            });
            swagger.EnableAnnotations();
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Введите действительный токен",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });

        builder.Services.AddDbContext<Study1cDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString(nameof(Study1cDbContext))
            );
        });

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();

        builder.Services.AddIdentity<AuthUser, AuthRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
    .AddRoles<AuthRole>()
    .AddEntityFrameworkStores<Study1cDbContext>()
    .AddDefaultTokenProviders();

        var validIssuer = builder.Configuration.GetValue<string>("JWT:Issuer");
        var validAudience = builder.Configuration.GetValue<string>("JWT:Audience");
        var symmetricSecurityKey = builder.Configuration.GetValue<string>("JWT:SigningKey");

        builder.Services.AddAuthentication(options =>  // схема аутентификации - с помощью jwt-токенов
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultScheme =
            options.DefaultForbidScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => // подключение аутентификации с помощью jwt-токенов;
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                )
            };
            options.Events = new JwtBearerEvents //Для создания кастомного сообщения о том, что пользователь не авторизован
            {
                OnChallenge = async context =>
                {
                    // Call this to skip the default logic and avoid using the default response
                    context.HandleResponse();

                    var httpContext = context.HttpContext;
                    var statusCode = StatusCodes.Status401Unauthorized;

                    var routeData = httpContext.GetRouteData();
                    var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());

                    var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = factory.CreateProblemDetails(httpContext, statusCode);

                    var result = new ObjectResult(problemDetails) { StatusCode = statusCode };
                    await result.ExecuteResultAsync(actionContext);
                }
            };
        });
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        }); //для того, чтобы для каждого запроса нужна была атворизация

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Study 1C API v1");
            });
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
