using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Domain.Constants;
using MuniLK.Infrastructure.Services;
using MuniLK.Web.Clients;
using MuniLK.Web.Components;
using MuniLK.Web.Interfaces;
using MuniLK.Web.Services;
using Syncfusion.Blazor;
using System.Globalization;
using Microsoft.AspNetCore.Components.Server.Circuits;

var builder = WebApplication.CreateBuilder(args);
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzk2NDQ4OUAzMzMwMmUzMDJlMzAzYjMzMzQzYm1vQ2VaaUFGSzFoQ1Jqbk9WL05xZEYzQVRtMCtYZ1VLQ3QvS0ZEenhpWGs9");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SubmitBuildingPlan", policy =>
        policy.RequireRole(
            Roles.SuperAdmin,
            Roles.Admin,
            Roles.Officer));
});
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("si") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddSingleton<CircuitHandler, GlobalCircuitHandler>();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient("ApiHttpClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<DocumentClient>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
#if DEBUG
        options.DetailedErrors = true;
#else
        options.DetailedErrors = false;
#endif
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10); // allow seamless reconnect for idle users
    })
    .AddHubOptions(options =>
    {
        options.EnableDetailedErrors = false; // true only if needed
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60); // detect dead clients
        options.KeepAliveInterval = TimeSpan.FromSeconds(15); // push keepalive pings
    });

builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseRouting();
var supportedCultures = new[] { "en", "si", "ta" };

var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .SetDefaultCulture("en");

localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapFallbackToPage("/_Host");
app.MapBlazorHub();

app.Run();
