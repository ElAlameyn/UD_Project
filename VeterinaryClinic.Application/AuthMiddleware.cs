namespace VeterinaryClinic.Application;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey("username") 
            && !context.Request.Cookies.ContainsKey("userpassword"))
        {
            context.Request.Cookies.Append(new KeyValuePair<string, string>("username", "simpleuser"));
            context.Request.Cookies.Append(new KeyValuePair<string, string>("userpassword", "12345678"));
        }

        await _next.Invoke(context);
    }
}