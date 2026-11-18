using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sección_25___Identity__Authorization_and_Security.Authorization;
using Sección_25___Identity__Authorization_and_Security.Data;
using Sección_25___Identity__Authorization_and_Security.Models;

var builder = WebApplication.CreateBuilder(args);

// XSRF: todos los POST/PUT/DELETE exigen token
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Reglas de contraseña
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;   // la cookie solo viaja por HTTPS
});

int premiumDays = builder.Configuration.GetValue<int>("PremiumMinimumDays");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Premium", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new AccountAgeRequirement(premiumDays));
    });
});


builder.Services.AddScoped<IAuthorizationHandler, AccountAgeHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}




app.Run();
