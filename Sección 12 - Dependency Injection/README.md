# Sección 12 - Dependency Injection (Weather App)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, centrado en la separación de responsabilidades mediante **Services, Service Contracts y Dependency Injection**.

---

## 🚀 Descripción del Proyecto

Continuación de la aplicación meteorológica desarrollada en las secciones anteriores del curso de Harsha.

El proyecto muestra información meteorológica de diferentes ciudades (Londres, Nueva York y París), permitiendo consultar un listado general y acceder a los detalles de una ciudad específica.

En esta etapa, se refactorizó la aplicación para separar la lógica de acceso a los datos del controlador, implementando una arquitectura basada en servicios y bibliotecas de clases independientes.

Los principales cambios incluyen:

- Creación de una biblioteca de clases **Models** para centralizar el modelo `CityWeather` y permitir su reutilización entre los distintos proyectos.
- Creación de la biblioteca **ServiceContracts**, que contiene la interfaz `IWeatherService` y define las operaciones disponibles para consultar la información meteorológica.
- Creación de la biblioteca **Services**, que contiene la implementación concreta `WeatherService` y centraliza los datos meteorológicos de las ciudades.
- Implementación de los métodos `GetWeatherDetails()` y `GetWeatherByCityCode(string cityCode)` para obtener la lista completa de ciudades y consultar una ciudad específica por su código.
- Inyección de dependencias mediante constructor (*Constructor Injection*) en el controlador `HomeController`, utilizando la interfaz `IWeatherService` en lugar de instanciar directamente la clase `WeatherService`.
- Registro del servicio en el contenedor de dependencias de ASP.NET Core mediante `AddScoped<IWeatherService, WeatherService>()` en `Program.cs`.
- Mantenimiento de las vistas fuertemente tipadas de Razor y los **View Components** desarrollados en la sección anterior.
- Manejo de códigos de ciudad inexistentes mediante una respuesta de error apropiada.

---

## 🏗️ Arquitectura del Proyecto

La solución se organiza en proyectos independientes, cada uno con una responsabilidad específica:

- **Proyecto MVC (`Sección_12___Dependency_Injection`):** contiene los controladores, las vistas, los View Components y la configuración de la aplicación web.
- **Models:** contiene la clase `CityWeather`, que representa la información meteorológica de cada ciudad.
- **ServiceContracts:** define la interfaz `IWeatherService`, que establece el contrato de las operaciones disponibles.
- **Services:** implementa la interfaz `IWeatherService` mediante la clase `WeatherService`, encargada de almacenar y consultar los datos.

Esta organización permite desacoplar el controlador de la implementación concreta del servicio, facilitando el mantenimiento, las pruebas y la posibilidad de reemplazar la implementación sin modificar la lógica del controlador.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC.
- C# y bibliotecas de clases (*Class Libraries*).
- Separación de responsabilidades.
- Interfaces y contratos de servicios.
- Inyección de dependencias mediante constructor.
- Contenedor de dependencias integrado de ASP.NET Core.
- Registro de servicios con `AddScoped`.
- Vistas fuertemente tipadas con Razor.
- Attribute Routing.
- View Components y reutilización de componentes de interfaz.

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución completa desde el archivo `.sln` en Visual Studio, asegurándose de que todos los proyectos estén cargados y sus referencias estén configuradas correctamente.

También puede ejecutarse desde una terminal situada en el directorio del proyecto MVC:

```bash
dotnet run
