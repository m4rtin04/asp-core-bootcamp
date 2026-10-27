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

### Sección 11: Próximamente
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
