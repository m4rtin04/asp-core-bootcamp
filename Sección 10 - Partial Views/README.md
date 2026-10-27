# Sección 10 - Partial Views y Organización de Vistas Razor (Weather App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en la reutilización de estructuras HTML mediante **Layout Views**, **Partial Views** y la configuración compartida de vistas Razor.

---

## 🚀 Descripción del Proyecto

Continuación de la aplicación meteorológica desarrollada en las secciones anteriores del curso de Harsha.

El proyecto muestra información meteorológica de diferentes ciudades (Londres, Nueva York y París), permitiendo consultar un listado general y acceder a los detalles de una ciudad específica.

En esta etapa, se refactorizaron las vistas para:
- Modularizar la tarjeta de cada ciudad extrayéndola en una **Partial View** (`_CityPartial.cshtml`) reutilizable.
- Invocar la vista parcial dentro del bucle `foreach` en la vista `Index` utilizando `@await Html.PartialAsync()`.
- Aplicar funciones locales en Razor para determinar dinámicamente las clases CSS de fondo según la temperatura en Fahrenheit.

---

## 🛠️ Cómo probarlo

Puedes ejecutar el proyecto desde Visual Studio o mediante `dotnet run` y acceder a las siguientes rutas desde un navegador web:

### Casos de prueba

- **Listado general de ciudades:**
  - **URL:** `/` o `/weather`
  - **Método:** `GET`
  - **Resultado esperado:** listado de ciudades renderizado mediante la vista parcial en cada iteración, con sus respectivos datos meteorológicos y enlaces a detalles.

- **Detalle de una ciudad específica:**
  - **URL:** `/weather/LDN` (o `/weather/NYC`, `/weather/PAR`)
  - **Método:** `GET`
  - **Resultado esperado:** página con la información detallada de la ciudad seleccionada y el color de fondo correspondiente a su temperatura.

- **Comprobación del diseño compartido y parciales:**
  - Verificar que el encabezado y la hoja de estilos `StyleSheet.css` se cargan correctamente en todas las páginas.
  - Comprobar la correcta modularización e inyección de datos a través de las vistas parciales.
