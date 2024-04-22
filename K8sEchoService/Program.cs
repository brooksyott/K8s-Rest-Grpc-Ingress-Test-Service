
namespace K8sEchoService;
using Serilog;
using K8sEchoService.Echo;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Microsoft.AspNetCore.Rewrite;

// using K8sEchoService.Greeter;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();

        builder.Services.AddControllers();

        builder.Logging.ClearProviders();
        var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                    .Build();


        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(hostContext.Configuration);
        });

        builder.Services.AddScoped<IEchoService, EchoService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // ==================================================================================
        // The below code is added to handle the gRPC with a path, ie: localhost:50051/grpc
        // Remove it for normal grpc services to be at the root "/", localhost:50051
        app.UseRouting();
        var rewriteOptions = new RewriteOptions()
           .AddRewrite("grpc/(.*)", "$1", skipRemainingRules: false);
        app.UseRewriter(rewriteOptions);
        // ==================================================================================

        app.MapGrpcReflectionService();
        // app.UseHttpsRedirection();
        // app.UseMiddleware<GrpcPathMiddleware>();

        app.UseAuthorization();

        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<EchoGrpcService>();

        app.MapControllers();

        app.Run();
    }
}
