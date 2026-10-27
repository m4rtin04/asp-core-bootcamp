# Sección 9 - Layout Views y Organización de Vistas Razor (Weather App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core 10**, centrado en la reutilización de estructuras HTML mediante **Layout Views**, la configuración compartida de vistas Razor y la separación entre el diseño general y el contenido específico de cada página.

---

## 🚀 Descripción del Proyecto

Continuación de la aplicación meteorológica desarrollada en la sección anterior del curso de Harsha.

El proyecto muestra información meteorológica de diferentes ciudades (Londres, Nueva York y París), permitiendo consultar un listado general y acceder a los detalles de una ciudad específica.

En esta etapa, se refactorizaron las vistas `Index.cshtml` y `Details.cshtml` para eliminar la duplicación de código HTML y centralizar la estructura visual en una vista de diseño compartida.

La aplicación mantiene la lógica de presentación basada en la temperatura en Fahrenheit, aplicando clases CSS dinámicas según los rangos definidos.

---

## 🛠️ Cómo probarlo

Puedes ejecutar el proyecto desde Visual Studio o mediante `dotnet run` y acceder a las siguientes rutas desde un navegador web:

### Casos de prueba

- **Listado general de ciudades:**
  - **URL:** `/` o `/weather`
  - **Método:** `GET`
  - **Resultado esperado:** listado de ciudades con sus respectivos datos meteorológicos y enlaces a las páginas de detalles.
  - **Layout:** estructura HTML compartida, encabezado y hoja de estilos centralizados.

- **Detalle de una ciudad específica:**
  - **URL:** `/weather/LDN` (o `/weather/NYC`, `/weather/PAR`)
  - **Método:** `GET`
  - **Resultado esperado:** página con la información detallada de la ciudad seleccionada y el color de fondo correspondiente a su temperatura.
  - **Layout:** reutilización de la misma estructura visual que la página principal.

- **Comprobación del diseño compartido:**
  - Verificar que ambas vistas muestran el encabezado `Weather App`.
  - Comprobar que la hoja de estilos `StyleSheet.css` se carga correctamente.
  - Confirmar que cada página mantiene su título específico mediante `ViewData["Title"]`.

---

## 🧱 Estructura y archivos principales

- **`Views/Shared/_Layout.cshtml`**
  - Define la estructura HTML común de la aplicación.
  - Centraliza las etiquetas `html`, `head` y `body`, el encabezado de la aplicación y la referencia a la hoja de estilos.
  - Utiliza `@RenderBody()` para insertar el contenido específico de cada vista.
  - Utiliza `@ViewData["Title"]` para mostrar el título correspondiente a cada página.

- **`Views/_ViewStart.cshtml`**
  - Configura el layout predeterminado mediante `Layout = "_Layout"`.
  - Permite que las vistas de la carpeta `Views` y sus subcarpetas utilicen el diseño compartido, salvo que se configure otro explícitamente.

- **`Views/_ViewImports.cshtml`**
  - Centraliza directivas Razor compartidas.
  - Permite importar namespaces mediante `@using`.
  - Habilita los Tag Helpers de ASP.NET Core mediante `@addTagHelper`.

- **`Views/Weather/Index.cshtml`**
  - Muestra el listado de ciudades.
  - Utiliza el modelo `IEnumerable<CityWeather>`.
  - Conserva la lógica de presentación y el recorrido de la colección mediante `foreach`.

- **`Views/Weather/Details.cshtml`**
  - Muestra los datos de una ciudad individual.
  - Utiliza el modelo `CityWeather`.
  - Conserva la lógica de presentación correspondiente a los detalles meteorológicos.

---

## 💡 Lo que aprendí en esta sección

- **Layout Views:** creación de una estructura HTML compartida para múltiples vistas MVC.
- **`_Layout.cshtml`:** centralización del encabezado, la hoja de estilos y otros elementos comunes de la interfaz.
- **`@RenderBody()`:** definición del punto donde se inserta el contenido particular de cada vista.
- **`_ViewStart.cshtml`:** configuración del layout predeterminado para evitar asignarlo manualmente en cada vista.
- **`_ViewImports.cshtml`:** centralización de namespaces y Tag Helpers para simplificar las vistas Razor.
- **`ViewData["Title"]`:** personalización del título de cada página sin duplicar la estructura HTML.
- **Separación de responsabilidades:** diferenciación entre el diseño compartido y el contenido específico de `Index` y `Details`.
- **Refactorización de vistas:** eliminación de código HTML repetido para mejorar la organización y el mantenimiento de la aplicación.
- **Vistas fuertemente tipadas:** conservación de los modelos `CityWeather` e `IEnumerable<CityWeather>` según las necesidades de cada vista.

---

## 🎯 Objetivo de aprendizaje

Comprender cómo ASP.NET Core MVC permite reutilizar diseños mediante Razor Layout Views y cómo los archivos de configuración compartida contribuyen a mantener una estructura consistente, organizada y fácil de mantener.

---

*Desarrollado con C#, ASP.NET Core 10 y Razor Views.*
