using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
        {
            // Handle the specific RpcException here
            context.Response.StatusCode = 500; // Internal Server Error
            context.Response.ContentType = "application/json";

            var response = new { StatusCode = 500, Message = ex.Message };
            var jsonResponse = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(jsonResponse);
        }
        catch (Exception)
        {
            // Handle other exceptions here
            throw; // Re-throw the exception for further handling by other middleware
        }
    }
}
