# Sección 24 - Clean Architecture (Stocks App)
> **Nota:** Los nombres y las carpetas se refieren a StockMarket y en el readme habla de StocksApp, son lo mismo, hubo un pequeño error al nombrar los nombres solamente
> 
Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, centrado en reorganizar la solución con **Clean Architecture**. La aplicación se ve y funciona igual que antes; lo que cambia es **en qué proyecto vive cada clase y quién puede referenciar a quién**.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de la Sección 23. La funcionalidad no cambia: precio en vivo con WebSocket, exploración de acciones populares, órdenes de compra y venta en SQL Server, exportación a PDF, logging con Serilog, filtro de validación, manejo de errores y principios SOLID.

Hasta ahora los controladores, los servicios, los repositorios y el `DbContext` convivían en un solo proyecto. Con Clean Architecture se separan en tres proyectos, y las dependencias apuntan siempre hacia adentro:

```
UI  ──────►  Core (con Domain adentro)  ◄──────  Infrastructure
```

Los principales cambios incluyen:

- Creación del proyecto **StocksApp.Core** (biblioteca de clases) con la lógica de negocio, los DTOs y el dominio. No referencia a ningún otro proyecto y no conoce EF Core, `HttpClient` ni MVC.
- Creación del proyecto **StocksApp.Infrastructure** (biblioteca de clases) con el `DbContext`, los repositorios y las llamadas a la API externa. Referencia a Core e implementa los contratos que Core define.
- El proyecto web original (`StocksApp`) pasa a ser la capa **UI** y referencia a Core y a Infrastructure (este último solo para registrarlo en el contenedor de dependencias).
- Los contratos de repositorio (`IFinnhubRepository`, `IStocksRepository`) se **definen** en Core, dentro de `Domain`, pero se **implementan** en Infrastructure. Es la inversión de dependencias aplicada a las capas.
- Actualización de los `namespace` y los `using` de toda la solución.
- Creación de `ConfigureServicesExtension`, un método de extensión que concentra el registro de servicios, para que `Program.cs` quede limpio.
- Reinicio de las migraciones: al cambiar el `DbContext` de proyecto y de namespace, se creó una migración inicial nueva dentro de Infrastructure, usando una base de datos nueva.
- Actualización de los `using` de los proyectos de pruebas.

---

## 🏗️ Arquitectura del Proyecto

| Capa | Proyecto | Contenido |
|------|----------|-----------|
| **Core / Domain** | `StocksApp.Core` | `BuyOrder`, `SellOrder` (entidades) e `IFinnhubRepository`, `IStocksRepository` (contratos de repositorio). |
| **Core** | `StocksApp.Core` | DTOs, interfaces de servicio, servicios, `FinnhubException` y helpers de validación. |
| **Infrastructure** | `StocksApp.Infrastructure` | `StockMarketDbContext`, `FinnhubRepository`, `StocksRepository` y migraciones. |
| **UI** | `StocksApp` | Controladores, filtros, middleware, view models, opciones, View Components, vistas y `Program.cs`. |

Reglas de referencia entre proyectos:

- **Core** no tiene ninguna referencia a proyectos.
- **Infrastructure** referencia a **Core**.
- **UI** referencia a **Core** e **Infrastructure**.
- **Tests** referencia a Core y a UI.

Esto permite, por ejemplo, cambiar SQL Server por otra base de datos o Finnhub por otra API tocando solo Infrastructure.

```
StocksApp.sln
│
├── StocksApp.Core/
│   ├── Domain/
│   │   ├── Entities/               → BuyOrder, SellOrder
│   │   └── RepositoryContracts/    → IFinnhubRepository, IStocksRepository
│   ├── DTO/                        → OrderRequest, BuyOrderRequest, SellOrderRequest,
│   │                                 BuyOrderResponse, SellOrderResponse
│   ├── Exceptions/                 → FinnhubException
│   ├── Helpers/                    → FinnhubCall, ValidationHelper, MinimumDateAttribute
│   ├── ServiceContracts/           → IFinnhubCompanyProfileService, IFinnhubStockPriceQuoteService,
│   │                                 IFinnhubStocksService, IFinnhubSearchStocksService, IStocksService
│   └── Services/                   → servicios de Finnhub y StocksService
│
├── StocksApp.Infrastructure/
│   ├── DatabaseContext/                       → StockMarketDbContext
│   ├── Repositories/               → FinnhubRepository, StocksRepository
│   └── Migrations/
│
├── StocksApp/                      (UI)
│   ├── Controllers/                → TradeController, StocksController, HomeController
│   ├── Filters/                    → CreateOrderActionFilter
│   ├── Middlewares/                → ExceptionHandlingMiddleware
│   ├── Models/                     → StockTrade, Stock, StockDetails, Orders
│   ├── Options/                    → TradingOptions
│   ├── StartupExtensions/          → ConfigureServicesExtension
│   ├── ViewComponents/             → SelectedStockViewComponent
│   ├── Views/
│   ├── wwwroot/
│   └── Program.cs
│
└── StocksApp.Tests/
```

`TradingOptions` se mantiene en la UI porque solo la usan los controladores.

---

## 🧩 Registro de servicios

```csharp
public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
{
    services.AddControllersWithViews();
    services.AddHttpClient();

    services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));

    services.AddDbContext<StockMarketDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    // Repositorios
    services.AddScoped<IFinnhubRepository, FinnhubRepository>();
    services.AddScoped<IStocksRepository, StocksRepository>();

    // Servicios
    services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
    services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
    services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
    services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
    services.AddScoped<IStocksService, StocksService>();

    return services;
}
```

En `Program.cs` se invoca con `builder.Services.ConfigureServices(builder.Configuration);`.

---

## 🧪 Pruebas

Las pruebas no cambian su lógica, solo sus `using`, que ahora apuntan a los nuevos namespaces (`StocksApp.Core.Domain.Entities`, `StocksApp.Core.DTO`, `StocksApp.Core.ServiceContracts`, etc.). El proyecto de pruebas referencia a Core y a la UI.

- Pruebas unitarias de `StocksService` con xUnit, Moq, AutoFixture y FluentAssertions.
- Prueba unitaria de `StocksController.Explore()`.
- Prueba de integración de `/Trade/Index/MSFT`, con un repositorio de Finnhub falso (`FakeFinnhubRepository`) registrado en `CustomWebApplicationFactory`.

```bash
dotnet test
```

---

## 🛠️ Tecnologías y conceptos aplicados

- Clean Architecture: capas Core, Domain, Infrastructure y UI.
- Regla de dependencias: las referencias apuntan hacia adentro.
- Inversión de dependencias a nivel de proyecto (contratos en Core, implementaciones en Infrastructure).
- Bibliotecas de clases y referencias entre proyectos.
- Método de extensión para configurar servicios.
- Migraciones de Entity Framework Core en un proyecto distinto al de inicio (`--project` y `--startup-project`).
- Principios SOLID, patrón Repositorio, excepciones personalizadas, middleware y filtros.
- Logging con Serilog, generación de PDF con Rotativa y WebSockets.
- Pruebas unitarias y de integración.

---

## 🔑 Configuración previa

**1. Token de Finnhub.** Registrate en [Finnhub.io](https://finnhub.io/) y guardá el token en User Secrets desde el directorio del proyecto web (`StocksApp`):

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

**2. `appsettings.json`:** se mantienen `TradingOptions`, `ConnectionStrings` y `Serilog`. Se recomienda usar un nombre de base de datos nuevo en `Initial Catalog` para no mezclarla con la de la sección anterior.

**3. Base de datos.** Desde la carpeta de la solución:

```bash
dotnet ef migrations add Initial --project StocksApp.Infrastructure --startup-project StocksApp
dotnet ef database update --project StocksApp.Infrastructure --startup-project StocksApp
```

El proyecto web necesita el paquete `Microsoft.EntityFrameworkCore.Design`.

**4. Rotativa.** Colocar `wkhtmltopdf.exe` en `wwwroot/Rotativa/` del proyecto web.

**5. `.gitignore`.** Mantener la línea `logs/`.

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución desde el archivo `.sln` en Visual Studio, con `StocksApp` como proyecto de inicio, o desde una terminal:

```bash
dotnet run --project StocksApp
```

Para comprobar el manejo de errores, ejecutar en otro entorno:

```bash
dotnet run --project StocksApp --environment Production
```

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible.
