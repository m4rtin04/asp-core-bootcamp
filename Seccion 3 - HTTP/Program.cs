var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.Run(async (HttpContext context) =>
{
    if (context.Request.Method == "GET" && context.Request.Path == "/")
    {
        int firstNumber = 0, secondNumber = 0;
        string? operation = null;
        long? result = null;

        // 1. Leer y validar 'firstNumber'
        if (context.Request.Query.ContainsKey("firstNumber"))
        {
            string firstNumberString = context.Request.Query["firstNumber"][0] ?? string.Empty;
            if (!string.IsNullOrEmpty(firstNumberString) && int.TryParse(firstNumberString, out firstNumber))
            {
                // Válido, continúa
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
            }
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
        }

        //Leer y validar 'secondNumber'
        if (context.Request.Query.ContainsKey("secondNumber"))
        {
            string secondNumberString = context.Request.Query["secondNumber"][0] ?? string.Empty;
            if (!string.IsNullOrEmpty(secondNumberString) && int.TryParse(secondNumberString, out secondNumber))
            {
                if (context.Response.StatusCode == 200)
                    context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
        }

        //Leer y validar 'operation'
        if (context.Request.Query.ContainsKey("operation"))
        {
            operation = Convert.ToString(context.Request.Query["operation"][0]);

            switch (operation)
            {
                case "+":
                case "add":
                    result = (long)firstNumber + secondNumber;
                    break;

                case "-":
                case "subtract":
                    result = (long)firstNumber - secondNumber;
                    break;

                case "*":
                case "multiply":
                    result = (long)firstNumber * secondNumber;
                    break;

                case "/":
                case "divide":
                    result = (secondNumber != 0) ? firstNumber / secondNumber : 0;
                    break;

                case "%":
                case "mod":
                    result = (secondNumber != 0) ? firstNumber % secondNumber : 0;
                    break;
            }

            // Si la operación matcheó con un caso válido
            if (result.HasValue)
            {
                // Solo escribe el resultado si el código de estado sigue siendo 200 (sin errores previos)
                if (context.Response.StatusCode != 400)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync(result.Value.ToString());
                }
            }
            else
            {
                if (context.Response.StatusCode == 200)
                    context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'operation'\n");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for 'operation'\n");
        }
    }
    
});
