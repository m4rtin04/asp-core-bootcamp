using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Seccion_4___Middleware.CustomMiddleware
{
    
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if(httpContext.Request.Path=="/" && httpContext.Request.Method=="POST")
            {
                // Leer el cuerpo de la solicitud como texto (form-urlencoded)
                StreamReader reader = new StreamReader(httpContext.Request.Body);
                string body = await reader.ReadToEndAsync();

                // Analizar los datos del cuerpo como si fueran query parameters
                Dictionary<string, StringValues> queryDic = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

                string? email = null, password = null;
                bool emailSubmitted = false;
                bool passwordSubmitted = false;

                // Verificar si se envió el campo 'email'
                if (httpContext.Request.Query.ContainsKey("email")){
                    emailSubmitted = true;
                    email = httpContext.Request.Query["email"][0];
                }

                // Verificar si se envió el campo 'password'
                if (httpContext.Request.Query.ContainsKey("password")){
                    passwordSubmitted = true;
                    password = httpContext.Request.Query["password"][0];
                }

                // Lista para acumular los errores si faltan campos
                List<string> errors = new List<string>();

                if(!emailSubmitted || string.IsNullOrEmpty(email))
                {
                    errors.Add("Invalid input for 'email'");
                }

                if(!passwordSubmitted || string.IsNullOrEmpty(password))
                {
                    errors.Add("Invalid input for 'password'");
                }

                //Si hay errores, devolver un 400 Bad Request con los mensajes de error
                if (errors.Count > 0)
                {
                    httpContext.Response.StatusCode = 400; // Bad Request
                    await httpContext.Response.WriteAsync(string.Join("\n", errors));
                    return;
                }

                // Validar credenciales si ambos campos fueron enviados
                string validEmail = "admin@example.com";
                string validPassword = "admin1234";

                if(email == validEmail && password == validPassword)
                {
                    httpContext.Response.StatusCode = 200; // OK
                    await httpContext.Response.WriteAsync("Login successful");
                }
                else
                {
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    await httpContext.Response.WriteAsync("Invalid credentials");
                }
            }
            else
            {
                // Si la solicitud no es un POST a la raíz, pasar al siguiente middleware
                await _next(httpContext);
            }
                
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
