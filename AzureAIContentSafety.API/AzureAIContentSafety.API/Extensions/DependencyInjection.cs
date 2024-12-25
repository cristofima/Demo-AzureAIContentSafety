using System.Reflection;
using AzureAIContentSafety.API.Interfaces;
using AzureAIContentSafety.API.Options;
using AzureAIContentSafety.API.Persistence;
using AzureAIContentSafety.API.Repositories;
using AzureAIContentSafety.API.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace AzureAIContentSafety.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add services to the container.
        var dbConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(dbConnectionString)
        );

        services
            .AddOptions<AzureStorageOptions>()
            .Bind(configuration.GetSection("AzureStorage"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<AzureAIContentSafetyOptions>()
            .Bind(configuration.GetSection("AzureAIContentSafety"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IAzureStorageService, AzureStorageService>();
        services.AddScoped<IAzureContentSafetyService, AzureContentSafetyService>();
        services.AddScoped<IPostRepository, PostRepository>();

        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors();

        return services;
    }
}
