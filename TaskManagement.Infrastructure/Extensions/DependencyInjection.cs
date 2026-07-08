namespace TaskManagement.Infrastructure.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Authentication;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        #region Authentication

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var secret = configuration["Jwt:Secret"];
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];

                if (string.IsNullOrEmpty(secret))
                {
                    throw new Exception("JWT Secret is missing from configuration.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });

        #endregion

        #region Repositories

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<DatabaseSeeder>();

        #endregion

        #region Background Processing

        services.AddSingleton<ITaskProcessingQueue, Services.TaskProcessingQueue>();
        services.AddHostedService<Services.TaskProcessingBackgroundWorker>();

        #endregion

        #region Caching

        var redisSettings = new Configuration.RedisSettings();
        configuration.GetSection(TaskManagement.Infrastructure.Configuration.RedisSettings.SectionName).Bind(redisSettings);
        
        services.Configure<Configuration.RedisSettings>(
            configuration.GetSection(TaskManagement.Infrastructure.Configuration.RedisSettings.SectionName));

        if (!string.IsNullOrEmpty(redisSettings.ConnectionString))
        {
            services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(
                StackExchange.Redis.ConnectionMultiplexer.Connect(redisSettings.ConnectionString));
            services.AddScoped<ICacheService, Services.RedisCacheService>();
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings.ConnectionString;
            options.InstanceName = "TaskManagement_";
        });

        #endregion

        return services;
    }
}
