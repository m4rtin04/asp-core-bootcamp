# Sección 3 - HTTP y Middleware

Ejercicio práctico de calculadora basado en Web utilizando ASP.NET Core y Request Delegates (`app.Run`).

## 🚀 Cómo probarlo
Envía una petición GET a la raíz (`/`) con los parámetros `firstNumber`, `secondNumber` y `operation` en la URL.

### Ejemplos de uso:
* **Suma:** `/?firstNumber=5&secondNumber=2&operation=add` (Respuesta: `7`)
* **Multiplicación:** `/?firstNumber=5&secondNumber=2&operation=multiply` (Respuesta: `10`)
* **Error (Operación inválida):** `/?firstNumber=5&secondNumber=2&operation=something` (Código HTTP: `400`)

## 💡 Lo que aprendí en esta sección:
* Manejo del ciclo de vida de peticiones con `HttpContext`.
* Lectura y validación de parámetros de consulta (`Query Strings`).
* Control y manipulación de códigos de estado HTTP (`200 OK` y `400 Bad Request`).
