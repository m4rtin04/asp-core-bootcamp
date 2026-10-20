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

---
*Construido con C# y .NET Core.*
