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

---

---
*Construido con C# y .NET Core.*
