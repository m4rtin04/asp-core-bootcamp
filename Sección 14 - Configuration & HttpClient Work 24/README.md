# Sección 14 - Configuration & HttpClient Work 24 (Social Media Links)

Ejercicio práctico de desarrollo web utilizando **ASP.NET Core MVC**, centrado en el manejo de la **configuración por entornos**, el **Options Pattern** y el tag helper `<environment>`.

---

## 🚀 Descripción del Proyecto

Aplicación web que muestra los enlaces a las redes sociales (Facebook, Instagram, Twitter y YouTube) leídos desde los archivos de configuración. Los enlaces cambian según el entorno de ejecución:

- **Entornos distintos de Development:** se muestran los 4 enlaces, con los valores de `appsettings.json` (cuentas de Microsoft).
- **Development:** se muestran solo 3 enlaces (sin Instagram), con los valores sobrescritos desde `appsettings.Development.json` (cuentas de dotnet).

Los principales componentes incluyen:

- Definición de la sección `SocialMediaLinks` en `appsettings.json` y su sobrescritura parcial en `appsettings.Development.json`.
- Creación de la clase `SocialMediaLinksOptions` (`Facebook`, `Instagram`, `Twitter`, `Youtube`) para representar la configuración de forma tipada.
- Registro de la configuración como servicio mediante `Configure<SocialMediaLinksOptions>(...)` en `Program.cs`, aplicando el **Options Pattern**.
- Inyección de `IOptions<SocialMediaLinksOptions>` en `HomeController` y envío de los enlaces a la vista mediante `ViewBag`.
- Controlador con Attribute Routing en la ruta `/`.
- Uso del tag helper `<environment exclude="Development">` en la vista para ocultar el enlace de Instagram en Development.
- Habilitación de los Tag Helpers en `_ViewImports.cshtml` con `@addTagHelper`.
- Uso de `_Layout.cshtml`, `_ViewStart.cshtml` y la hoja de estilos `StyleSheet.css` provista por el curso.

> **Detalle importante:** ASP.NET Core **combina** `appsettings.json` con `appsettings.{Environment}.json`; el segundo solo sobrescribe las claves que define. Como `appsettings.Development.json` no incluye `Instagram`, esa propiedad conserva el valor del archivo base en lugar de quedar en `null`. Por eso el ocultamiento se resuelve en la vista con `<environment>`.

---

## 🏗️ Arquitectura del Proyecto

- **Configuración (`appsettings.json` y `appsettings.Development.json`):** almacenan los enlaces para cada entorno.
- **Options (`SocialMediaLinksOptions`):** representa la sección de configuración como una clase tipada.
- **Controller (`HomeController`):** recibe `IOptions<SocialMediaLinksOptions>` y entrega los enlaces a la vista.
- **View (`Home/Index.cshtml`):** genera los enlaces y decide cuáles mostrar según el entorno.

```
SocialMediaApp/
├── Controllers/        → HomeController
├── Options/            → SocialMediaLinksOptions
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/_Layout.cshtml
│   └── Home/Index.cshtml
├── appsettings.json
├── appsettings.Development.json
└── wwwroot/StyleSheet.css
```

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC.
- Configuración con `appsettings.json` y archivos por entorno.
- Options Pattern (`IOptions<T>` y `Configure<T>`).
- Variable de entorno `ASPNETCORE_ENVIRONMENT`.
- Tag Helper `<environment>`.
- Attribute Routing.
- `ViewBag` para enviar datos a la vista.
- Layout Views, `_ViewStart` y `_ViewImports`.

---

## ▶️ Cómo ejecutar el proyecto

Abrir la solución desde el archivo `.sln` en Visual Studio, o ejecutar desde una terminal situada en el directorio del proyecto:

```bash
dotnet run
```

Visual Studio y `dotnet run` usan el entorno **Development** por defecto (según `launchSettings.json`), por lo que se verán 3 enlaces. Para ver los 4 enlaces, ejecutar en otro entorno:

```bash
dotnet run --environment Production
```

En PowerShell también se puede definir la variable antes de ejecutar:

```powershell
$env:ASPNETCORE_ENVIRONMENT="Production"
dotnet run
```
