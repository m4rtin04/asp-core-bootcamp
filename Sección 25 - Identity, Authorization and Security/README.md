# Sección 25 - Identity, Authorization and Security 

Proyecto práctico, hecho para aplicar los temas de la sección de **ASP.NET Core Identity, autorización y seguridad**. No tiene lógica de negocio: toda la app gira alrededor de registrarse, iniciar sesión y controlar quién puede ver cada página.

---

## 🚀 Descripción del Proyecto

**El proyecto** es un portal con tres zonas:

- **Zona pública:** la página de inicio, visible para cualquiera.
- **Zona privada:** el perfil del usuario (donde puede cambiar su nombre visible) y un contenido *Premium* que solo ven las cuentas con cierta antigüedad.
- **Panel de administración:** un *Area* llamada `Admin` donde un administrador ve la lista de usuarios y puede darles o quitarles el rol `Admin`.

Funcionalidades principales:

- Registro, inicio y cierre de sesión con `UserManager` y `SignInManager`.
- Barra de navegación que cambia según el usuario esté autenticado o no, y que resalta el enlace de la página actual.
- Reglas de complejidad de contraseña configuradas en Identity.
- Redirección al login con `ReturnUrl` cuando se intenta entrar a una página protegida, y regreso a esa página tras autenticarse.
- Validación remota (AJAX) para comprobar que el email no esté registrado mientras se completa el formulario.
- Roles `Admin` y `User`: todo usuario nuevo recibe `User`, y un administrador inicial se crea al arrancar la app.
- Política de autorización personalizada `Premium`, con un requisito y un *handler* propios.
- Protección contra XSRF con un filtro global.
- Uso de HTTPS: redirección, HSTS y cookie de autenticación solo por conexión segura.

---

## 🧱 Temas de la sección y dónde se aplican

| Tema | Dónde se aplica |
|------|-----------------|
| Introducción a Identity, modelos y agregar Identity | `ApplicationUser` (hereda de `IdentityUser<Guid>`, con `DisplayName` y `CreatedAt`), `ApplicationRole` y `ApplicationDbContext` (hereda de `IdentityDbContext`). |
| Vista de registro y `UserManager` | `AccountController.Register` con `CreateAsync`. |
| `SignInManager` y vista de login | `AccountController.Login` con `PasswordSignInAsync`, y `Logout` con `SignOutAsync`. |
| Botones de Login/Logout y enlace activo | `_Layout.cshtml`: cambia según `User.Identity.IsAuthenticated` y marca el enlace activo con la ruta actual. |
| Complejidad de contraseña | Opciones de `AddIdentity` en `Program.cs`: mínimo 6 caracteres, dígito, mayúscula y minúscula. |
| Política de autorización y `ReturnUrl` | `[Authorize]` en `AccountController` (con `[AllowAnonymous]` en las acciones públicas) y `Url.IsLocalUrl` al volver. |
| Remote Validation | `[Remote]` en `RegisterDTO.Email`, que llama a `AccountController.IsEmailAvailable`. |
| Enrutamiento convencional | `MapControllerRoute` para las áreas y para la ruta por defecto. |
| Roles y autenticación basada en roles | `[Authorize(Roles = "Admin")]` en el área de administración. |
| Areas | `Areas/Admin` con su propio controlador, vistas, `_ViewImports` y `_ViewStart`. |
| Políticas de autorización personalizadas | `AccountAgeRequirement` y `AccountAgeHandler`, registrados como política `Premium`. |
| HTTPS | `UseHttpsRedirection`, `UseHsts` y `CookieSecurePolicy.Always`. |
| XSRF (partes 1 y 2) | Tag helper de `<form>`, que agrega el token, y `AutoValidateAntiforgeryTokenAttribute` como filtro global. |

---

## 🔐 Cómo funciona la política Premium

La página `/Premium` exige estar autenticado y tener una cuenta con al menos *N* días de antigüedad. Ese valor sale de `PremiumMinimumDays` en `appsettings.json` (7 por defecto).

- `AccountAgeRequirement` guarda la cantidad mínima de días.
- `AccountAgeHandler` busca al usuario, calcula los días desde `CreatedAt` y, si alcanza, llama a `context.Succeed(requirement)`. Si no llama a `Succeed`, la política falla.
- El handler se registra como `Scoped`, porque depende de `UserManager`, que también lo es.

Como el handler consulta la base de datos en cada petición, la antigüedad se evalúa siempre con el dato actual.

---

## 🏗️ Estructura del proyecto

```
Main/
├── Areas/Admin/
│   ├── Controllers/UsersController.cs
│   └── Views/
│       ├── _ViewImports.cshtml
│       ├── _ViewStart.cshtml
│       └── Users/Index.cshtml
├── Authorization/          → AccountAgeRequirement, AccountAgeHandler
├── Controllers/            → HomeController, AccountController, PremiumController
├── Data/                   → ApplicationDbContext, DbSeeder
├── DTO/                    → RegisterDTO, LoginDTO, ProfileDTO, UserRowDTO
├── Models/                 → ApplicationUser, ApplicationRole
├── Views/                  → Home, Account, Premium, Shared
├── wwwroot/css/site.css
├── appsettings.json
└── Program.cs
```

---

## 🛣️ Rutas y permisos

| Ruta | Quién puede entrar |
|------|--------------------|
| `/` | Cualquiera. |
| `/Account/Register`, `/Account/Login` | Cualquiera. |
| `/Account/Profile` | Usuarios autenticados. |
| `/Premium` | Usuarios autenticados con la antigüedad mínima (política `Premium`). |
| `/Admin/Users` | Usuarios con el rol `Admin`. |

Quien no está autenticado es enviado a `/Account/Login`. Quien está autenticado pero no tiene permiso va a `/Account/AccessDenied`.

---

## 🔑 Configuración previa

**1. Cadena de conexión.** En `appsettings.json`, usando una base de datos propia (`SecureHubDb`), separada de la de la Stocks App:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=;Integrated Security=True;Connect Timeout=30;Encrypt=False"
},
"PremiumMinimumDays": 7
```


**2. Administrador inicial.** Se guarda en User Secrets para no dejar credenciales en el código ni en el repositorio:

```bash
dotnet user-secrets init
dotnet user-secrets set "SeedAdmin:Email" "admin@securehub.com"
dotnet user-secrets set "SeedAdmin:Password" "UnaClaveDePractica1"
```

Es una cuenta de práctica: no hay que reutilizar esa contraseña en ningún sistema real.

**3. Certificado de desarrollo para HTTPS:**

```bash
dotnet dev-certs https --trust
```

**4. Base de datos.** Primero se migra y después se ejecuta la app, porque el *seeder* necesita que las tablas ya existan:

```bash
dotnet ef migrations add Initial
dotnet ef database update
```

---

## ▶️ Cómo ejecutar el proyecto

```bash
dotnet run --launch-profile https
```

Al arrancar, `DbSeeder` crea los roles `Admin` y `User` y la cuenta de administrador (si no existen).

---


## 🔎 Detalles a tener en cuenta

- **`[Authorize]` a nivel de clase en `AccountController`:** todo queda protegido por defecto y solo se abre lo público con `[AllowAnonymous]`. Si se olvida una acción, queda cerrada en lugar de abierta.
- **`asp-area=""` en los enlaces del layout:** sin esto, dentro del área `Admin` los enlaces heredan el área y apuntan a rutas que no existen.
- **`Url.IsLocalUrl(returnUrl)`:** evita que alguien arme un enlace de login que, tras autenticarse, redirija a otro sitio (*open redirect*).
- **Mensaje de login genérico:** "Invalid email or password" no distingue si falló el email o la contraseña, para no revelar qué cuentas existen. La validación remota del registro sí revela si un email está tomado: es un equilibrio entre usabilidad y seguridad.
- **Cambios de rol:** los roles viajan dentro de la cookie, por lo que el usuario afectado los ve al volver a iniciar sesión o cuando Identity revalida la cookie (cada 30 minutos por defecto). La política Premium, en cambio, se evalúa en cada petición.
- **El administrador no puede cambiar su propio rol**, para no quedarse sin administradores por error.
- **Remote Validation** necesita jQuery, jQuery Validate y jQuery Validation Unobtrusive en la vista de registro.

---

## 🛠️ Tecnologías y conceptos aplicados

- ASP.NET Core MVC e ASP.NET Core Identity con claves `Guid`.
- Entity Framework Core con SQL Server y migraciones.
- `UserManager`, `SignInManager` y `RoleManager`.
- Autorización por roles, políticas personalizadas (`IAuthorizationRequirement` y `AuthorizationHandler`).
- Areas y enrutamiento convencional.
- Validación remota y DataAnnotations.
- Protección XSRF con tokens antifalsificación.
- HTTPS, HSTS y cookies seguras.
- User Secrets para datos sensibles.
