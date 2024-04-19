using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class GrpcPathMiddleware
{
    private readonly RequestDelegate _next;

    public GrpcPathMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/grpc"))
        {
            // Here you can modify the request or set context items
            context.Items["IsGrpcRequest"] = true;
        }

        // Continue processing
        await _next(context);
    }
}
