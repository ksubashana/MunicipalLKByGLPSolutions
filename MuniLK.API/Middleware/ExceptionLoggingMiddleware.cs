using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using MuniLK.Application.Generic.Interfaces;

namespace MuniLK.API.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionLoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                // Resolve tenant/user if available
                var tenantService = context.RequestServices.GetService(typeof(ICurrentTenantService)) as ICurrentTenantService;
                var userService = context.RequestServices.GetService(typeof(ICurrentUserService)) as ICurrentUserService;
                var tenantId = tenantService?.GetTenantId();
                var userId = userService?.UserId;

                using (LogContext.PushProperty("TenantId", tenantId, true))
                using (LogContext.PushProperty("UserId", userId, true))
                using (LogContext.PushProperty("RequestPath", context.Request.Path.Value))
                {
                    Log.Error(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var problem = new
                    {
                        title = "An unexpected error occurred.",
                        status = context.Response.StatusCode,
                        requestId = context.TraceIdentifier // renamed from traceId
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                }
            }
        }
    }

    public static class ExceptionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionLogging(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionLoggingMiddleware>();
    }
}
