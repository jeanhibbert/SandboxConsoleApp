using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SandboxConsoleApp.Http
{
    public partial class HttpConnector
    {
        public DataResponse<T> Get<T>(string url, params object[] args)
        {
            var escapedUrl = NormaliseUrl(url, args);
            var task = Client.GetAsync(escapedUrl);
            return task.ContinueWith(x => GetDataResponse<T>(x)).Result;
        }

        public async Task<TOut> PostAsJsonAsync<TIn, TOut>(TIn input, string url)
        {
            var result = await Client.PostAsJsonAsync(url, input);
            var response  = await result.Content.ReadAsAsync<TOut>();
            return response;
        }

        public DataResponse<TOut> PostAsJson<TIn, TOut>(TIn input, string url, params object[] args)
        {
            var escapedUrl = NormaliseUrl(url, args);
            var task = Client.PostAsJsonAsync(escapedUrl, input);
            return task.ContinueWith(x => GetDataResponse<TOut>(x)).Result;
        }

        public ContentResponse PostAsJson<T>(T input, string url, params object[] args)
        {
            var escapedUrl = NormaliseUrl(url, args);
            var task = Client.PostAsJsonAsync(escapedUrl, input);

            return task.ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    var response = new ContentResponse { Faulted = true };
                    if (x.Exception != null)
                    {
                        response.Message = x.Exception.Message;
                    }

                    return response;
                }

                var result = task.Result;

                if (!result.IsSuccessStatusCode)
                {
                    return CreateResponseIfNotSuccessful<ContentResponse>(x);
                }

                if (ResponseIsHtmlContentType(result))
                {
                    return new ContentResponse
                    {
                        Faulted = true,
                        Message = "Server responded with text/html (indicates a non-webapi/mvc data action has an unhandled exception)"
                    };
                }

                return new ContentResponse
                {
                    Faulted = false,
                    StatusCode = result.StatusCode,
                    Message = x.Result.ReasonPhrase
                };
            })
            .Result;
        }

        private static bool ResponseIsHtmlContentType(HttpResponseMessage result)
        {
            if (result.Content == null)
            {
                return false;
            }

            if (result.Content.Headers == null || !result.Content.Headers.Any())
            {
                return false;
            }

            if (result.Content.Headers.ContentType == null)
            {
                return false;
            }

            var mediaType = result.Content.Headers.ContentType.MediaType ?? string.Empty;
            return mediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase);
        }

        private static DataResponse<TOut> GetDataResponse<TOut>(Task<HttpResponseMessage> x)
        {
            if (x.IsFaulted)
            {
                var response = new DataResponse<TOut> { Faulted = true };
                if (x.Exception != null)
                {
                    response.Message = x.Exception.Message;
                }

                return response;
            }

            var result = x.Result;

            if (!result.IsSuccessStatusCode)
            {
                return CreateResponseIfNotSuccessful<DataResponse<TOut>>(x);
            }

            if (ResponseIsHtmlContentType(result))
            {
                return new DataResponse<TOut>
                {
                    Faulted = true,
                    Message = "Server responded with text/html - expected a data representation of type '{0}'".FormatWith(typeof(TOut).Name)
                };
            }

            return new DataResponse<TOut>
            {
                Faulted = false,
                StatusCode = result.StatusCode,
                Message = x.Result.ReasonPhrase,
                Data = result.Content.ReadAsAsync<TOut>().Result
            };
        }

        private static T CreateResponseIfNotSuccessful<T>(Task<HttpResponseMessage> x)
            where T : ContentResponse, new()
        {
            string message = null;
            HttpErrorContent httpError;

            try
            {
                var error = x.Result.Content.ReadAsAsync<HttpError>().Result;
                httpError = error.ToObject<HttpErrorContent>();
            }
            catch (Exception)
            {
                httpError = null;

                try
                {
                    message = x.Result.Content.ReadAsStringAsync().Result;
                }
                catch
                {
                    message = null;
                }
            }

            var msg = x.Result.ReasonPhrase;
            if (!string.IsNullOrWhiteSpace(message))
            {
                msg += " ({0})".FormatWith(message);
            }

            return new T
            {
                Faulted = true,
                StatusCode = x.Result.StatusCode,
                HttpError = httpError,
                Message = msg,
            };
        }
    }
}
