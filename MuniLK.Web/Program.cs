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

var builder = WebApplication.CreateBuilder(args);
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzk2NDQ4OUAzMzMwMmUzMDJlMzAzYjMzMzQzYm1vQ2VaaUFGSzFoQ1Jqbk9WL05xZEYzQVRtMCtYZ1VLQ3QvS0ZEenhpWGs9");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SubmitBuildingPlan", policy =>
        policy.RequireRole(
            Roles.SuperAdmin,
            Roles.Admin,
            Roles.Officer));
    // add other shared policies if needed
}); 
builder.Services.AddCascadingAuthenticationState(); // Makes AuthenticationState available to components

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Needed if CustomAuthStateProvider accesses HttpContext

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

// Add separate HTTP clients 
builder.Services.AddHttpClient(); // For the factory
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
}).AddHttpMessageHandler<AuthTokenHandler>();

// Add a client without token handler for internal use (to avoid recursion during token refresh)
builder.Services.AddHttpClient("ApiHttpClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
});


// For Blazor Server, use the main authorized HTTP client
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<DocumentClient>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
}); 
//builder.Services.AddRazorComponents()
//    .AddInteractiveServerComponents()
//     .AddCircuitOptions(options =>
//    {
//        options.DetailedErrors = true;
//    });

builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode();
app.MapBlazorHub();

app.Run();
