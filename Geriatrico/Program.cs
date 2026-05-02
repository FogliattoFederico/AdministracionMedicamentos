using Geriatrico.Models.Negocio;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _ => "El campo es obligatorio");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (x, y) => $"El valor '{x}' no es válido para {y}");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        x => $"El valor '{x}' no es válido");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        x => $"El campo {x} debe ser un número");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
        x => $"El campo {x} es obligatorio");
});

// Sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddScoped<PacienteNegocio>(provider => new PacienteNegocio(connectionString));
builder.Services.AddScoped<MedicamentoNegocio>(provider => new MedicamentoNegocio(connectionString));
builder.Services.AddScoped<MedicamentoPacienteNegocio>(provider => new MedicamentoPacienteNegocio(connectionString));
builder.Services.AddScoped<AdministracionNegocio>(provider => new AdministracionNegocio(connectionString));
builder.Services.AddScoped<DashboardNegocio>(provider => new DashboardNegocio(connectionString));
builder.Services.AddScoped<UsuarioNegocio>(provider => new UsuarioNegocio(connectionString));

var supportedCultures = new[] { new CultureInfo("es-AR") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-AR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseRequestLocalization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();