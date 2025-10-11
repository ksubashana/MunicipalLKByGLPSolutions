using Application.Services;
using AutoMapper;
using Azure.Storage.Blobs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MuniLK.Application.Assignments;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Documents.Interfaces;
using MuniLK.Application.FeatureId.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Mappers;
using MuniLK.Application.PropertiesLK;
using MuniLK.Application.PropertyOwners.Interfaces;
using MuniLK.Application.Reports.Interfaces;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Application.Services;
using MuniLK.Application.Tenants;
using MuniLK.Application.Tenants.Services;
using MuniLK.Applications.Interfaces;
using MuniLK.Domain.Interfaces;
using MuniLK.Infrastructure.Assignments;
using MuniLK.Infrastructure.BuildingAndPlanning;
using MuniLK.Infrastructure.Contact;
using MuniLK.Infrastructure.Data;
using MuniLK.Infrastructure.Documents; // Add this using directive
using MuniLK.Infrastructure.FeatureIDService;
using MuniLK.Infrastructure.Generic.Repositories;
using MuniLK.Infrastructure.Generic.Services;
using MuniLK.Infrastructure.Logging;
using MuniLK.Infrastructure.PropertiesLK;
using MuniLK.Infrastructure.PropertyOwners;
using MuniLK.Infrastructure.Reports;
using MuniLK.Infrastructure.ScheduleAppointment;
using MuniLK.Infrastructure.Security;
using MuniLK.Infrastructure.Services;
using MuniLK.Web.Services;
using MuniLK.Worker.Services;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Sinks.RabbitMQ;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.OpenApi; // add this



var builder = WebApplication.CreateBuilder(args);
SelfLog.Enable(msg => Console.Error.WriteLine("[Serilog SelfLog] " + msg));
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
// Register IHttpContextAccessor and CurrentTenantService for multi-tenancy
builder.Services.AddHttpContextAccessor(); // Required for CurrentTenantService to access HTTP context
builder.Services.AddSingleton<ILogEventEnricher, TenantIdEnricher>();
// --- Configure Azure Blob Storage Client ---
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorageConnection"));

});
Bold.Licensing.BoldLicenseProvider.RegisterLicense("HE9AZ25cXVO/KlqZZSzZCRp2hIyDA6akPgvLlkA+OQM=");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .WithOrigins("http://localhost:5116") // Blazor app URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});
// --- Register your custom BlobStorageService ---
// BlobServiceClient is typically a singleton as it manages connections efficiently.
// BlobStorageService is stateless and depends on a singleton, so it can also be a singleton.
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    // Serilog will now use the 'services' (IServiceProvider) provided by the host
    // to resolve ILogEventEnricher instances.
    // We explicitly get the TenantIdEnricher from the services and pass the instance.
    .Enrich.With(services.GetRequiredService<ILogEventEnricher>())
);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Add MediatR
builder.Services.AddMediatR(
    cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.RegisterServicesFromAssembly(typeof(GetAllLicensesQuery).Assembly);
});

// Add DbContext (update connection string as needed)
builder.Services.AddDbContext<MuniLKDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Use a separate connection string if desired

// Add ASP.NET Core Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Password settings (adjust for production requirements)
    options.SignIn.RequireConfirmedAccount = false; // For simplicity, set to true in production
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>() // Use your DbContext for Identity tables
    .AddDefaultTokenProviders(); // For password reset tokens, email confirmation tokens etc.

// Configure Data Protection for secure refresh token cookies
builder.Services.AddDataProtection();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Make JWT the default
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured."),
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured."),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.")))
    };
});

// Add Authorization policies (optional, but good for fine-grained control)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageLicenses", policy =>
        policy.RequireClaim("permission", "licenses.manage")); // Example custom claim
    // You can add more policies here based on roles or custom claims
});



// Register your custom services and implementations
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>(); 
builder.Services.AddScoped<ITokenService, JwtTokenService>(); 
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IRoleService, RoleService>();
//builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailService, MuniLK.Infrastructure.Services.EmailService>(); 
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ModuleService>(); // If no interface
builder.Services.AddAutoMapper(config => { config.AddProfile<MappingProfile>(); });


// Add scoped services
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IMyMessageProcessor, LoggingRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IBuildingPlanRepository, BuildingAndPlanningRepository>();
builder.Services.AddScoped<IPlanningCommitteeReviewRepository, PlanningCommitteeReviewRepository>();
builder.Services.AddScoped<IFeatureIdService, FeatureIDService>();
builder.Services.AddScoped<IPropertyOwnerRepository, PropertyOwnerRepository>();
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<MuniLKDbContext>());
builder.Services.AddScoped<IDocumentLinkRepository, DocumentLinkRepository>();
builder.Services.AddScoped<IClientConfigurationService, ClientConfigurationService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IScheduleAppointmentRepository, ScheduleAppointmentRepository>();
builder.Services.AddScoped<ISiteInspectionRepository, SiteInspectionRepository>();
builder.Services.AddScoped<IPlanningCommitteeReviewRepository, PlanningCommitteeReviewRepository>();
builder.Services.AddScoped<IEntityOptionSelectionRepository, EntityOptionSelectionRepository>();
builder.Services.AddScoped<IEntityOptionSelectionService, MuniLK.Application.BuildingAndPlanning.Services.EntityOptionSelectionService>();


var app = builder.Build();
app.UseCors("AllowBlazor");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

app.Run();
