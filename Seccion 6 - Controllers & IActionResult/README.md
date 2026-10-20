# Sección 6 - Controllers & IActionResult (Aplicación Bancaria)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en la creación de **Controllers** con enrutamiento por atributos (*Attribute Routing*), el uso de los distintos tipos de **`IActionResult`** (Content, Json, File y códigos de estado HTTP) y la validación de parámetros de ruta.

---

## 🚀 Descripción del Proyecto
Este proyecto simula una aplicación bancaria básica estructurada mediante controladores (`Controller`). Gestiona detalles de cuentas bancarias utilizando datos hardcodeados en objetos anónimos y responde a diferentes rutas HTTP con formatos específicos (texto plano, JSON, archivos PDF) aplicando validaciones defensivas y códigos de estado adecuados.

---

## 🛠️ Cómo probarlo

Puedes probar la aplicación utilizando herramientas como **Postman**, **cURL**, un navegador web o un cliente HTTP similar.

### Casos de prueba:

- **Mensaje de bienvenida (Raíz):**
  * **URL:** `/`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):** `Welcome to the Best Bank`

- **Obtener detalles de la cuenta bancaria (JSON):**
  * **URL:** `/account-details`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):**
    ```json
    {
      "accountNumber": 1001,
      "accountHolderName": "Example Name",
      "currentBalance": 5000
    }
    ```

- **Obtener extracto bancario (Archivo PDF):**
  * **URL:** `/account-statement`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):** Archivo PDF (`statement.pdf` desde `wwwroot`)

- **Consultar balance actual con número de cuenta válido:**
  * **URL:** `/get-current-balance/1001`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):** `5000`

- **Falta el número de cuenta en la ruta:**
  * **URL:** `/get-current-balance`
  * **Método:** `GET`
  * **Respuesta HTTP:** `404 Not Found`
  * **Respuesta Body (output):** `Account number should be supplied in the URL.`

- **Número de cuenta incorrecto:**
  * **URL:** `/get-current-balance/10`
  * **Método:** `GET`
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body (output):** `Account number should be 1001.`

---

## 💡 Lo que aprendí en esta sección:

- **Estructura de Controladores:** Creación de clases que heredan de `Controller` para organizar la lógica de negocio y las respuestas HTTP de manera modular.
- **Attribute Routing:** Uso del atributo `[Route(...)]` tanto a nivel de controlador como de métodos para definir rutas limpias y explícitas.
- **Uso avanzado de `IActionResult`:** Implementación de distintos resultados de acción como `ContentResult` (`Content`), `JsonResult` (`Json`), y `FileResult` (`File`).
- **Manejo de Objetos Anónimos:** Creación rápida de estructuras de datos tipadas en tiempo de compilación para representar entidades simples (como cuentas bancarias).
- **Lectura y Validación de Parámetros de Ruta:** Extracción de valores desde `HttpContext.Request.RouteValues`, aplicación de restricciones de tipo (`:int`) y retorno de códigos de estado HTTP precisos (`200 OK`, `400 Bad Request`, `404 Not Found`).
