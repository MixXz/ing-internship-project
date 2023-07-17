using System.Net;

namespace VacaYAY.Web.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exc)
        {
            //TODO - Add logger
            Console.WriteLine(exc.ToString());

            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync(@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Oops, something went wrong!</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin: 40px;
                            padding: 0;
                            text-align: center;
                        }

                        h1 {
                            font-size: 28px;
                            margin-bottom: 20px;
                        }

                        p {
                            font-size: 18px;
                            margin-bottom: 20px;
                        }
                    </style>
                </head>
                <body>
                    <h1>Oops, something went wrong!</h1>
                    <p>We're sorry, but an unexpected error occurred.</p>
                </body>
                </html>
            ");
        }
    }
}
