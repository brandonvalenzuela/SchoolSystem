using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SchoolSystem.Web;
using SchoolSystem.Web.Auth;
using SchoolSystem.Web.Services;
using SchoolSystem.Web.Services.Toast;

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
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<GradoService>();
builder.Services.AddScoped<MateriaService>();
builder.Services.AddScoped<GrupoService>();
builder.Services.AddScoped<InscripcionService>();
builder.Services.AddScoped<CalificacionService>();
builder.Services.AddScoped<PagoService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<AsistenciaService>();
builder.Services.AddScoped<ConductaService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<NotificacionService>();
builder.Services.AddScoped<RelacionService>();
builder.Services.AddScoped<PadreService>();
builder.Services.AddScoped<AcademicContextService>();
builder.Services.AddScoped<PeriodoEvaluacionService>();


// 4. Servicios de la Aplicación
builder.Services.AddScoped<ApiService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
