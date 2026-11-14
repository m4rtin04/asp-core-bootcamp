# Sección 22 - Manejo de Errores (Error Handling) en ASP.NET Core (Stocks App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, que incorpora un **manejo de errores centralizado**: una excepción personalizada para las fallas de Finnhub, un **middleware propio** que registra las excepciones y las relanza, y el middleware integrado `UseExceptionHandler` para mostrar una **vista de error amigable** al usuario.

---

## 🚀 Descripción del Proyecto

Continuación de la Stocks App (Finnhub.io) de la Sección 21. La aplicación sigue mostrando el precio en vivo mediante WebSocket, permite explorar acciones populares, crear órdenes de compra y venta guardadas en SQL Server, exportarlas a PDF, registrar eventos con Serilog y validar órdenes con un filtro de acción. En esta etapa se agrega la capa que se encarga de lo que ocurre **cuando algo falla**.

Los principales cambios incluyen:

- Creación de la excepción personalizada `FinnhubException`, con los tres constructores habituales (vacío, con mensaje y con mensaje más excepción interna), para conservar la causa original en `InnerException`.
- Refactorización de `FinnhubService`: todos sus métodos (`GetCompanyProfile`, `GetStockPriceQuote`, `GetStocks` y `SearchStocks`) envuelven las fallas del repositorio en una `FinnhubException`. Un método privado genérico (`Execute`) concentra el `try/catch` en un único lugar, en vez de repetirlo cuatro veces.
- Creación de `ExceptionHandlingMiddleware`, un middleware personalizado que atrapa cualquier excepción de los componentes posteriores del pipeline (middlewares, acciones y filtros), registra sus detalles con `ILogger` y la **relanza** con `throw;` para conservar el *stack trace*.
- Método de extensión `UseExceptionHandlingMiddleware()` para registrar el middleware de forma limpia en `Program.cs`.
- Configuración del pipeline para entornos **distintos de Development**: primero `UseExceptionHandler("/Home/Error")` y después el middleware personalizado, de modo que la excepción relanzada sea atrapada por el manejador integrado.
- Creación de `HomeController` con la acción `Error()` (`/Home/Error`), que obtiene la excepción mediante `IExceptionHandlerPathFeature`.
- Creación de la vista `Views/Shared/Error.cshtml`, con un mensaje amigable y un enlace de regreso a **Explore**.
- En Development se mantiene `UseDeveloperExceptionPage()` con la página de excepción detallada.

> **Seguridad:** la vista de error **no muestra** el mensaje de la excepción. En producción, los mensajes de error pueden filtrar datos internos (rutas, consultas, URLs con tokens), por lo que solo se registran en el log. El mensaje de `FinnhubException` es genérico a propósito; la causa real viaja en `InnerException` y se escribe en el log.

> **Orden del pipeline:** `UseExceptionHandler` debe ir **antes** que `ExceptionHandlingMiddleware`. Las excepciones suben desde los componentes más internos hacia los externos, por lo que, si el orden se invierte, la excepción relanzada no tiene quién la atrape y el usuario recibe un error 500 en blanco.

---

## 🧩 Cómo funciona el manejo de errores

```
Request → UseExceptionHandler → ExceptionHandlingMiddleware → ... → Acción
                ▲                         ▲                           │
                │  atrapa y muestra       │  loguea y relanza         │
                └─────────────────────────┴───── excepción ───────────┘
```

| Paso | Componente | Qué hace |
|------|-----------|----------|
| 1 | `FinnhubRepository` | Lanza una excepción si falla la llamada HTTP o Finnhub devuelve un error. |
| 2 | `FinnhubService` | Envuelve la falla en `FinnhubException`, conservando la excepción original. |
| 3 | `ExceptionHandlingMiddleware` | Registra la causa real (la `InnerException` si existe) y relanza con `throw;`. |
| 4 | `UseExceptionHandler` | Atrapa la excepción relanzada y redirige internamente a `/Home/Error`. |
| 5 | `HomeController.Error()` | Devuelve la vista amigable `Error.cshtml`. |

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");   
    app.UseExceptionHandlingMiddleware();     
}
```

---

## 🏗️ Arquitectura del Proyecto

Se mantiene la arquitectura de **4 capas** (UI, BLL, RL, DAL). El manejo de errores es transversal: el middleware actúa sobre el pipeline HTTP y la excepción personalizada se lanza desde la capa de negocio.

```
StocksApp/
├── Controllers/            → TradeController, StocksController, HomeController
├── DTO/
├── Entities/
├── Exceptions/             → FinnhubException
├── Filters/                → CreateOrderActionFilter
├── Helpers/
├── Middlewares/            → ExceptionHandlingMiddleware
├── Migrations/
├── Models/
├── Options/
├── Repositories/
├── RepositoryContracts/
├── ServiceContracts/
├── Services/               → FinnhubService (envuelve errores), StocksService
├── ViewComponents/
├── Views/
│   └── Shared/Error.cshtml → vista de error amigable
├── logs/                   → excluido de Git
└── Program.cs              → pipeline de manejo de errores
```

---

## 🧪 Pruebas

No se agregaron pruebas nuevas en esta sección, y las existentes se mantienen sin cambios: `FinnhubService` ahora envuelve errores, pero las pruebas actuales cubren `StocksService`, el controlador y la ruta de integración.

```bash
dotnet test
```


Para verificar el flujo de errores manualmente (`UseExceptionHandler` solo se activa fuera de Development):

1. Provocar una falla: guardar un token inválido en User Secrets (`dotnet user-secrets set "FinnhubToken" "xxx"`).
2. Ejecutar en Production:
   ```bash
   dotnet run --environment Production
   ```
   En PowerShell también: `$env:ASPNETCORE_ENVIRONMENT="Production"`.
3. Entrar a `/Stocks/Explore`: debe aparecer la vista "Something went wrong".
4. Revisar la consola o `logs/`: debe haber un registro con el tipo y el mensaje de la causa real.
5. Invertir temporalmente el orden de los dos middlewares del `else` para comprobar que la vista amigable deja de aparecer.
6. **Restaurar el token real** al terminar.

En Development se verá la página de excepción detallada, que es lo esperado.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC, pipeline de middleware y arquitectura de 4 capas.
- **Excepciones personalizadas** y conservación de la causa original con `InnerException`.
- **Middleware personalizado** (`RequestDelegate`, `InvokeAsync`) y método de extensión para registrarlo.
- Relanzado de excepciones con `throw;` para conservar el *stack trace*.
- `UseExceptionHandler` e `IExceptionHandlerPathFeature`.
- Configuración del pipeline según el entorno (`IsDevelopment()`).
- Importancia del orden de los middlewares.
- Método genérico auxiliar (`Execute<T>`) para evitar código repetido.
- Logging con Serilog e `ILogger<T>`.
- Buenas prácticas de seguridad: no mostrar detalles de excepciones al usuario.
- Filtros de acción, patrón Repositorio, View Components y Options Pattern.
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

Por defecto se ejecuta en Development. Para ver el manejo de errores amigable, ejecutar en otro entorno:

```bash
dotnet run --environment Production
```

> **Nota:** el precio solo se actualiza en vivo mientras el mercado está abierto (aproximadamente de 09:30 a 16:00, hora de Nueva York). Fuera de ese horario se muestra el último precio disponible.
