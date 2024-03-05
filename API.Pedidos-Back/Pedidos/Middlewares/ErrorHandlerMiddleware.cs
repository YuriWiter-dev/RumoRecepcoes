using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DTI.Pedidos.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ErrorHandlerMiddleware(
          RequestDelegate next
        , IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    private static async Task MenssagemErro500(HttpContext context, Exception error)
    {
        string idErro = Guid.NewGuid().ToString();
        Log.Error(error, idErro);
        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await response.WriteAsync(JsonSerializer.Serialize(new
        {
            CodigoError = idErro,
            Error = $"Erro. Por favor procure o suporte DTI (suporte@dti.com) e forneça esse código: {idErro}"
        }));
    }

    private static async Task MenssagemErro400(HttpContext context, Exception error)
    {
        string idErro = Guid.NewGuid().ToString();
        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await response.WriteAsync(JsonSerializer.Serialize(new
        {
            CodigoError = idErro,
            Error = error.Message
        }));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException error)
        {
            await MenssagemErro400(context, error);
        }
        catch (Exception error)
        {
            await MenssagemErro500(context, error);
        }
    }
}
