using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RestApi.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                return null;

            return httpContext.User.Claims.Single(s => s.Type == "id").Value;
        }
    }
}
