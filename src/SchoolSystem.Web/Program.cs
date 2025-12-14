using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoolSystem.Web;
using SchoolSystem.Web.Auth;
using SchoolSystem.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Configurar la URL base de tu API (Asegúrate que coincida con tu launchSettings.json de la API)
// Si tu API corre en https://localhost:7207
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7207") });

// 2. LocalStorage
builder.Services.AddBlazoredLocalStorage();

// 3. Autenticación
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<ClientAuthService>();
builder.Services.AddScoped<AlumnoService>();
builder.Services.AddScoped<UiService>();
builder.Services.AddScoped<MaestroService>();
builder.Services.AddScoped<UsuarioService>();

// 4. Servicios de la Aplicación
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();
