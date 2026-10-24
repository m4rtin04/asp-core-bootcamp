# Sección 8 - Views y Motor Razor (Aplicación de Clima - Weather App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en la implementación de **Vistas fuertemente tipadas**, el motor de vistas **Razor**, funciones locales para lógica de presentación y el uso de **Attribute Routing** en controladores.

---

## 🚀 Descripción del Proyecto
Este proyecto simula una aplicación meteorológica que gestiona un catálogo hardcodeado de ciudades (Londres, Nueva York, París). Permite visualizar un listado completo con el clima actual de todas las regiones o consultar el detalle específico de una ciudad seleccionada a través de parámetros opcionales en la URL, aplicando colores de fondo dinámicos en la interfaz según el rango de temperatura en Fahrenheit.

---

## 🛠️ Cómo probarlo

Puedes probar la aplicación ejecutando el proyecto en Visual Studio / `dotnet run` y navegando a través de un navegador web mediante las siguientes rutas:

### Casos de prueba:

- **Listado general de ciudades (Home):**
  * **URL:** `/` o `/weather`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):** Vista renderizada con tarjetas contenedoras para cada ciudad, mostrando sus respectivos datos meteorológicos y un enlace hacia sus detalles.

- **Detalle de una ciudad específica:**
  * **URL:** `/weather/LDN` (o `/weather/NYC`, `/weather/PAR`)
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK`
  * **Respuesta Body (output):** Vista detallada ampliada con la información de la ciudad seleccionada y su color de fondo condicional.

- **Código de ciudad inválido o inexistente:**
  * **URL:** `/weather/INVALIDO`
  * **Método:** `GET`
  * **Respuesta HTTP:** `200 OK` (o código de error controlado)
  * **Respuesta Body (output):** Página de error controlada o mensaje indicando que el código de ciudad no existe, previniendo excepciones en servidor.

---

## 💡 Lo que aprendí en esta sección:

- **Vistas Fuertemente Tipadas:** Envío y recepción segura de modelos individuales (`CityWeather`) o colecciones (`IEnumerable<CityWeather>`) desde el controlador hacia las vistas Razor mediante la directiva `@model`.
- **Sintaxis y Motor Razor:** Integración limpia de código C# en plantillas HTML utilizando bloques de código y expresiones evaluadas en tiempo de ejecución.
- **Funciones Locales en Vistas:** Creación de funciones dentro de bloques Razor para encapsular lógica de presentación (como evaluar rangos numéricos de temperatura y retornar clases CSS específicas).
- **Attribute Routing Flexible:** Configuración de rutas declarativas con parámetros opcionales (`{cityCode?}`) para manejar múltiples comportamientos lógicos en una misma acción del controlador.
- **Optimización con `_ViewImports`:** Centralización de espacios de nombres y Tag Helpers compartidos para evitar repeticiones en las vistas del proyecto.
