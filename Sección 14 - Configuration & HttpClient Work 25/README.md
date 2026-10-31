# Sección 14 - Configuration & HttpClient Work 25 (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, centrado en el consumo de una API externa (**Finnhub.io**), el **Options Pattern**, el manejo seguro de credenciales con **User Secrets** y la actualización de datos en tiempo real mediante **WebSockets**.

---

## 🚀 Descripción del Proyecto

Aplicación web que muestra el nombre, el símbolo y el precio actual de una acción bursátil (por defecto Microsoft Corp, `MSFT`), con actualizaciones en vivo mientras el mercado está abierto.

La información inicial se obtiene desde el servidor consultando la API REST de Finnhub, y luego el navegador se conecta al WebSocket de Finnhub para refrescar el precio sin recargar la página.

Los principales componentes incluyen:

- Creación de la interfaz `IFinnhubService` con los métodos `GetCompanyProfile()` y `GetStockPriceQuote()`, que devuelven la respuesta de Finnhub como `Dictionary<string, object>`.
- Implementación de `FinnhubService` utilizando `IHttpClientFactory` y `HttpClient` para enviar las peticiones, leer la respuesta y deserializar el JSON. Un método privado (`SendRequestAsync`) centraliza el código repetido y valida respuestas vacías o con error.
- Almacenamiento del token de Finnhub en **User Secrets**, para no exponerlo en el código fuente ni en el repositorio.
- Configuración de `TradingOptions` (con `DefaultStockSymbol`) en `appsettings.json` y lectura mediante el **Options Pattern**, inyectando `IOptions<TradingOptions>` en el controlador.
- Creación del view model `StockTrade` (`StockSymbol`, `StockName`, `Price`, `Quantity`) para enviar los datos del controlador a la vista.
- `TradeController` con Attribute Routing (`Trade/Index`), que invoca ambos métodos del servicio y arma el `StockTrade`. Usa `TryGetValue` para evitar excepciones cuando Finnhub devuelve un símbolo inexistente.
- Vista fuertemente tipada `Trade/Index.cshtml`, con el símbolo guardado en un campo oculto (`hidden`) para que JavaScript pueda leerlo.
- Script en JavaScript que abre el WebSocket `wss://ws.finnhub.io`, se suscribe al símbolo, actualiza el precio en cada mensaje (mostrando el más alto del lote) y cancela la suscripción al cerrar la página para evitar fugas de memoria.
- Uso de `_Layout.cshtml`, `_ViewStart.cshtml`, `_ViewImports.cshtml` y la hoja de estilos `StyleSheet.css` provista por el curso.
- Uso de `CultureInfo.InvariantCulture` para convertir y mostrar los números decimales de forma consistente, independientemente de la configuración regional.

---

## 🏗️ Arquitectura del Proyecto

Se aplica una arquitectura **n-layer (n-capas)**, con responsabilidades separadas:

- **Controller (`TradeController`):** recibe la petición, consulta el servicio y construye el view model.
- **View Model (`StockTrade`):** transporta los datos del controlador a la vista.
- **View (`Trade/Index.cshtml`):** muestra la información y contiene el script del WebSocket.
- **Business Logic / Services (`IFinnhubService` y `FinnhubService`):** se comunican con la API de Finnhub.
- **Options (`TradingOptions`):** representa la configuración tipada leída desde `appsettings.json`.

```
StocksApp/
├── Controllers/        → TradeController
├── Models/             → StockTrade (view model)
├── Options/            → TradingOptions
├── ServiceContracts/   → IFinnhubService
├── Services/           → FinnhubService
├── Views/
│   ├── Shared/_Layout.cshtml
│   └── Trade/Index.cshtml
└── wwwroot/StyleSheet.css
```

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC.
- Consumo de APIs externas con `IHttpClientFactory` y `HttpClient`.
- Deserialización de JSON con `System.Text.Json`.
- Options Pattern (`IOptions<T>`) y `appsettings.json`.
- User Secrets para el manejo seguro de credenciales.
- Inyección de dependencias con `AddScoped`, `AddHttpClient` y `Configure<T>`.
- Programación asíncrona (`async` / `await`).
- Attribute Routing.
- Vistas fuertemente tipadas, Layout Views y secciones Razor (`@section`).
- WebSockets y JavaScript para actualizaciones en tiempo real.
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

Abrir la solución desde el archivo `.sln` en Visual Studio, o ejecutar desde una terminal situada en el directorio del proyecto:

```bash
dotnet run
```

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible.
