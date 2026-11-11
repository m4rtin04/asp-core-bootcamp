# Sección 20 - Logging con Serilog

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que agrega **registro de eventos (logging)** a la Stocks App mediante **Serilog**, usando la abstracción `ILogger<T>` en controladores y servicios.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de la Sección 19. La funcionalidad no cambia: la aplicación sigue mostrando el precio en vivo mediante WebSocket, permite explorar acciones populares, crear órdenes de compra y venta guardadas en SQL Server y exportarlas a PDF. Lo que se agrega es la capacidad de **registrar lo que sucede en la aplicación**, tanto en la consola como en archivos de log.

Los principales cambios incluyen:

- Instalación del paquete **Serilog.AspNetCore**, que incluye los *sinks* de consola y archivo y el soporte para leer la configuración desde `appsettings.json`.
- Reemplazo de la sección `Logging` de `appsettings.json` por una sección `Serilog`, con nivel mínimo `Information`, *overrides* para silenciar el ruido de `Microsoft.AspNetCore` y de los comandos de EF Core, y dos destinos: consola y archivo (`logs/log-.txt`, un archivo por día).
- Integración de Serilog en `Program.cs` con `UseSerilog`, leyendo la configuración y los servicios registrados.
- Uso de `UseSerilogRequestLogging()` para registrar una línea por cada petición HTTP, ubicado después de `UseStaticFiles()` para no registrar las peticiones a archivos estáticos.
- Inyección de `ILogger<T>` en `TradeController`, `StocksController` y `StocksService`. Al usar la interfaz estándar de .NET, el código de la aplicación no depende de Serilog directamente.
- Registros con distintos niveles: `LogInformation` (solicitudes y órdenes creadas), `LogWarning` (datos de la acción no encontrados y órdenes rechazadas por validación) y `LogDebug` (cantidad de acciones devueltas por `Explore`).
- Uso de **plantillas de mensaje** (`{StockSymbol}`) en lugar de interpolación de cadenas, para conservar el registro estructurado.
- Actualización de las pruebas unitarias para pasar `NullLogger<T>.Instance` a los nuevos constructores.
- Exclusión de la carpeta `logs/` del repositorio mediante `.gitignore`.

> **Seguridad:** la URL de Finnhub contiene el token (`token=...`), por eso **no se registran URLs** en `FinnhubRepository`. Si se agregan logs en esa capa, deben incluir solo datos como el símbolo de la acción, para que el secreto no termine en un archivo de log.

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura de **4 capas** de la Sección 19. El logging es transversal y se agrega en las capas de UI y BLL:

| Capa | En el proyecto | Cambio en esta sección |
|------|----------------|------------------------|
| **UI** | Controllers, Views y View Components | `TradeController` y `StocksController` reciben `ILogger<T>`. |
| **BLL** | `FinnhubService` y `StocksService` | `StocksService` recibe `ILogger<T>`. |
| **RL** | `FinnhubRepository` y `StocksRepository` | Sin cambios. |
| **DAL** | `StockMarketDbContext` y `HttpClient` | Sin cambios. |

```
StocksApp/
├── Controllers/            → TradeController, StocksController (con ILogger)
├── DTO/
├── Entities/
├── Helpers/
├── Migrations/
├── Models/
├── Options/
├── Repositories/
├── RepositoryContracts/
├── ServiceContracts/
├── Services/               → FinnhubService, StocksService (con ILogger)
├── ViewComponents/
├── Views/
├── logs/                   → archivos de log generados (excluidos de Git)
├── appsettings.json        → sección "Serilog"
└── Program.cs              → UseSerilog y UseSerilogRequestLogging

StocksApp.Tests/
├── StocksServiceTest.cs            → usa NullLogger<StocksService>
├── StocksControllerTest.cs         → usa NullLogger<StocksController>
├── CustomWebApplicationFactory.cs
└── TradeControllerIntegrationTest.cs
```

---

## 📝 Configuración de Serilog

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "WriteTo": [
    { "Name": "Console" },
    {
      "Name": "File",
      "Args": {
        "path": "logs/log-.txt",
        "rollingInterval": "Day"
      }
    }
  ]
}
```

- **`Default: Information`:** registra los mensajes de nivel `Information`, `Warning` y `Error`; los `Debug` quedan ocultos salvo que se cambie el nivel.
- **`Override`:** reduce el ruido del framework y de EF Core, que generan una gran cantidad de mensajes.
- **`rollingInterval: Day`:** crea un archivo nuevo por día (por ejemplo `log-20261111.txt`).

En `Program.cs`:

```csharp
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

// ...
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
```

---

## 🧪 Pruebas

Las pruebas de la Sección 20 se mantienen, con un único cambio: como los constructores de `StocksService` y `StocksController` ahora piden un `ILogger<T>`, se les pasa `NullLogger<T>.Instance`, un logger que no escribe nada.

- **Pruebas unitarias del servicio** (`StocksService`) con xUnit, Moq, AutoFixture y FluentAssertions.
- **Prueba unitaria del controlador** (`StocksController.Explore()`).
- **Prueba de integración** de la ruta `/Trade/Index/MSFT` con `WebApplicationFactory` y un repositorio de Finnhub falso. No requiere cambios.

```bash
dotnet test
```

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC y arquitectura de 4 capas (UI, BLL, RL, DAL).
- **Serilog** con sinks de consola y archivo, configurado desde `appsettings.json`.
- Abstracción `ILogger<T>` de `Microsoft.Extensions.Logging` e inyección de dependencias.
- Niveles de log (`Debug`, `Information`, `Warning`) y niveles mínimos con `Override`.
- Registro estructurado con plantillas de mensaje.
- Registro de peticiones HTTP con `UseSerilogRequestLogging()`.
- Buenas prácticas de seguridad: no registrar secretos ni URLs con tokens.
- Patrón Repositorio, View Components y Options Pattern.
- Entity Framework Core con SQL Server y migraciones.
- Generación de PDF con Rotativa.
- Pruebas unitarias con Moq, AutoFixture y FluentAssertions, y pruebas de integración.
- WebSockets y JavaScript para actualizar el precio en tiempo real.

---

## 🔑 Configuración previa

**1. Token de Finnhub.** El proyecto necesita un token de [Finnhub.io](https://finnhub.io/). Registrate para obtener el tuyo y guardalo en User Secrets desde el directorio del proyecto web:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

**2. Configuración en `appsettings.json`:** además de la sección `Serilog` descrita arriba, se mantienen `TradingOptions` y `ConnectionStrings`:

```json
"TradingOptions": {
  "DefaultStockSymbol": "MSFT",
  "DefaultOrderQuantity": 100,
  "Top25PopularStocks": "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO"
},
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StockMarketDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
}
```

> `(localdb)\MSSQLLocalDB` solo existe en Windows. En macOS o Linux hay que usar SQL Server en Docker y adaptar la cadena de conexión.

**3. Base de datos.** Desde el directorio del proyecto web:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

Esta sección no agrega migraciones nuevas.

**4. Rotativa.** Colocar `wkhtmltopdf.exe` en `wwwroot/Rotativa/`.

**5. `.gitignore`.** Agregar la carpeta de logs para no subir los archivos generados:

```
logs/
```

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución desde el archivo `.sln` en Visual Studio, o ejecutar desde una terminal situada en el directorio del proyecto web:

```bash
dotnet run
```

Para verificar el logging:

1. En la consola aparecen mensajes como `Trade/Index requested for MSFT` y una línea por cada petición HTTP.
2. Al crear una orden de compra se registran `Buy order created` y `Buy order ... saved`.
3. En la carpeta `logs/` se genera el archivo del día con el mismo contenido.

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible, y las órdenes se crean con ese valor.
