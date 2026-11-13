# Sección 21 - Filtros (Filters) 

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que incorpora un **filtro de acción asíncrono** para validar las órdenes de compra y venta antes de que se ejecute la acción del controlador, y adapta la interfaz a una **nueva hoja de estilos**.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de la Sección 20. La aplicación sigue mostrando el precio en vivo mediante WebSocket, permite explorar acciones populares, crear órdenes de compra y venta guardadas en SQL Server, exportarlas a PDF y registrar eventos con Serilog. En esta etapa, la validación de las órdenes sale del controlador y pasa a un **filtro reutilizable**.

Los principales cambios incluyen:

- Creación de `CreateOrderActionFilter`, un filtro que implementa `IAsyncActionFilter`, aplicado a `TradeController.BuyOrder()` y `TradeController.SellOrder()` mediante `[TypeFilter]`.
- Creación de la clase base abstracta `OrderRequest`, de la que heredan `BuyOrderRequest` y `SellOrderRequest`, para que el filtro trate ambas con el mismo código.
- Lógica *before* del filtro: verifica que se aplique a `TradeController`, obtiene el argumento `orderRequest`, asigna `DateAndTimeOfOrder`, limpia el `ModelState` y vuelve a validar el modelo con `TryValidateModel()`.
- Si hay errores, el filtro arma un `StockTrade`, carga los mensajes en `ViewBag.Errors` y devuelve la vista `Trade/Index` asignando `context.Result` (*short-circuiting*), por lo que la acción nunca se ejecuta.
- Si no hay errores, el filtro invoca `next()` y continúa el pipeline.
- Simplificación de `BuyOrder()` y `SellOrder()`: ahora solo crean la orden, registran el log y redirigen a `Trade/Orders`. Se eliminaron la asignación de la fecha, la revalidación y el método auxiliar `IndexWithErrors()`.
- Registro de las órdenes rechazadas con `ILogger` dentro del filtro.
- Adaptación del layout y de las vistas a la **nueva hoja de estilos** (`StyleSheet.css`): barra superior con título y búsqueda, menú con enlace activo, paneles `trading-panel`, listas `list`, botones `button-red-back` / `button-green-back` / `button-blue-back` y etiquetas `pill`.

> **Importante:** el filtro busca el argumento por el nombre `orderRequest`. Si el parámetro de la acción se llama distinto, el filtro no valida ni asigna la fecha, y el error aparece más adelante en `StocksService`. Una alternativa más robusta es buscar el primer argumento de tipo `OrderRequest` con `context.ActionArguments.Values.OfType<OrderRequest>()`.

> **Doble validación intencional:** el filtro valida para mostrar errores al usuario, y `StocksService` sigue validando por su cuenta porque el servicio también se usa desde otros puntos, como las pruebas.

---

## 🧩 Cómo funciona el filtro

```
Request → Routing → Model Binding → [CreateOrderActionFilter] → Acción → Response
                                          │
                                          └─ con errores: devuelve la vista Index
                                             y la acción no se ejecuta
```

| Paso | Qué hace |
|------|----------|
| 1 | Comprueba que el controlador sea `TradeController`. |
| 2 | Obtiene de `ActionArguments` el argumento `orderRequest` (tipo `OrderRequest`). |
| 3 | Asigna `DateAndTimeOfOrder` y revalida el modelo (`ModelState.Clear()` + `TryValidateModel()`). |
| 4 | Sin errores: `await next()` continúa con la acción. |
| 5 | Con errores: arma `StockTrade`, carga `ViewBag.Errors` y asigna `context.Result`. |

El filtro recibe `ILogger` e `IConfiguration` por constructor, por eso se aplica con `[TypeFilter]`. La alternativa es `[ServiceFilter]`, que exige registrar el filtro en el contenedor de dependencias.

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura de **4 capas** (UI, BLL, RL, DAL). Los filtros pertenecen a la capa de UI, porque actúan sobre el pipeline de MVC.

```
StocksApp/
├── Controllers/            → TradeController, StocksController
├── DTO/                    → OrderRequest (base), BuyOrderRequest, BuyOrderResponse,
│                             SellOrderRequest, SellOrderResponse
├── Entities/
├── Filters/                → CreateOrderActionFilter
├── Helpers/
├── Migrations/
├── Models/
├── Options/
├── Repositories/
├── RepositoryContracts/
├── ServiceContracts/
├── Services/
├── ViewComponents/
├── Views/                  → adaptadas a la nueva hoja de estilos
├── logs/                   → excluido de Git
└── wwwroot/StyleSheet.css  → nueva versión
```

---

## 📚 Temas vistos en la sección

La sección del curso cubre el sistema de filtros completo. Este ejercicio aplica específicamente el filtro de acción asíncrono, mientras que el resto se practicó en los ejemplos de las clases:

- Introducción a los filtros y pipeline de filtros.
- Action Filter, validación de parámetros y uso de `ViewData` en filtros.
- Serilog y logging estructurado.
- Argumentos en filtros y filtros globales.
- Orden personalizado de filtros e `IOrderedFilter`.
- Filtros asíncronos y cortocircuito (*short-circuiting*) de un filtro de acción.
- Result Filter, Resource Filter, Authorization Filter y Exception Filter.
- Impacto del cortocircuito e `IAlwaysRunResultFilter`.
- Sobrescritura de filtros (*Filter Overrides*).
- `ServiceFilter`, clases de atributo de filtro e `IFilterFactory`.
- Filtros frente a middleware.
- Mejoras de interfaz y método de extensión para configurar servicios.

---

## 🧪 Pruebas

Las pruebas de la Sección 20 se mantienen sin cambios: `StocksService` conserva su validación, y `BuyOrderRequest` / `SellOrderRequest` siguen exponiendo las mismas propiedades (ahora heredadas de `OrderRequest`).

```bash
dotnet test
```

Para verificar el filtro manualmente:

1. **Camino válido:** en `/Trade/Index/MSFT`, comprar con cantidad 50 redirige a `Trade/Orders`.
2. **Camino inválido:** con cantidad `0` o `100001` (quitando temporalmente `min` y `max` del input) se vuelve a mostrar `Index` con el error. En el log aparece la advertencia del filtro y no aparece `Buy order created`.
3. Un breakpoint en la primera línea de `BuyOrder()` no debe alcanzarse con datos inválidos.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC, pipeline de filtros y arquitectura de 4 capas.
- `IAsyncActionFilter`, `ActionExecutingContext` y `ActionArguments`.
- Cortocircuito del pipeline asignando `context.Result`.
- `[TypeFilter]` y `[ServiceFilter]` para filtros con dependencias.
- Herencia de DTOs para reutilizar validaciones.
- Validación con Data Annotations y `TryValidateModel`.
- Logging con Serilog e `ILogger<T>`.
- Patrón Repositorio, View Components y Options Pattern.
- Entity Framework Core con SQL Server y generación de PDF con Rotativa.
- Pruebas unitarias y de integración.
- WebSockets y JavaScript para actualizar el precio en tiempo real.

---

## 🔑 Configuración previa

**1. Token de Finnhub.** Registrate en [Finnhub.io](https://finnhub.io/) y guardá el token en User Secrets desde el directorio del proyecto web:

```bash
dotnet user-secrets init
dotnet user-secrets set "FinnhubToken" "TU_TOKEN_AQUI"
```

**2. `appsettings.json`:** se mantienen `TradingOptions`, `ConnectionStrings` y `Serilog` de la sección anterior.

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
