# Sección 16 - Órdenes de Compra y Venta en la Interfaz (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que completa la aplicación de cotización de acciones con **formularios para crear órdenes de compra y venta**, una **vista de listado de órdenes** y una navegación entre páginas, manteniendo las actualizaciones de precio en vivo mediante **WebSocket**.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de las secciones 14 y 16. Hasta ahora existía la lógica de negocio para órdenes (entidades, DTOs y `StocksService`), pero sin interfaz. En esta etapa se conecta esa lógica con el controlador y las vistas.

La aplicación tiene dos páginas:

- **`Trade/Index`:** muestra el nombre, el símbolo y el precio en vivo de la acción, junto con un bloque "New Order" con la cantidad por defecto y los botones **Sell** y **Buy**.
- **`Trade/Orders`:** muestra dos listados lado a lado, **Buy Orders** y **Sell Orders**, con cantidad, precio, monto total y fecha de cada orden.

Los principales cambios incluyen:

- Incorporación de `DefaultOrderQuantity` en `TradingOptions` (`appsettings.json`) para definir la cantidad inicial del formulario, leída mediante el **Options Pattern**.
- Creación del view model `Orders`, con las listas `BuyOrders` (`BuyOrderResponse`) y `SellOrders` (`SellOrderResponse`).
- Nuevas acciones en `TradeController` con Attribute Routing: `Orders()` (GET), `BuyOrder()` (POST) y `SellOrder()` (POST).
- Uso de `BuyOrderRequest` y `SellOrderRequest` como parámetros de las acciones POST mediante **Model Binding**, respetando la regla de que el controlador no accede a las entidades.
- Asignación de `DateAndTimeOfOrder` en el controlador y revalidación del modelo con `ModelState.Clear()` y `TryValidateModel()`, porque la validación automática ocurre antes de asignar la fecha.
- Si el modelo es inválido, se vuelve a mostrar la vista `Index` con los datos recibidos y los mensajes de error. Si es válido, se crea la orden y se redirige a `Trade/Orders` (patrón *Post/Redirect/Get*).
- Un único formulario con dos botones que apuntan a rutas distintas mediante los tag helpers `asp-controller` y `asp-action` (generan `formaction`).
- Campos ocultos (`hidden`) para enviar `StockSymbol`, `StockName` y `Price`, con el precio actualizado desde JavaScript en cada mensaje del WebSocket para no enviar un valor desactualizado.
- Vista `Orders.cshtml` fuertemente tipada, con bucles `foreach`, formato de fechas y montos con `CultureInfo.InvariantCulture`, y mensaje cuando no hay órdenes.
- Barra de navegación en `_Layout.cshtml` con enlaces a **Trade** y **Orders**, y resaltado del enlace activo mediante `ViewBag`.
- Configuración de **Request Localization** con la cultura `en-US` para que el *model binding* interprete correctamente los decimales con punto, independientemente de la configuración regional del servidor.

> **Nota:** las órdenes se siguen almacenando en **listas en memoria** dentro de `StocksService`, registrado como `Singleton`. Si se reinicia la aplicación, las órdenes se pierden. La persistencia en base de datos se incorporará en una sección posterior.

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura **n-layer (n-capas)**:

- **Controller (`TradeController`):** recibe las peticiones, invoca los servicios y construye los view models. Usa únicamente DTOs.
- **View Models (`StockTrade`, `Orders`):** transportan los datos del controlador a las vistas.
- **Views (`Trade/Index.cshtml`, `Trade/Orders.cshtml`):** muestran la información; `Index` contiene además el script del WebSocket.
- **Services (`FinnhubService`, `StocksService`):** contienen la lógica de negocio. `FinnhubService` consulta la API externa y `StocksService` gestiona las órdenes.
- **Entities y DTOs:** las entidades se usan solo dentro de los servicios; los DTOs definen los datos de entrada y salida.
- **Options (`TradingOptions`):** configuración tipada (`DefaultStockSymbol` y `DefaultOrderQuantity`).

```
StocksApp/
├── Controllers/        → TradeController (Index, Orders, BuyOrder, SellOrder)
├── DTO/                → BuyOrderRequest, BuyOrderResponse,
│                         SellOrderRequest, SellOrderResponse
├── Entities/           → BuyOrder, SellOrder
├── Helpers/            → MinimumDateAttribute, ValidationHelper
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
│       └── Orders.cshtml
└── wwwroot/StyleSheet.css

StocksApp.Tests/
└── StocksServiceTest.cs
```

---

## 🧭 Rutas de la aplicación

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/` y `/Trade/Index` | Muestra la acción con su precio en vivo y el formulario de nueva orden. |
| POST | `/Trade/BuyOrder` | Crea una orden de compra y redirige a `Trade/Orders`. |
| POST | `/Trade/SellOrder` | Crea una orden de venta y redirige a `Trade/Orders`. |
| GET | `/Trade/Orders` | Lista las órdenes de compra y de venta, de la más reciente a la más antigua. |

---

## 🧪 Pruebas Unitarias

Las pruebas con **xUnit** de la Sección 16 se mantienen sin cambios, ya que esta etapa amplía el controlador y las vistas, no el servicio. Siguen cubriendo `CreateBuyOrder()`, `CreateSellOrder()`, `GetBuyOrders()` y `GetSellOrders()`, y sirven para comprobar que la nueva funcionalidad no rompió la lógica existente.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC.
- Model Binding con DTOs y validación con Data Annotations.
- Revalidación manual con `ModelState.Clear()` y `TryValidateModel()`.
- Patrón Post/Redirect/Get con `RedirectToAction`.
- Retorno de una vista con nombre distinto al de la acción (`View("Index", modelo)`).
- Attribute Routing con `[HttpGet]` y `[HttpPost]`.
- Tag Helpers (`asp-controller`, `asp-action`) en enlaces y botones.
- Formularios con campos ocultos y un único `<form>` con múltiples destinos.
- Vistas fuertemente tipadas, Layout Views y secciones Razor.
- Formato de fechas y números con `CultureInfo.InvariantCulture`.
- Request Localization (`UseRequestLocalization`).
- Options Pattern (`IOptions<T>`) y User Secrets.
- WebSockets y JavaScript para actualizar el precio en tiempo real.
- Arquitectura n-layer.

---

## 🔑 Configuración previa

El proyecto necesita un token de [Finnhub.io](https://finnhub.io/). Registrate para obtener el tuyo y guardalo en User Secrets desde el directorio del proyecto:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

En `appsettings.json` se definen el símbolo y la cantidad de orden por defecto:

```json
"TradingOptions": {
  "DefaultStockSymbol": "MSFT",
  "DefaultOrderQuantity": 100
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

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible, y las órdenes se crean con ese valor.
