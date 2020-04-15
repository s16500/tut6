using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stut6.MiddleWares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            //Our code
            if (!(httpContext == null))
            {
                string method = httpContext.Request.Method;
                var route = httpContext.Request.Path;
                string queryStrings = httpContext.Request.QueryString.ToString();
                var body = "";
                using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                }

                using (StreamWriter writer = new StreamWriter("/MiddleWares/requestsLog.txt"))
                {
                    writer.Write("Method type: "+method);
                  writer.Write("Endpoint path of request: "+route);
                    writer.Write("The body of request: "+body);
                    writer.Write("QueryStrings of request: "+queryStrings);
                     }


            }

            await _next(httpContext);
        }
        


    }
}
