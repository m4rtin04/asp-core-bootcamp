# Sección 5 - Routing (Ejercicio de Países)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en el uso del sistema de **Routing** (`MapGet`, `UseEndpoints`), restricciones de ruta (*Route Constraints*) y validación de parámetros para servir detalles dinámicos de países.

---

## 🚀 Descripción del Proyecto
Este proyecto implementa una API o servicio web simple que gestiona un catálogo estático de países almacenados en un diccionario. Utiliza enrutamiento avanzado basado en endpoints para manejar diferentes escenarios según la ruta y el ID del país solicitado, aplicando validaciones estrictas y devolviendo los códigos de estado HTTP adecuados.

---

## 🛠️ Cómo probarlo

Puedes probar la aplicación utilizando herramientas como **Postman**, **cURL**, un navegador web o un cliente HTTP similar.

### Casos de prueba:

- **Listar todos los países:**
  * **URL:** `/countries`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):**
    ```text
    1. 1, United States
    2. 2, Canada
    3. 3, United Kingdom
    4. 4, India
    5. 5, Japan
    ```

- **Obtener un país existente por ID:**
  * **URL:** `/countries/1`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body:** `United States`

- **ID fuera del rango existente (1 al 5):**
  * **URL:** `/countries/6`
  * **Método:** `GET`
  * **Respuesta HTTP:** `404 Not Found`
  * **Respuesta Body:** `[No Country]`

- **ID superior a 100 (Validación de límite superior):**
  * **Url:** `/countries/101`
  * **Método:** `GET`
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body:** `The CountryID should be between 1 and 100`

- **Parámetro con formato incorrecto (Restricción de tipo entero):**
  * **URL:** `/countries/abc`
  * **Método:** `GET`
  * **Respuesta HTTP:** `404 Not Found` *(El motor de rutas descarta la ruta al no cumplir con la restricción `int`)*

---

## 💡 Lo que aprendí en esta sección:

- **Uso de Endpoint Routing:** Configuración de rutas mediante `UseRouting()` y `UseEndpoints()` utilizando métodos específicos como `MapGet()`.
- **Parámetros de Ruta Esenciales:** Cómo capturar valores dinámicos desde la URL (ej. `{countryID}`) e inyectarlos directamente en los delegados de los endpoints.
- **Restricciones de Ruta (*Route Constraints*):** Aplicación de restricciones nativas de ASP.NET Core (como restricciones de tipo `int` y validaciones de rango con `{countryID:range(1,100)}`) para filtrar peticiones inválidas a nivel de infraestructura de enrutamiento.
- **Manejo de Respuestas HTTP:** Retorno preciso de códigos de estado (`200 OK`, `400 Bad Request`, `404 Not Found`) combinados con mensajes de texto personalizados según el negocio.
