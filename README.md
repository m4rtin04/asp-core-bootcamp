# asp-core-bootcamp
# Curso de ASP.NET Core (.NET 10) - Harsha

Este repositorio contiene mi progreso, prácticas y ejercicios realizados a lo largo del curso de ASP.NET Core de Harsha.

## 📂 Contenido del Curso

### - Sección 3: HTTP y Query Strings
* **Proyecto:** Calculadora basada en Web.
* **Descripción:** Implementación de un servidor web utilizando `app.Run` y Request Delegates.
* **Conceptos clave:** 
  * Lectura y validación de parámetros en la URL (Query Strings: `firstNumber`, `secondNumber`, `operation`).
  * Manejo manual de códigos de estado HTTP (`200 OK` y `400 Bad Request`).

### - Sección 4: Middleware Personalizado y Autenticación
* **Proyecto:** Sistema de Login mediante Middleware.
* **Descripción:** Desarrollo de un middleware personalizado con extensiones para interceptar peticiones HTTP POST.
* **Conceptos clave:**
  * Estructuración de clases middleware independientes (`RequestDelegate`, método `Invoke`).
  * Métodos de extensión para registrar el middleware en el pipeline (`UseMiddleware<T>`).
  * Lectura y parseo del cuerpo de la petición (`Request.Body`) enviado en formato `x-www-form-urlencoded`.
  * Aplicación de validaciones defensivas (*Fail Fast* / *Guard Clauses*) para múltiples errores.

### - Sección 5: Routing y Restricciones de Ruta
- **Proyecto:** Servicio de Países basado en Enrutamiento.
- **Descripción:** Implementación de endpoints interactivos para consultar catálogos y recursos mediante parámetros dinámicos en la URL.
- **Conceptos clave:**
  * Configuración de rutas utilizando `UseEndpoints` y `MapGet`.
  * Captura y uso de parámetros de ruta esenciales.
  * Aplicación de restricciones de ruta (*Route Constraints*, tipos de datos y rangos numéricos).
  * Control robusto de códigos de estado HTTP (`200 OK`, `400 Bad Request`, `404 Not Found`).
 
 ### - Sección 6: Controllers & IActionResult
- **Proyecto:** Aplicación Bancaria (Bank API).
- **Descripción:** Desarrollo de controladores web utilizando Attribute Routing y respuestas basadas en `IActionResult`.
- **Conceptos clave:**
  * Creación y organización de controladores mediante la clase base `Controller`.
  * Uso de Attribute Routing (`[Route(...)]`) para mapear rutas de forma declarativa.
  * Implementación de diferentes tipos de resultados de acción: `Content`, `Json`, y `File` (archivos estáticos desde `wwwroot`).
  * Manipulación de datos utilizando objetos anónimos.
  * Extracción y validación manual de parámetros desde la ruta (`RouteValues`) con restricciones de tipo (`:int`) y control de estados HTTP (`200`, `400`, `404`).

### - Sección 7: Model Binding y Validaciones con Data Annotations
- **Proyecto:** Sistema de Procesamiento de Órdenes (E-commerce Order API).
- **Descripción:** Recepción, validación y procesamiento de órdenes de compra con colecciones anidadas de productos utilizando Model Binding avanzado.
- **Conceptos clave:** 
  * Aplicación de **Data Annotations** para validaciones declarativas y restricciones de tipos de datos.
  * Uso del atributo `[Bind]` para proteger propiedades del sistema e impedir la inyección de valores por parte del usuario (`OrderNo`).
  * Enlace de colecciones (*Collection Binding*) para mapear listas complejas desde formularios (`Products[0].ProductCode`, etc.).
  * Validación de reglas de negocio personalizadas (coincidencia de `InvoicePrice` con el costo total de los productos y validación de fechas mínimas).
  * Manejo del `ModelState` combinando respuestas con `JsonResult` (`200 OK`) y `ContentResult` (`400 Bad Request`).
 
 ### - Sección 8: Views y Motor Razor
- **Proyecto:** Aplicación de Clima (Weather App).
- **Descripción:** Desarrollo de una aplicación web MVC basada en vistas fuertemente tipadas y el motor de vistas Razor para mostrar información meteorológica de múltiples ciudades.
- **Conceptos clave:**
  * Creación de vistas fuertemente tipadas mediante `@model` para recibir objetos únicos o colecciones (`IEnumerable<CityWeather>`).
  * Uso de Attribute Routing avanzado en controladores combinando parámetros opcionales en la ruta (`/weather/{cityCode?}`).
  * Implementación de **funciones locales en Razor** para calcular dinámicamente clases CSS e inyectar estilos de fondo condicionales según el valor de la temperatura.
  * Configuración global de directivas y espacios de nombres mediante `_ViewImports.cshtml`.
 
  ### Sección 9: Layout Views y Organización de Vistas Razor

- **Proyecto:** Refactorización de la aplicación de Clima (Weather App).
- **Descripción:** Modificación del proyecto de la sección anterior para centralizar el diseño HTML compartido y reducir la duplicación de código entre las vistas `Index` y `Details`.
- **Conceptos clave:**
  - Creación de `_Layout.cshtml` para definir la estructura común de las páginas.
  - Uso de `@RenderBody()` para insertar el contenido específico de cada vista dentro del layout.
  - Configuración de `_ViewStart.cshtml` para establecer el layout predeterminado.
  - Uso de `_ViewImports.cshtml` para centralizar namespaces y habilitar Tag Helpers.
  - Personalización de títulos mediante `ViewData["Title"]`.
  - Separación entre el diseño general de la aplicación y el contenido de cada vista.
  - Refactorización de Razor Views para mejorar la reutilización, la consistencia visual y el mantenimiento del código.
 
### - Sección 10: Partial Views
- **Proyecto:** Refactorización de la aplicación de Clima (Weather App).
- **Descripción:** Modificación del proyecto para centralizar el diseño HTML compartido, modularizar tarjetas repetitivas mediante vistas parciales y optimizar la mantenibilidad del código Razor.
- **Conceptos clave:**
  - Creación de `_Layout.cshtml` para definir la estructura común de las páginas.
  - Uso de `@RenderBody()` para insertar el contenido específico de cada vista.
  - Creación e invocación de **Partial Views** (`_CityPartial.cshtml`) mediante `@await Html.PartialAsync()` dentro de bucles.
  - Configuración de `_ViewStart.cshtml` y `_ViewImports.cshtml` para establecer layouts y directivas globales.
  - Implementación de funciones locales en Razor para calcular dinámicamente clases CSS e inyectar estilos de fondo condicionales según la temperatura.
 
 ### - Sección 11: View Components
- **Proyecto:** Refactorización avanzada de la aplicación de Clima (Weather App).
- **Descripción:** Modificación del proyecto para reemplazar las vistas parciales por **View Components**, encapsulando la lógica de presentación y los datos de las tarjetas de clima en componentes reutilizables e independientes.
- **Conceptos clave:**
  - Creación de clases `ViewComponent` personalizadas heredando de la clase base del framework.
  - Implementación del método `Invoke` / `InvokeAsync` para procesar y retornar datos al componente.
  - Configuración de la estructura de carpetas obligatoria para vistas de componentes (`Views/Shared/Components/[Name]/Default.cshtml`).
  - Invocación asíncrona de componentes desde vistas Razor mediante `@await Component.InvokeAsync()`.
  - Separación óptima de responsabilidades (mini-controladores para elementos de la UI).
 
  ### - Sección 12: Dependency Injection
- **Proyecto:** Refactorización de la aplicación de Clima (Weather App).
- **Descripción:** Evolución de la aplicación MVC para separar la lógica de acceso a los datos en servicios independientes e incorporar inyección de dependencias mediante interfaces.
- **Conceptos clave:**
  * Creación de bibliotecas de clases independientes para `Models`, `ServiceContracts` y `Services`.
  * Definición de la interfaz `IWeatherService` para establecer el contrato de las operaciones meteorológicas.
  * Implementación de `WeatherService` para centralizar los datos y las consultas de ciudades.
  * Uso de `GetWeatherDetails()` para recuperar todas las ciudades y `GetWeatherByCityCode()` para consultar una ciudad por su código.
  * Aplicación de **Constructor Injection** en `HomeController`, recibiendo `IWeatherService` mediante el constructor.
  * Registro de la implementación concreta en el contenedor de dependencias de ASP.NET Core con `AddScoped<IWeatherService, WeatherService>()`.
  * Desacoplamiento entre controladores y servicios, favoreciendo la mantenibilidad, la reutilización y la posibilidad de reemplazar implementaciones.
  * Mantenimiento de las vistas fuertemente tipadas, el enrutamiento por atributos y los View Components desarrollados en las secciones anteriores.
 
### - Sección 14: Configuration & HttpClient Work 24
- **Proyecto:** Enlaces de Redes Sociales (Social Media Links).
- **Descripción:** Desarrollo de una aplicación web MVC que muestra enlaces a redes sociales leídos desde archivos de configuración, con valores distintos según el entorno de ejecución.
- **Conceptos clave:**
  * Configuración mediante `appsettings.json` y `appsettings.Development.json`, entendiendo que ASP.NET Core combina ambos archivos y el específico del entorno solo sobrescribe las claves que define.
  * Lectura de configuración tipada mediante el **Options Pattern** (`Configure<T>` e `IOptions<SocialMediaLinksOptions>`).
  * Envío de los datos del controlador a la vista mediante `ViewBag`.
  * Uso del tag helper `<environment>` (`exclude="Development"`) para mostrar u ocultar contenido según el entorno.
  * Habilitación de Tag Helpers con `@addTagHelper` en `_ViewImports.cshtml`.
  * Cambio de entorno mediante `ASPNETCORE_ENVIRONMENT` o `dotnet run --environment`.
  * Controlador con Attribute Routing, Layout Views y `_ViewStart.cshtml`.

### - Sección 14: Configuration & HttpClient Work 25
- **Proyecto:** Aplicación de Cotización de Acciones (Stocks App).
- **Descripción:** Desarrollo de una aplicación web MVC que muestra el precio de una acción con actualizaciones en tiempo real, consumiendo la API de Finnhub.io y aplicando una arquitectura n-layer.
- **Conceptos clave:**
  * Consumo de APIs externas mediante `IHttpClientFactory` y `HttpClient`, deserializando la respuesta JSON a `Dictionary<string, object>`.
  * Definición de la interfaz `IFinnhubService` e implementación de `FinnhubService` con los métodos `GetCompanyProfile()` y `GetStockPriceQuote()`.
  * Lectura de configuración tipada mediante el **Options Pattern** (`IOptions<TradingOptions>`) desde `appsettings.json`.
  * Almacenamiento seguro del token de la API con **User Secrets**.
  * Uso del view model `StockTrade` para enviar datos del controlador a la vista fuertemente tipada.
  * Controlador con Attribute Routing (`Trade/Index`) y manejo seguro de respuestas con `TryGetValue`.
  * Conexión a un **WebSocket** desde JavaScript para actualizar el precio sin recargar la página, con cancelación de la suscripción al cerrar la página.
  * Uso de campos ocultos y secciones Razor (`@section`).
 
### - Sección 15: xUnit
- **Proyecto:** Refactorización de la aplicación de Cotización de Acciones (Stocks App).
- **Descripción:** Ampliación de la aplicación de la Sección 14 incorporando entidades, DTOs y un servicio de órdenes de compra y venta, con validaciones declarativas y pruebas unitarias con xUnit.
- **Conceptos clave:**
  * Creación de las entidades `BuyOrder` y `SellOrder` con validaciones mediante **Data Annotations** (`[Required]` y `[Range]`).
  * Uso del patrón **DTO** con clases Request y Response (`BuyOrderRequest`, `BuyOrderResponse`, `SellOrderRequest`, `SellOrderResponse`) para separar los datos de entrada y salida de los servicios.
  * Creación de un atributo de validación personalizado (`MinimumDateAttribute`) y de un helper de validación basado en `Validator.TryValidateObject`.
  * Definición de la interfaz `IStocksService` e implementación de `StocksService` con `CreateBuyOrder()`, `CreateSellOrder()`, `GetBuyOrders()` y `GetSellOrders()`, almacenando las órdenes en listas en memoria.
  * Conversión entre entidades y DTOs mediante métodos de extensión.
  * Inyección de `IStocksService`, `IFinnhubService` e `IOptions<TradingOptions>` en el controlador, sin exponer las entidades fuera de la capa de servicios.
  * Registro del servicio con `AddSingleton` para conservar los datos en memoria entre peticiones.
  * **Pruebas unitarias con xUnit**: `[Fact]`, `[Theory]` con `[InlineData]` y `Assert.ThrowsAsync` para validar excepciones (`ArgumentNullException`, `ArgumentException`) y el comportamiento correcto del servicio.

### - Sección 16: CRUDS Operations
- **Proyecto:** Refactorización de la aplicación de Cotización de Acciones (Stocks App).
- **Descripción:** Ampliación de la aplicación de las secciones 14 y 16 incorporando formularios para crear órdenes de compra y venta, una vista de listado de órdenes y navegación entre páginas, manteniendo el precio en vivo mediante WebSocket.
- **Conceptos clave:**
  * Nuevas acciones en `TradeController` con Attribute Routing: `Orders()` (GET), `BuyOrder()` y `SellOrder()` (POST).
  * **Model Binding** con DTOs (`BuyOrderRequest`, `SellOrderRequest`), asignando la fecha en el controlador y revalidando con `ModelState.Clear()` y `TryValidateModel()`.
  * Manejo de errores de validación devolviendo la vista `Index` con los datos recibidos mediante `View("Index", modelo)`, y patrón **Post/Redirect/Get** cuando la orden es válida.
  * Nuevo view model `Orders` y vista fuertemente tipada `Trade/Orders.cshtml` con listados de compras y ventas.
  * Formulario único con dos botones hacia rutas distintas mediante los tag helpers `asp-controller` y `asp-action`, y campos ocultos para enviar los datos de la orden.
  * Actualización del precio oculto desde JavaScript para enviar siempre el valor más reciente del WebSocket.
  * Lectura de `DefaultOrderQuantity` mediante el **Options Pattern**.
  * Barra de navegación en `_Layout.cshtml` con resaltado del enlace activo.
  * Formato de fechas y montos con `CultureInfo.InvariantCulture` y configuración de **Request Localization** (`en-US`) para el correcto enlace de decimales.

### - Sección 17: Tag Helpers
- **Proyecto:** Refactorización de la aplicación de Cotización de Acciones (Stocks App).
- **Descripción:** Modificación de las vistas de la aplicación de las secciones 14, 16 y 17 para reemplazar el HTML escrito a mano por Tag Helpers, manteniendo la misma funcionalidad.
- **Conceptos clave:**
  * Uso de `asp-for` en inputs y labels para generar `id`, `name`, `value` y `type` a partir de las propiedades del modelo, evitando errores en el *model binding*.
  * Uso de `asp-controller` y `asp-action` en enlaces y botones, generando rutas y atributos `formaction` hacia distintas acciones desde un mismo formulario.
  * Uso de `asp-validation-summary` para mostrar los errores de `ModelState` sin armarlos manualmente en el controlador.
  * Uso de `asp-format` para controlar el formato de los valores de los campos.
  * Uso de `asp-append-version` para invalidar la caché del navegador cuando cambian los archivos estáticos.
  * Habilitación de Tag Helpers con `@addTagHelper` en `_ViewImports.cshtml`.
  * Actualización del JavaScript del WebSocket para adaptarse al `id` generado por `asp-for`.
  * Simplificación del controlador al delegar la presentación de errores en la vista.
 
### - Sección 18: Entity Framework Core y Generación de PDF
- **Proyecto:** Refactorización de la aplicación de Cotización de Acciones (Stocks App).
- **Descripción:** Ampliación de la aplicación de las secciones anteriores para guardar las órdenes de compra y venta en una base de datos SQL Server mediante Entity Framework Core, y exportar el listado de órdenes en formato PDF.
- **Conceptos clave:**
  * Creación de `StockMarketDbContext` con `DbSet<BuyOrder>` y `DbSet<SellOrder>`, configurado con una cadena de conexión desde `appsettings.json` y registrado con `AddDbContext` y `UseSqlServer`.
  * Refactorización de `StocksService` para usar el `DbContext` con `SaveChangesAsync()`, `ToListAsync()` y `AsNoTracking()`.
  * Cambio del registro del servicio de `AddSingleton` a `AddScoped` por el ciclo de vida del `DbContext`.
  * Creación de la base de datos y las tablas mediante **migraciones** de EF Core.
  * Nueva acción `OrdersPDF()` y vista `OrdersPDF.cshtml` que generan un PDF con **Rotativa** (`ViewAsPdf`), con márgenes y orientación configurables.
  * Vista sin layout y con estilos en línea para una correcta conversión a PDF.
  * Combinación de las listas de compras y ventas en una sola tabla ordenada por fecha.
  * Adaptación de las pruebas unitarias con xUnit usando el proveedor **EF Core InMemory**.
 
### - Sección 19: Advanced Unit Testing: Moq & Repository Pattern
- **Proyecto:** Refactorización de la aplicación de Cotización de Acciones (Stocks App).
- **Descripción:** Reorganización de la aplicación de las secciones anteriores en una arquitectura de 4 capas con repositorios, incorporación de una página para explorar acciones populares con un View Component de detalle, y ampliación de las pruebas con Moq, AutoFixture, FluentAssertions y tests de integración.
- **Conceptos clave:**
  * Arquitectura de 4 capas: **UI, BLL, Repository Layer y Data Access Layer**.
  * Implementación del **Patrón Repositorio** (`IFinnhubRepository`, `IStocksRepository`) y servicios que delegan en los repositorios.
  * Consumo de nuevos endpoints de Finnhub (`GetStocks`, `SearchStocks`) con `Uri.EscapeDataString` y verificación del código de estado HTTP.
  * Nuevo `StocksController` con la acción `Explore()` y ruta con parámetro opcional (`Stocks/Explore/{stock?}`).
  * Filtrado de las acciones populares mediante `Top25PopularStocks`, leído con el **Options Pattern**.
  * Creación de `SelectedStockViewComponent` e invocación con `Component.InvokeAsync` para mostrar el detalle de la acción seleccionada.
  * Navegación entre Explore y Trade con `asp-route-*` y cambio de ruta de `Trade/Index` para recibir el símbolo.
  * Pruebas unitarias con **Moq** para simular repositorios y servicios, **AutoFixture** para generar datos y **FluentAssertions** para las aserciones.
  * Pruebas de **integración** con `WebApplicationFactory`, HtmlAgilityPack y un repositorio falso para aislar la API externa.


### Sección 20: Próximamente
---
## 🛠️ Tecnologías y herramientas

- C#
- .NET 10
- ASP.NET Core
- ASP.NET Core MVC
- Razor Views
- HTML5 y CSS3
- Visual Studio
- Git y GitHub

---

*Repositorio de aprendizaje práctico construido con C# y ASP.NET Core (.NET 10).*
