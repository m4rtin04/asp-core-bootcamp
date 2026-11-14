# Sección 23 - Principios SOLID (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, centrado en **refactorizar el código existente aplicando los principios SOLID**. La aplicación se comporta igual que antes; lo que cambia es cómo está organizado el código.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de la Sección 22. La funcionalidad no cambia: precio en vivo con WebSocket, exploración de acciones populares, órdenes de compra y venta en SQL Server, exportación a PDF, logging con Serilog, filtro de validación y manejo de errores.

El problema que se corrige: `IFinnhubService` tenía cuatro métodos y todas las clases que lo usaban dependían de los cuatro, aunque cada una usara solo uno o dos.

| Clase | Usa realmente | Dependía de |
|-------|---------------|-------------|
| `StocksController` | `GetStocks()` | los 4 métodos |
| `SelectedStockViewComponent` | perfil y cotización | los 4 métodos |
| `TradeController` | perfil y cotización | los 4 métodos |

Los principales cambios incluyen:

- Eliminación de `IFinnhubService` y `FinnhubService`.
- Creación de cuatro interfaces pequeñas, cada una con un solo método: `IFinnhubCompanyProfileService`, `IFinnhubStockPriceQuoteService`, `IFinnhubStocksService` e `IFinnhubSearchStocksService`.
- Creación de sus cuatro implementaciones, cada una con una única responsabilidad y una dependencia hacia `IFinnhubRepository`.
- Creación de `FinnhubCall.Execute<T>()`, un método auxiliar que concentra el `try/catch` que envuelve las fallas en `FinnhubException`, para no repetirlo en los cuatro servicios.
- Registro de los cuatro servicios en `Program.cs`.
- Actualización de `TradeController`, `StocksController` y `SelectedStockViewComponent`, que ahora reciben únicamente los servicios que usan.
- Actualización de `StocksControllerTest`, que ahora simula (`Mock`) solo `IFinnhubStocksService`.


---

## 🧱 Principios SOLID aplicados

| Principio | Dónde se aplica |
|-----------|-----------------|
| **S** - Responsabilidad única | Cada servicio de Finnhub hace una sola cosa. El filtro valida, el controlador crea la orden, `FinnhubCall` maneja los errores. |
| **O** - Abierto/Cerrado | Para agregar un endpoint nuevo de Finnhub basta una interfaz y una clase nuevas, sin modificar las existentes. |
| **L** - Sustitución de Liskov | `BuyOrderRequest` y `SellOrderRequest` heredan de `OrderRequest` sin alterar su comportamiento. `FakeFinnhubRepository` sustituye al repositorio real en las pruebas. |
| **I** - Segregación de interfaces | Cuatro interfaces pequeñas en lugar de una de cuatro métodos. |
| **D** - Inversión de dependencias | Controladores, servicios y View Component dependen de interfaces; el contenedor de DI entrega la clase concreta. |

Ejemplo de segregación:

```csharp
public interface IFinnhubCompanyProfileService
{
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
}
```

Registro en `Program.cs`:

```csharp
builder.Services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
builder.Services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
builder.Services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
builder.Services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
```

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura de **4 capas** (UI, BLL, RL, DAL).

```
StocksApp/
├── Controllers/            → TradeController, StocksController, HomeController
├── DTO/
├── Entities/
├── Exceptions/             → FinnhubException
├── Filters/                → CreateOrderActionFilter
├── Helpers/                → FinnhubCall, ValidationHelper, MinimumDateAttribute
├── Middlewares/            → ExceptionHandlingMiddleware
├── Migrations/
├── Models/
├── Options/
├── Repositories/           → FinnhubRepository, StocksRepository
├── RepositoryContracts/    → IFinnhubRepository, IStocksRepository
├── ServiceContracts/       → IFinnhubCompanyProfileService, IFinnhubStockPriceQuoteService,
│                             IFinnhubStocksService, IFinnhubSearchStocksService, IStocksService
├── Services/               → FinnhubCompanyProfileService, FinnhubStockPriceQuoteService,
│                             FinnhubStocksService, FinnhubSearchStocksService, StocksService
├── ViewComponents/         → SelectedStockViewComponent
├── Views/
└── Program.cs
```

---

## 🧪 Pruebas

- `StocksControllerTest` se actualiza: el mock pasa de `IFinnhubService` a `IFinnhubStocksService`. Al depender de una interfaz mínima, la prueba muestra exactamente qué necesita el controlador.
- `StocksServiceTest`, `CustomWebApplicationFactory` y la prueba de integración no cambian (el factory reemplaza `IFinnhubRepository`, que se mantiene).

```bash
dotnet test
```
---

## 🔎 Decisiones y mejoras posibles

- `IFinnhubRepository` sigue con cuatro métodos, porque el enunciado solo pide dividir los servicios. Dividirlo también sería coherente con ISP, pero obligaría a cambiar el repositorio falso de las pruebas y los registros.
- `IStocksService` se mantiene unido: `TradeController` usa tanto las operaciones de compra como las de venta.
- `TradeController` aún recibe `IConfiguration` para entregar el token al WebSocket. Una mejora futura sería una clase de opciones con el token, inyectada con `IOptions`.

---

## 🛠️ Tecnologías y conceptos aplicados

- Principios SOLID: SRP, OCP, LSP, ISP y DIP.
- Inyección de dependencias y ciclos de vida de servicios (`AddScoped`).
- Refactorización sin cambiar el comportamiento externo.
- Patrón Repositorio y arquitectura de 4 capas.
- Excepciones personalizadas, middleware y filtros de acción.
- Logging con Serilog, Entity Framework Core y generación de PDF con Rotativa.
- Pruebas unitarias con Moq, AutoFixture y FluentAssertions, y pruebas de integración.
- WebSockets y JavaScript para actualizar el precio en tiempo real.

---

## 🔑 Configuración previa

**1. Token de Finnhub.** Registrate en [Finnhub.io](https://finnhub.io/) y guardá el token en User Secrets desde el directorio del proyecto web:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

**2. `appsettings.json`:** se mantienen `TradingOptions`, `ConnectionStrings` y `Serilog`.

**3. Base de datos.** Desde el directorio del proyecto web:

```bash
dotnet ef database update
```

Esta sección no agrega migraciones nuevas.

**4. Rotativa.** Colocar `wkhtmltopdf.exe` en `wwwroot/Rotativa/`.

**5. `.gitignore`.** Mantener la línea `logs/`.

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución desde el archivo `.sln` en Visual Studio, o desde una terminal situada en el directorio del proyecto web:

```bash
dotnet run
```

Para comprobar el manejo de errores, ejecutar en otro entorno:

```bash
dotnet run --environment Production
```

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible.
