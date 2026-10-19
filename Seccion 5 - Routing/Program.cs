var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.UseRouting();
Dictionary<int, string> countries = new Dictionary<int, string>
{
    { 1, "United States" },
    { 2, "Canada" },
    { 3, "United Kingdom" },
    { 4, "India" },
    { 5, "Japan" }
};

app.UseEndpoints(endpoints =>
{
    // 1. Endpoint para listar todos los países (/countries)
    endpoints.MapGet("/countries", async context =>
    {
        var response = string.Empty;
        int index = 1;
        foreach (var country in countries)
        {
            response += $"{index}. {country.Key}, {country.Value}\n";
            index++;
        }

        context.Response.StatusCode = 200;
        await context.Response.WriteAsync(response);
    });

    // 2. Endpoint para buscar un país por ID con restricciones de ruta
    // Usamos restricciones: {countryID:int} y combinamos con la lógica de rangos.
    endpoints.MapGet("/countries/{countryID:int}", async context =>
        {
            string? countryIdStr = context.Request.RouteValues["countryID"]?.ToString();

            if (int.TryParse(countryIdStr, out int countryID))
            {
                // Ejemplo 3 parte 2: Si el ID es mayor a 100 (y está en el rango permitido por la constraint general, lo validamos aquí)
                if (countryID > 100)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("The CountryID should be between 1 and 100");
                    return; 
    
                }

                // Buscar en el diccionario
                if (countries.ContainsKey(countryID))
                {
                    string countryName = countries[countryID];
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync(countryName);
                }
                else
                {
                    // Ejemplo 3 parte 1: Si el ID no existe entre 1 y 5 (fuera de rango pero <= 100)
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("[No Country]");
                }
            }

        });
});
app.Run();
