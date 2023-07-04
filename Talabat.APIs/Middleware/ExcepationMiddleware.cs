using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middleware
{
    public class ExcepationMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExcepationMiddleware> Logger;
        private readonly IHostEnvironment Env;
        public ExcepationMiddleware (RequestDelegate next,ILogger<ExcepationMiddleware> logger, IHostEnvironment env) 
        {
            Next= next;
            Logger= logger;
            Env= env;
        }

        public async Task InvokeAsync (HttpContext context) 
        {
            try
            {
                await Next.Invoke(context); // Move To Next Muddleware
            }
            catch (Exception ex) // handle exception and control response error  return to user
            {

                Logger.LogError(ex , ex.Message);
                // log exception at Database

                //response error  return to user
                // header of response
                context.Response.ContentType= "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // HttpStatusCode.InternalServerError => return string
                // Body of response
                var excepationErrorRespons = Env.IsDevelopment() ?
                    new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
                    :
                     new ApiExceptionResponse(500);//OR(500,ex.Message, ex.StackTrace.ToString()) // messege=null , details=null

                var options = new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase };//convert Json from capital to small
                var json = JsonSerializer.Serialize(excepationErrorRespons, options);

              await  context.Response.WriteAsync(json); 

            }
        }
    }
}
