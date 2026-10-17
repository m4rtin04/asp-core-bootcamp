# Sección 4 - Middleware (Ejercicio de Login)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en la creación y uso de **Middlewares personalizados con extensiones** para procesar peticiones HTTP POST y validar credenciales.

---

## 🚀 Descripción del Proyecto
Este proyecto implementa un flujo de autenticación mediante un **Middleware personalizado** (`LoginMiddleware`). El middleware intercepta las solicitudes POST enviadas a la raíz (`/`), analiza el cuerpo de la petición (`request body`) en formato `application/x-www-form-urlencoded`, valida la presencia obligatoria de los campos `email` y `password`, y evalúa si las credenciales coinciden con un usuario administrador predefinido.

---

## 🛠️ Cómo probarlo

Puedes probar la aplicación utilizando herramientas como **Postman**, **cURL** o un cliente HTTP similar.

### Casos de prueba:

* **Login exitoso:**
  * **URL:** `/`
  * **Método:** `POST`
  * **Body (x-www-form-urlencoded):** `email=admin@example.com&password=admin1234`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body:** `Successful login`

* **Credenciales incorrectas:**
  * **URL:** `/`
  * **Método:** `POST`
  * **Body (x-www-form-urlencoded):** `email=manager@example.com&password=wrong-password`
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body:** `Invalid login`

* **Falta de ambos campos (`email` y `password`):**
  * **URL:** `/`
  * **Método:** `POST`
  * **Body (x-www-form-urlencoded):** *[Vacío]*
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body:** 
    ```text
    Invalid input for 'email'
    Invalid input for 'password'
    ```

* **Falta de un campo específico (Ej. Password faltante):**
  * **URL:** `/`
  * **Método:** `POST`
  * **Body (x-www-form-urlencoded):** `email=test@example.com`
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body:** `Invalid input for 'password'`

* **Petición GET estándar:**
  * **URL:** `/`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body:** `No response`

---

## 💡 Lo que aprendí en esta sección:

* **Creación de Custom Middlewares:** Cómo estructurar clases middleware independientes con su respectivo método de extensión (`UseMiddleware<T>`) para mantener el código limpio y modular.
* **Manejo del Pipeline HTTP:** Uso de `RequestDelegate` y el control de flujo con `_next(context)` para decidir cuándo procesar una petición o dejarla pasar al siguiente eslabón.
* **Lectura y Parseo del Request Body:** Extracción de datos enviados mediante solicitudes POST usando `StreamReader` y `QueryHelpers.ParseQuery()`.
* **Validaciones Defensivas (Fail Fast):** Aplicación de cláusulas de guarda para acumular múltiples errores de validación y retornar códigos de estado HTTP precisos (`400 Bad Request` y `200 OK`).
