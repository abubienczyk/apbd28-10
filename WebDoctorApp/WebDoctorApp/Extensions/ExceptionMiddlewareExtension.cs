using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace WebDoctorApp.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                // context.Response.ContentType = "application/json";
                //     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //
                //     var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                //     if (contextFeature != null)
                //     {
                //         var logger = context.RequestServices.GetService<ILogger<ExceptionMiddlewareExtension>>();
                //         logger?.LogError($"Something went wrong: {contextFeature.Error}");
                //
                //         var errorDetails = new ErrorDetails
                //         {
                //             StatusCode = context.Response.StatusCode,
                //             Message = "Internal Server Error. Please try again later."
                //         };
                //
                //         // Tutaj możesz użyć bibliotek takich jak Serilog, aby logować więcej szczegółów o błędzie
                //         // Możesz także logować exception details do pliku lub bazy danych
                //
                //         var response = JsonSerializer.Serialize(errorDetails);
                //         await context.Response.WriteAsync(response);
                //     }
            });
        });
    }
    // public class ErrorDetails
    // {
    //     public int StatusCode { get; set; }
    //     public string Message { get; set; } = string.Empty;
    //
    //     public override string ToString()
    //     {
    //         return JsonSerializer.Serialize(this);
    //     }
    // }
}