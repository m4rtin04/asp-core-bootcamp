# Sección 19 - Advanced Unit Testing: Moq & Repository Pattern (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que reorganiza la Stocks App en una **arquitectura de 4 capas** con **repositorios**, agrega una página para **explorar las acciones más populares** con un **View Component** de detalle, y amplía las pruebas con **Moq, AutoFixture, FluentAssertions** y **tests de integración**.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de las secciones anteriores. La aplicación sigue mostrando el precio en vivo mediante WebSocket, permite crear órdenes de compra y venta guardadas en SQL Server y exportarlas a PDF. En esta etapa se separa el acceso a datos de la lógica de negocio y se suma una nueva página de exploración.

La aplicación tiene cuatro páginas:

- **`Stocks/Explore`:** lista de las 25 acciones más populares. Al hacer clic en una, se muestra a la derecha su detalle (logo, nombre, símbolo, industria, exchange y precio) con un botón **Trade Now**.
- **`Trade/Index`:** precio en vivo de la acción y formulario de nueva orden (Sell / Buy).
- **`Trade/Orders`:** listados de órdenes de compra y de venta.
- **`Trade/OrdersPDF`:** todas las órdenes en una sola tabla, generada como PDF.

Los principales cambios incluyen:

- Creación de la capa de **repositorios**: `IFinnhubRepository` / `FinnhubRepository` (consume la API de Finnhub) e `IStocksRepository` / `StocksRepository` (accede a la base de datos con EF Core).
- Nuevos métodos en Finnhub: `GetStocks()` (lista de acciones del mercado US) y `SearchStocks()` (búsqueda por nombre).
- Refactorización de `FinnhubService` para que solo delegue en su repositorio, y de `StocksService` para que valide, convierta DTOs a entidades y delegue en `IStocksRepository`.
- Uso de `Uri.EscapeDataString` en los parámetros de las URL y verificación del código de estado HTTP antes de deserializar.
- Nuevo `StocksController` con la acción `Explore()` (GET `Stocks/Explore/{stock?}`), que filtra las acciones según `Top25PopularStocks` (configurado en `appsettings.json` y leído mediante el **Options Pattern**).
- Nuevos view models `Stock` y `StockDetails`.
- Creación de `SelectedStockViewComponent`, invocado desde `Explore.cshtml` con `Component.InvokeAsync`, que muestra el detalle de la acción seleccionada y el botón **Trade Now**.
- Cambio de la ruta de `Trade/Index` a `Trade/Index/{stockSymbol?}` para recibir el símbolo desde Explore.
- Redirección de la raíz `/` hacia `Stocks/Explore`.
- Nuevo enlace **Explore** en la barra de navegación del layout.
- Reescritura de las pruebas unitarias del servicio con **Moq** (repositorios simulados), **AutoFixture** (datos de prueba) y **FluentAssertions**.
- Prueba unitaria del controlador (`StocksController.Explore`) y prueba de integración de la ruta `/Trade/Index/MSFT` con `WebApplicationFactory`.

> **Nota:** `Top25PopularStocks` define qué acciones se muestran.


---

## 🏗️ Arquitectura del Proyecto

Se pasa de 3 a **4 capas**:

| Capa | En el proyecto |
|------|----------------|
| **UI** | Controllers, Views y View Components. |
| **BLL (Business Logic Layer)** | `FinnhubService` y `StocksService`: validación y reglas de negocio. |
| **RL (Repository Layer)** | `FinnhubRepository` y `StocksRepository`: consultas y operaciones sobre los datos. |
| **DAL (Data Access Layer)** | `StockMarketDbContext` y `HttpClient`. |

La capa de negocio ya no usa EF Core ni `HttpClient` directamente; solo la capa de repositorios lo hace. Esto permite **simular los repositorios** en las pruebas unitarias.

```
StocksApp/
├── Controllers/            → TradeController, StocksController
├── DTO/                    → BuyOrderRequest, BuyOrderResponse,
│                             SellOrderRequest, SellOrderResponse
├── Entities/               → BuyOrder, SellOrder, StockMarketDbContext
├── Helpers/                → MinimumDateAttribute, ValidationHelper
├── Migrations/             → migraciones de EF Core
├── Models/                 → StockTrade, Orders, Stock, StockDetails
├── Options/                → TradingOptions
├── Repositories/           → FinnhubRepository, StocksRepository
├── RepositoryContracts/    → IFinnhubRepository, IStocksRepository
├── ServiceContracts/       → IFinnhubService, IStocksService
├── Services/               → FinnhubService, StocksService
├── ViewComponents/         → SelectedStockViewComponent
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── Components/SelectedStock/Default.cshtml
│   ├── Stocks/Explore.cshtml
│   └── Trade/
│       ├── Index.cshtml
│       ├── Orders.cshtml
│       └── OrdersPDF.cshtml
└── wwwroot/
    ├── StyleSheet.css
    └── Rotativa/           → wkhtmltopdf.exe

StocksApp.Tests/
├── StocksServiceTest.cs
├── StocksControllerTest.cs
├── CustomWebApplicationFactory.cs
└── TradeControllerIntegrationTest.cs
```

---

## 🧭 Rutas de la aplicación

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/` | Redirige a `/Stocks/Explore`. |
| GET | `/Stocks/Explore/{stock?}` | Lista de acciones populares; con símbolo, muestra además su detalle. |
| GET | `/Trade/Index/{stockSymbol?}` | Precio en vivo y formulario de nueva orden. |
| POST | `/Trade/BuyOrder` | Crea una orden de compra y redirige a `Trade/Orders`. |
| POST | `/Trade/SellOrder` | Crea una orden de venta y redirige a `Trade/Orders`. |
| GET | `/Trade/Orders` | Lista las órdenes de compra y de venta. |
| GET | `/Trade/OrdersPDF` | Devuelve todas las órdenes como archivo PDF. |

---

## 🧪 Pruebas

**Pruebas unitarias del servicio (`StocksService`)**, con **xUnit**, **Moq**, **AutoFixture** y **FluentAssertions**:

- **`CreateBuyOrder()` y `CreateSellOrder()`:** request nulo (`ArgumentNullException`), cantidad y precio fuera de rango, símbolo nulo y fecha anterior al 2000-01-01 (`ArgumentException`), y caso válido con `Guid` generado. Cuando la validación falla se verifica que nunca se invoque al repositorio.
- **`GetBuyOrders()` y `GetSellOrders()`:** lista vacía por defecto y lista con todas las órdenes devueltas por el repositorio simulado.

**Prueba unitaria del controlador:** `StocksController.Explore()` con `stock` nulo devuelve la vista con un `List<Stock>` que contiene solo las acciones populares, con el servicio simulado.

**Prueba de integración:** una petición GET a `/Trade/Index/MSFT` devuelve HTML con un elemento de clase `price`. Se usa `WebApplicationFactory<Program>` y un repositorio de Finnhub falso (`FakeFinnhubRepository`), para que la prueba no dependa de internet, del token ni de la cuota de la API. Para esto `Program.cs` termina con `public partial class Program { }`.

```bash
dotnet test
```


---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC y arquitectura de 4 capas (UI, BLL, RL, DAL).
- Patrón Repositorio con interfaces e inyección de dependencias.
- View Components (`ViewComponent`, `InvokeAsync`, `Component.InvokeAsync`).
- Rutas con parámetros opcionales mediante Attribute Routing.
- Consumo de APIs externas con `IHttpClientFactory`.
- Entity Framework Core con SQL Server y migraciones.
- Options Pattern (`IOptions<T>`) y User Secrets.
- Generación de PDF con Rotativa.
- Tag Helpers (`asp-controller`, `asp-action`, `asp-route-*`, `asp-for`).
- Pruebas unitarias con Moq, AutoFixture y FluentAssertions.
- Pruebas de integración con `WebApplicationFactory` y HtmlAgilityPack.
- WebSockets y JavaScript para actualizar el precio en tiempo real.

---

## 🔑 Configuración previa

**1. Token de Finnhub.** El proyecto necesita un token de [Finnhub.io](https://finnhub.io/). Registrate para obtener el tuyo y guardalo en User Secrets desde el directorio del proyecto web:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

**2. Configuración en `appsettings.json`:**

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

Esta sección no agrega migraciones nuevas: las tablas no cambian.

**4. Rotativa.** Colocar `wkhtmltopdf.exe` en `wwwroot/Rotativa/`. Es una herramienta sin mantenimiento activo; en proyectos nuevos suelen preferirse alternativas como QuestPDF.

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución desde el archivo `.sln` en Visual Studio, o ejecutar desde una terminal situada en el directorio del proyecto web:

```bash
dotnet run
```

Al abrir `/` se redirige a **Explore**. Elegir una acción muestra su detalle; **Trade Now** lleva a la pantalla de trading con el precio en vivo.

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible, y las órdenes se crean con ese valor.
