using ApiKeyAuthentication.Authentication;
using Microsoft.OpenApi.Models;

namespace ApiKeyAuthentication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers(/*x => x.Filters.Add<ApiKeyAuthFilter>()*/);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "The API Key to access the API",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
            {
                {scheme, new List<string>() }
            };
            c.AddSecurityRequirement(requirement);
        });

        builder.Services.AddScoped<ApiKeyAuthFilter>();

        var app = builder.Build();

        // Example of adding the endpoint filter
        app.MapGet("/", () => "Welcome mini!")
            .AddEndpointFilter<ApiKeyEndpointFilter>();

        // Also works for groups
        //var group = app.MapGroup("mygroup").AddEndpointFilter<ApiKeyEndpointFilter>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //app.UseMiddleware<ApiAuthMiddleware>();

        app.UseAuthorization();

        app.UseRouting();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        //app.MapControllers();

        app.Run();
    }
}