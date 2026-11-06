# Sección 17 - Tag Helpers (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que refactoriza las vistas de la Stocks App para usar **Tag Helpers** (`asp-for`, `asp-controller`, `asp-action`, `asp-validation-summary`, entre otros) en lugar de HTML escrito a mano.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de las secciones 14, 16 y 17. La funcionalidad no cambia: la aplicación sigue mostrando el precio en vivo de una acción mediante WebSocket, permite crear órdenes de compra y venta, y lista las órdenes creadas. Lo que cambia es **cómo se escriben las vistas**.

Los principales cambios incluyen:

- Uso de `asp-for` en los campos del formulario (`StockSymbol`, `StockName`, `Price` y `Quantity`). Genera automáticamente los atributos `id`, `name` y `value` a partir de las propiedades del modelo, lo que evita errores de tipeo que romperían el *model binding*.
- Uso de `<label asp-for="Quantity">` para generar la etiqueta asociada al campo.
- Uso de `asp-format="{0:F2}"` para fijar dos decimales en el campo oculto del precio.
- Reemplazo del bloque manual de errores (`ViewBag.Errors`) por `asp-validation-summary="All"`, que lista los errores de `ModelState` directamente.
- Mantenimiento de `asp-controller` y `asp-action` en los enlaces del menú y en los botones **Sell** y **Buy**, que generan el atributo `formaction` hacia cada ruta.
- Uso de `asp-append-version="true"` en la hoja de estilos para evitar que el navegador use una versión cacheada cuando el CSS cambia.
- Simplificación de `TradeController.IndexWithErrors()`, que ya no necesita construir una lista de errores en `ViewBag`.
- Actualización del script de JavaScript, ya que `asp-for="Price"` genera el `id="Price"` en lugar del `OrderPrice` utilizado antes.
- Incorporación en `Orders.cshtml` de un enlace con tag helper hacia `Trade/Index` cuando no hay órdenes.

> **Nota:** `asp-for` toma primero el valor de `ModelState` y luego el del modelo. Por eso, cuando `Index` se vuelve a mostrar por un error de validación, los campos conservan lo que el usuario envió.

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura **n-layer (n-capas)**. Esta sección solo modifica la capa de vistas y un método auxiliar del controlador:

- **Controller (`TradeController`):** sin cambios de comportamiento; se simplifica el método auxiliar que vuelve a mostrar `Index` con errores.
- **View Models (`StockTrade`, `Orders`):** sin cambios.
- **Views (`Trade/Index.cshtml`, `Trade/Orders.cshtml`, `_Layout.cshtml`):** refactorizadas con Tag Helpers.
- **Services, Entities, DTOs y Options:** sin cambios.

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
│   ├── _ViewImports.cshtml      → habilita los Tag Helpers
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

## 🏷️ Tag Helpers utilizados

| Tag Helper | Dónde se usa | Qué hace |
|------------|--------------|----------|
| `asp-controller` / `asp-action` | Enlaces del menú, botones Buy/Sell, enlaces de `Orders` | Genera la URL (o el `formaction`) según el controlador y la acción. |
| `asp-for` | Inputs y `<label>` del formulario | Genera `id`, `name`, `value` y `type` según la propiedad del modelo. |
| `asp-format` | Campo oculto `Price` | Aplica un formato al valor del campo. |
| `asp-validation-summary` | Bloque "New Order" | Muestra los errores de validación de `ModelState`. |
| `asp-append-version` | `<link>` del CSS en el layout | Agrega un hash del archivo a la URL para invalidar la caché. |

---

## 🧪 Pruebas Unitarias

Las pruebas con **xUnit** de la Sección 16 se mantienen sin cambios, ya que esta etapa solo modifica las vistas. Siguen cubriendo `CreateBuyOrder()`, `CreateSellOrder()`, `GetBuyOrders()` y `GetSellOrders()`, y sirven para comprobar que la refactorización no rompió la lógica existente.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC.
- Tag Helpers integrados (`asp-for`, `asp-controller`, `asp-action`, `asp-format`, `asp-validation-summary`, `asp-append-version`).
- Habilitación de Tag Helpers con `@addTagHelper` en `_ViewImports.cshtml`.
- Model Binding con DTOs y validación con Data Annotations.
- Patrón Post/Redirect/Get con `RedirectToAction`.
- Formularios con un único `<form>` y múltiples destinos (`formaction`).
- Vistas fuertemente tipadas, Layout Views y secciones Razor.
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
