// Program.cs
using Microsoft.EntityFrameworkCore;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Infrastructure.Data;
using MuniLK.Infrastructure.Logging;
using MuniLK.Infrastructure.Services;
using MuniLK.Worker.Services;


Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<MuniLKDbContext>(options =>
    options.UseSqlServer(
        hostContext.Configuration.GetConnectionString("DefaultConnection")));

        // Register ICurrentTenantService and its implementation in the Worker project
        // IMPORTANT: Choose the correct lifetime (Scoped, Transient, or Singleton)
        // Scoped is common if CurrentTenantService depends on request-specific context (like HttpContext),
        // but in a worker, there might not be an HttpContext.
        // If CurrentTenantService just provides a static tenant ID or is set up differently for workers,
        // you might need a different approach or a Transient/Singleton lifetime.
        // For a worker, often CurrentTenantService might be simpler or configured differently.
        // Let's assume for now it's simple enough to be Scoped, but keep an eye on its dependencies.
        services.AddScoped<ICurrentTenantService, WorkerCurrentTenantService>(); 

        services.AddHostedService<RabbitMqConsumerWorker>();
       services.Configure<EmailSettings>(hostContext.Configuration.GetSection("Email"));
        // Register your application services here
        services.AddScoped<IMyMessageProcessor,  LoggingRepository>();
        // Add other dependencies if needed
    })
    .Build()
    .Run();
