# Sección 15 - Entidades, DTOs y Pruebas Unitarias (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que amplía la aplicación de cotización de acciones con **modelos de entidad, DTOs, un servicio de órdenes de compra y venta**, validaciones con **Data Annotations** y **pruebas unitarias con xUnit**.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) desarrollada en la Sección 14. La aplicación mantiene la vista `Trade/Index`, que muestra el nombre, el símbolo y el precio de una acción con actualizaciones en vivo mediante WebSocket, y suma la lógica para crear y consultar órdenes de compra y de venta.

Los principales cambios incluyen:

- Creación de las **entidades** `BuyOrder` y `SellOrder`, con validaciones declarativas (`[Required]` y `[Range]`) sobre símbolo, nombre, cantidad (1 a 100000) y precio (1 a 10000).
- Creación de cuatro **DTOs**: `BuyOrderRequest`, `SellOrderRequest` (entrada al servicio) y `BuyOrderResponse`, `SellOrderResponse` (salida hacia el controlador). Las respuestas incluyen `TradeAmount` (cantidad × precio).
- Validación de fecha mínima (no anterior al 01/01/2000) mediante un atributo de validación personalizado, `MinimumDateAttribute`.
- Creación de `ValidationHelper`, que valida el modelo completo con `Validator.TryValidateObject` y lanza `ArgumentException` cuando hay errores.
- Definición de la interfaz `IStocksService` e implementación de `StocksService` con los métodos `CreateBuyOrder()`, `CreateSellOrder()`, `GetBuyOrders()` y `GetSellOrders()`.
- Conversión entre entidades y DTOs mediante métodos de extensión (`ToBuyOrderResponse()`, `ToSellOrderResponse()`) y métodos de conversión en los requests (`ToBuyOrder()`, `ToSellOrder()`).
- Inyección de `IStocksService` en `TradeController`, junto con `IOptions<TradingOptions>` e `IFinnhubService`. El controlador solo trabaja con DTOs y nunca accede a las entidades.
- Creación del proyecto de pruebas **StocksApp.Tests** con **xUnit**, con casos para cada método del servicio.

> **Nota:** el enunciado menciona las tablas `BuyOrders` y `SellOrders`, pero en esta etapa las órdenes se almacenan en **listas en memoria** dentro del servicio, que se registra como `Singleton` para conservar los datos entre peticiones. La persistencia en base de datos se incorporará en una sección posterior.

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura **n-layer (n-capas)**:

- **Controller (`TradeController`):** recibe la petición, consulta los servicios y construye el view model. Usa únicamente DTOs.
- **View Model (`StockTrade`):** transporta los datos del controlador a la vista.
- **View (`Trade/Index.cshtml`):** muestra la información y contiene el script del WebSocket.
- **Services (`FinnhubService` y `StocksService`):** contienen la lógica de negocio. `FinnhubService` consulta la API externa y `StocksService` gestiona las órdenes.
- **Entities (`BuyOrder`, `SellOrder`):** representan los datos almacenados y se usan solo dentro de los servicios.
- **DTOs:** definen los datos que entran y salen de los servicios.
- **Options (`TradingOptions`):** configuración tipada leída desde `appsettings.json`.

```
StocksApp/
├── Controllers/        → TradeController
├── DTO/                → BuyOrderRequest, BuyOrderResponse,
│                         SellOrderRequest, SellOrderResponse
├── Entities/           → BuyOrder, SellOrder
├── Helpers/            → MinimumDateAttribute, ValidationHelper
├── Models/             → StockTrade (view model)
├── Options/            → TradingOptions
├── ServiceContracts/   → IFinnhubService, IStocksService
├── Services/           → FinnhubService, StocksService
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/_Layout.cshtml
│   └── Trade/Index.cshtml
└── wwwroot/StyleSheet.css

StocksApp.Tests/
└── StocksServiceTest.cs
```

---

## 🧪 Pruebas Unitarias

Las pruebas con **xUnit** cubren los cuatro métodos de `StocksService`:

- **`CreateBuyOrder()` y `CreateSellOrder()`:**
  - Request nulo → `ArgumentNullException`.
  - Cantidad fuera de rango (0 y 100001) → `ArgumentException`.
  - Precio fuera de rango (0 y 10001) → `ArgumentException`.
  - Símbolo nulo → `ArgumentException`.
  - Fecha anterior al 2000-01-01 → `ArgumentException`.
  - Datos válidos → devuelve la respuesta con un `Guid` generado automáticamente.
- **`GetBuyOrders()` y `GetSellOrders()`:**
  - Por defecto, la lista devuelta está vacía.
  - Tras crear varias órdenes, la lista contiene todas ellas.

Se utilizan `[Fact]` para casos individuales y `[Theory]` con `[InlineData]` para agrupar los valores límite.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC.
- Modelos de entidad y patrón DTO (Request / Response).
- Validaciones con Data Annotations (`[Required]`, `[Range]`) y atributos de validación personalizados.
- Validación manual de modelos con `Validator.TryValidateObject`.
- Métodos de extensión para mapear entidades a DTOs.
- Interfaces y contratos de servicios con inyección de dependencias (`AddScoped` y `AddSingleton`).
- Options Pattern (`IOptions<T>`) y User Secrets.
- Consumo de APIs externas con `IHttpClientFactory`.
- WebSockets y JavaScript para actualizaciones en tiempo real.
- Pruebas unitarias con xUnit (`[Fact]`, `[Theory]`, `Assert.ThrowsAsync`).
- Arquitectura n-layer.

---

## 🔑 Configuración previa

El proyecto necesita un token de [Finnhub.io](https://finnhub.io/). Registrate para obtener el tuyo y guardalo en User Secrets desde el directorio del proyecto:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

En `appsettings.json` se define el símbolo por defecto:

```json
"TradingOptions": {
  "DefaultStockSymbol": "MSFT"
}
```

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

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible.
