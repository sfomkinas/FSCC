using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using FSCC.Core.Data;

namespace FSCC.Handlers
{
    namespace Http
    {
        public class LogAttribute : System.Web.Http.Filters.ActionFilterAttribute
        {
            static string GetCookies(HttpRequestMessage request)
            {
                return string.Join("; ", request.Headers.GetCookies());
            }

            public override void OnActionExecuting(HttpActionContext actionContext)
            {
                HttpRequestMessage request = actionContext.Request;
                using (var context = new FSCCContext())
                {
                    var j = context.JournalLogs.Add(new JournalLog()
                    {
                        Date = DateTime.UtcNow,
                        Cookies = GetCookies(request),
                        Path = request.RequestUri.LocalPath,
                        QueryString = request.RequestUri.Query,
                        RequestType = request.Method.ToString(),
                        UrlReferrer = string.Join("\r\n", request.Headers.Where(h => h.Key == "Referer").Select(h => h.Value != null ? string.Join("; ", h.Value.Where(v => !string.IsNullOrEmpty(v))) : string.Empty)),
                        UserAgent = string.Join("\r\n", request.Headers.Where(h => h.Key == "User-Agent").Select(h => h.Value != null ? string.Join("; ", h.Value.Where(v => !string.IsNullOrEmpty(v))) : string.Empty)),
                        Headers = string.Join("\r\n", request.Headers.Select(h => h.Key + "=" + h.Value != null ? string.Join("; ", h.Value.Where(v => !string.IsNullOrEmpty(v))) : string.Empty)),
                    });

                    context.SaveChangesAsync();
                }

                base.OnActionExecuting(actionContext);
            }
        }
    }

    namespace Mvc
    {
        public class LogAttribute : System.Web.Mvc.ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext actionContext)
            {
                HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;

                using (var context = new FSCCContext())
                {
                    var j = context.JournalLogs.Add(new JournalLog()
                    {
                        Date = DateTime.UtcNow,
                        Cookies = request.Cookies != null ? string.Join("\r\n", request.Cookies.AllKeys.Select(key => key + "=" + request.Cookies[key].Value)) : string.Empty,
                        Path = request.Path,
                        QueryString = request.QueryString != null ? string.Join("&", request.QueryString.AllKeys.Select((key, i) => key + "=" + string.Join("; ", request.QueryString[key]))) : string.Empty,
                        RequestType = request.RequestType,
                        UrlReferrer = request.UrlReferrer?.OriginalString,
                        UserAgent = request.UserAgent,
                        Headers = request.Headers != null ? string.Join("\r\n", request.Headers.AllKeys.Select(key => key + "=" + string.Join("; ", request.Headers[key]))) : string.Empty,
                    });

                    context.SaveChangesAsync();
                }

                base.OnActionExecuting(actionContext);
            }
        }
    }
}