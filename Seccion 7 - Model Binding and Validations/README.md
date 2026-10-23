# Sección 7 - Model Binding y Validaciones (Sistema de Órdenes - E-commerce)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en el uso de **Model Binding**, validación de modelos mediante **Data Annotations**, enlace de colecciones complejas y seguridad en el binding con el atributo `[Bind]`.

---

## 🚀 Descripción del Proyecto
Este proyecto simula un sistema de e-commerce que recibe solicitudes HTTP `POST` con información de órdenes de compra y listas de productos anidados. Valida automáticamente las propiedades obligatorias, verifica reglas de negocio complejas (como la concordancia del precio total de la factura con los productos) y genera un número de orden aleatorio único cuando los datos son correctos.

---

## 🛠️ Cómo probarlo

Puedes probar la aplicación utilizando herramientas como **Postman**, **cURL** o un cliente HTTP similar enviando peticiones mediante `multipart/form-data` o `x-www-form-urlencoded`.

### Casos de prueba:

- **Orden de compra válida (Sin errores de validación):**
  * **URL:** `/order`
  * **Método:** `POST`
  * **Request Body (x-www-form-urlencoded):** 
    `OrderDate=2025-12-31T22:00:00&InvoicePrice=160&Products[0].ProductCode=1&Products[0].Price=15&Products[0].Quantity=10&Products[1].ProductCode=2&Products[1].Price=2&Products[1].Quantity=5`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):** `New Order Number: [número aleatorio generado]`

- **Error de validación (El InvoicePrice no coincide con el costo total):**
  * **URL:** `/order`
  * **Método:** `POST`
  * **Request Body (x-www-form-urlencoded):** 
    `OrderDate=2025-12-31T22:00:00&InvoicePrice=170&Products[0].ProductCode=1&Products[0].Price=15&Products[0].Quantity=10&Products[1].ProductCode=2&Products[1].Price=2&Products[1].Quantity=5`
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body (output):** `InvoicePrice doesn't match with the total cost of the specified products in the order.`

- **Error de validación (Falta la fecha de la orden):**
  * **URL:** `/order`
  * **Método:** `POST`
  * **Request Body (x-www-form-urlencoded):** 
    `InvoicePrice=160&Products[0].ProductCode=1&Products[0].Price=15&Products[0].Quantity=10&Products[1].ProductCode=2&Products[1].Price=2&Products[1].Quantity=5`
  * **Respuesta HTTP:** `400 Bad Request`
  * **Respuesta Body (output):** `OrderDate can't be blank`

---

## 💡 Lo que aprendí en esta sección:

- **Data Annotations:** Implementación de atributos de validación integrados (`[Required]`, `[Range]`, etc.) directamente en las clases modelo.
- **Atributo `[Bind]`:** Uso estratégico para excluir propiedades sensibles o generadas por el sistema (como `OrderNo`) e impedir ataques de sobreexposición de datos (*over-posting*).
- **Collection Binding:** Capacidad de ASP.NET Core para enlazar automáticamente listas y objetos anidados enviados desde formularios web mediante la notación de corchetes (`Products[0].Property`).
- **Validación Personalizada:** Creación de reglas de negocio complejas evaluando la relación entre múltiples propiedades (ej. comparar el `InvoicePrice` con la sumatoria de `Price * Quantity`).
- **Gestión de Errores con `ModelState`:** Retorno estructurado de mensajes de error personalizados a través de `ContentResult` con código HTTP `400` o entrega exitosa mediante `JsonResult` con código HTTP `200`.
