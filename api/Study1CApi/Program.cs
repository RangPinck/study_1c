using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Study1CApi.Initializers;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;
using System.Text;

namespace Study1CApi;

public class Program
{
    public static async Task Main(string[] args)
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
                Description = "Enter the correct authorization token!",
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
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        builder.Services.AddScoped<IBlockRepository, BlockRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
        builder.Services.AddScoped<IPracticeRepository, PracticeRepository>();
        builder.Services.AddScoped<IStatusStudyRepository, StatusStudyRepository>();


        builder.Services.AddIdentity<AuthUser, Role>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<Role>()
        .AddEntityFrameworkStores<Study1cDbContext>()
        .AddDefaultTokenProviders();

        var validIssuer = builder.Configuration.GetValue<string>("JWT:Issuer");
        var validAudience = builder.Configuration.GetValue<string>("JWT:Audience");
        var symmetricSecurityKey = builder.Configuration.GetValue<string>("JWT:SigningKey");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultScheme =
            options.DefaultForbidScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
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
                    Encoding.UTF8.GetBytes(symmetricSecurityKey)
                )
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
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
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var service = scope.ServiceProvider;

            try
            {
                string adminName = builder.Configuration.GetValue<string>("StandardAdmin:Name");
                string adminLogin = builder.Configuration.GetValue<string>("StandardAdmin:Login");
                string adminPassword = builder.Configuration.GetValue<string>("StandardAdmin:Password");

                var userManager = service.GetRequiredService<UserManager<AuthUser>>();
                var roleManager = service.GetRequiredService<RoleManager<Role>>();
                var context = service.GetRequiredService<Study1cDbContext>();

                await DefaultUserInitializer.UserInitializeAsync(
                    userManager: userManager,
                    roleManager: roleManager,
                    context: context,
                    adminName: adminName,
                    adminPassword: adminPassword,
                    adminLogin: adminLogin);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

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
