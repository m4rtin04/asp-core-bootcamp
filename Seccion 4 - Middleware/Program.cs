using Seccion_4___Middleware.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseLoginMiddleware();

app.Run(async (context) =>
{
    await context.Response.WriteAsync("No response");
});

app.Run();
