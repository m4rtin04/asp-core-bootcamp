# Sección 18 - Entity Framework Core y Generación de PDF (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que incorpora **persistencia en SQL Server con Entity Framework Core** para almacenar las órdenes de compra y venta, y una nueva vista que exporta el listado de órdenes en **formato PDF** mediante **Rotativa**.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de las secciones anteriores. La aplicación sigue mostrando el precio en vivo de una acción mediante WebSocket, permite crear órdenes de compra y venta y listarlas. Hasta ahora las órdenes vivían en listas en memoria y se perdían al reiniciar la aplicación. En esta etapa se guardan en una base de datos.

La aplicación tiene tres páginas:

- **`Trade/Index`:** precio en vivo de la acción y formulario de nueva orden (Sell / Buy).
- **`Trade/Orders`:** listados de órdenes de compra y de venta.
- **`Trade/OrdersPDF`:** todas las órdenes en una sola tabla imprimible, generada como archivo PDF.

Los principales cambios incluyen:

- Creación de `StockMarketDbContext`, con los `DbSet<BuyOrder>` y `DbSet<SellOrder>` mapeados a las tablas `BuyOrders` y `SellOrders`.
- Configuración de la cadena de conexión `DefaultConnection` en `appsettings.json` y registro del contexto con `AddDbContext` y `UseSqlServer`.
- Refactorización de `StocksService` para usar el `DbContext` (inyectado por constructor) en lugar de listas en memoria, con `SaveChangesAsync()` para insertar y `ToListAsync()` para consultar.
- Uso de `AsNoTracking()` en las consultas de solo lectura.
- Cambio del registro de `IStocksService` de `AddSingleton` a `AddScoped`, ya que el `DbContext` tiene ciclo de vida *scoped*.
- Creación de la base de datos y las tablas mediante **migraciones** (`dotnet ef migrations add` y `dotnet ef database update`).
- Nueva acción `OrdersPDF()` en `TradeController` (GET `Trade/OrdersPDF`), que devuelve la vista como PDF con `ViewAsPdf` de **Rotativa**, con márgenes y orientación horizontal.
- Nueva vista `OrdersPDF.cshtml` sin layout y con estilos en línea, que combina compras y ventas en una sola tabla (fecha, acción, tipo de orden, cantidad, precio y monto) ordenada de la más reciente a la más antigua.
- Enlace con tag helper en `Orders.cshtml` para descargar el PDF.
- Adaptación de las pruebas unitarias para usar una base de datos **InMemory** distinta por cada test.

> **Nota:** Rotativa necesita el ejecutable `wkhtmltopdf` dentro de `wwwroot/Rotativa/`. Rotativa no lo incluye, por lo que hay que descargarlo aparte. Es una herramienta sin mantenimiento activo; en proyectos nuevos suelen preferirse alternativas como QuestPDF.

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura **n-layer (n-capas)**, ahora con una capa de acceso a datos:

- **Controller (`TradeController`):** recibe las peticiones, invoca los servicios y construye los view models. Usa únicamente DTOs. Suma la acción `OrdersPDF()`.
- **View Models (`StockTrade`, `Orders`):** transportan los datos del controlador a las vistas.
- **Views (`Index`, `Orders`, `OrdersPDF`):** muestran la información; `Index` contiene además el script del WebSocket.
- **Services (`FinnhubService`, `StocksService`):** lógica de negocio. `StocksService` ahora persiste en base de datos.
- **Data (`StockMarketDbContext`):** puente entre las entidades y SQL Server mediante EF Core.
- **Entities y DTOs:** las entidades se usan solo dentro de los servicios; los DTOs definen los datos de entrada y salida.
- **Options (`TradingOptions`):** configuración tipada.

```
StocksApp/
├── Controllers/        → TradeController (Index, Orders, OrdersPDF, BuyOrder, SellOrder)
├── DTO/                → BuyOrderRequest, BuyOrderResponse,
│                         SellOrderRequest, SellOrderResponse
├── Entities/           → BuyOrder, SellOrder, StockMarketDbContext
├── Helpers/            → MinimumDateAttribute, ValidationHelper
├── Migrations/         → migraciones de EF Core
├── Models/             → StockTrade, Orders (view models)
├── Options/            → TradingOptions
├── ServiceContracts/   → IFinnhubService, IStocksService
├── Services/           → FinnhubService, StocksService
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/_Layout.cshtml
│   └── Trade/
│       ├── Index.cshtml
│       ├── Orders.cshtml
│       └── OrdersPDF.cshtml
└── wwwroot/
    ├── StyleSheet.css
    └── Rotativa/       → wkhtmltopdf.exe

StocksApp.Tests/
└── StocksServiceTest.cs
```

---

## 🧭 Rutas de la aplicación

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/` y `/Trade/Index` | Precio en vivo de la acción y formulario de nueva orden. |
| POST | `/Trade/BuyOrder` | Crea una orden de compra y redirige a `Trade/Orders`. |
| POST | `/Trade/SellOrder` | Crea una orden de venta y redirige a `Trade/Orders`. |
| GET | `/Trade/Orders` | Lista las órdenes de compra y de venta. |
| GET | `/Trade/OrdersPDF` | Devuelve todas las órdenes como archivo PDF. |

---

## 🧪 Pruebas Unitarias

Las pruebas con **xUnit** se mantienen, con un único cambio: como `StocksService` ahora depende de `StockMarketDbContext`, el constructor de la clase de pruebas crea un contexto con el proveedor **EF Core InMemory**, usando un nombre de base distinto por cada test (`Guid.NewGuid()`). Así cada prueba arranca con una base vacía y no depende de un SQL Server real.

Siguen cubriendo `CreateBuyOrder()`, `CreateSellOrder()`, `GetBuyOrders()` y `GetSellOrders()`.

---


## 🔑 Configuración previa

**1. Token de Finnhub.** El proyecto necesita un token de [Finnhub.io](https://finnhub.io/). Registrate para obtener el tuyo y guardalo en User Secrets desde el directorio del proyecto:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

**2. Configuración en `appsettings.json`:**

```json
"TradingOptions": {
  "DefaultStockSymbol": "MSFT",
  "DefaultOrderQuantity": 100
},
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StockMarketDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
}
```

> `(localdb)\MSSQLLocalDB` solo existe en Windows. En macOS o Linux hay que usar SQL Server en Docker y adaptar la cadena de conexión.

**3. Base de datos.** Desde el directorio del proyecto web, aplicar las migraciones:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

**4. Rotativa.** Colocar `wkhtmltopdf.exe` en `wwwroot/Rotativa/`.

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución desde el archivo `.sln` en Visual Studio, o ejecutar desde una terminal situada en el directorio del proyecto web:

```bash
dotnet run
```

Para ejecutar las pruebas unitarias, desde la carpeta de la solución:

```bash
dotnet test
```

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible, y las órdenes se crean con ese valor.
