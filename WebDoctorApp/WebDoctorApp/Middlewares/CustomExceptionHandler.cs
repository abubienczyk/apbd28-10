using System.Net;

namespace WebDoctorApp.Middlewares;

public class CustomExceptionHandler
{
    private readonly RequestDelegate _next;
    
    public CustomExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }
    
    // Implement exception handling here
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
          
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new { message = "Wystąpił błąd podczas przetwarzania Twojego żądania." };

        // Tutaj możesz dodać logowanie szczegółowe błędu, np. z użyciem ILogger
        Console.WriteLine($"Exception: {exception.Message}");

        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}