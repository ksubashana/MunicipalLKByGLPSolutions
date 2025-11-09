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
using MuniLK.Infrastructure.Documents; 
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
using Microsoft.AspNetCore.OpenApi;
using MuniLK.Application.PlanningCommitteeMeetings.Interfaces;
using MuniLK.API.Middleware; // added

var builder = WebApplication.CreateBuilder(args);
SelfLog.Enable(msg => Console.Error.WriteLine("[Serilog SelfLog] " + msg));
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ILogEventEnricher, TenantIdEnricher>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorageConnection"));
});
Bold.Licensing.BoldLicenseProvider.RegisterLicense("HE9AZ25cXVO/KlqZZSzZCRp2hIyDA6akPgvLlkA+OQM=");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .WithOrigins("http://localhost:5116")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Enrich.With(services.GetRequiredService<ILogEventEnricher>())
);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddMediatR(
    cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.RegisterServicesFromAssembly(typeof(GetAllLicensesQuery).Assembly);
});

builder.Services.AddDbContext<MuniLKDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDataProtection();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageLicenses", policy =>
        policy.RequireClaim("permission", "licenses.manage"));

    options.AddPolicy("SubmitBuildingPlan", policy =>
        policy.RequireRole(
            MuniLK.Domain.Constants.Roles.SuperAdmin,
            MuniLK.Domain.Constants.Roles.Admin,
            MuniLK.Domain.Constants.Roles.Officer));
});

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>(); 
builder.Services.AddScoped<ITokenService, JwtTokenService>(); 
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, MuniLK.Infrastructure.Services.EmailService>(); 
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ModuleService>(); 
builder.Services.AddAutoMapper(config => { config.AddProfile<MappingProfile>(); });

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
builder.Services.AddScoped<IPlanningCommitteeMeetingRepository, PlanningCommitteeMeetingRepository>();
builder.Services.AddScoped<IRefreshTokenStore, RefreshTokenStore>(); // added

var app = builder.Build();
app.UseCors("AllowBlazor");



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// Custom exception logging middleware first to capture all downstream errors
app.UseExceptionLogging(); // added

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();

AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

app.Run();
