# Sección 11 - View Components (Weather App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core**, centrado en la modularización avanzada de la interfaz de usuario mediante **View Components**.

---

## 🚀 Descripción del Proyecto

Continuación y refactorización de la aplicación meteorológica desarrollada en las secciones anteriores del curso de Harsha.

El proyecto muestra información meteorológica de diferentes ciudades (Londres, Nueva York y París), permitiendo consultar un listado general y acceder a los detalles de una ciudad específica.

En esta etapa, se refactorizó la aplicación para:
- Extraer la tarjeta de clima individual convirtiéndola en un **View Component** (`CityWeatherViewComponent`) en lugar de una vista parcial tradicional.
- Implementar la lógica del componente mediante una clase independiente que hereda de `ViewComponent` y su método `Invoke`.
- Organizar la vista del componente dentro de la convención de carpetas requerida (`Views/Shared/Components/CityWeather/Default.cshtml`).
- Invocar el componente de forma asíncrona dentro del bucle `foreach` en la vista `Index` utilizando `@await Component.InvokeAsync()`.
- Mantener las funciones locales en Razor para determinar dinámicamente las clases CSS de fondo según la temperatura en Fahrenheit.

---

## 🛠️ Cómo probarlo

Puedes ejecutar el proyecto desde Visual Studio o mediante `dotnet run` y acceder a las siguientes rutas desde un navegador web:

### Casos de prueba

- **Listado general de ciudades:**
  - **URL:** `/` o `/weather`
  - **Método:** `GET`
  - **Resultado esperado:** listado de ciudades renderizado mediante el **View Component** en cada iteración, con sus respectivos datos meteorológicos y enlaces a detalles.

- **Detalle de una ciudad específica:**
  - **URL:** `/weather/LDN` (o `/weather/NYC`, `/weather/PAR`)
  - **Método:** `GET`
  - **Resultado esperado:** página con la información detallada de la ciudad seleccionada y el color de fondo correspondiente a su temperatura.

- **Comprobación del diseño y View Components:**
  - Verificar que el encabezado, el layout y la hoja de estilos `StyleSheet.css` se cargan correctamente.
  - Comprobar que la inyección de datos y el renderizado a través del componente de vista funcionan de forma encapsulada y limpia.
