using System;
using System.Net;
using System.Net.Http;

namespace SandboxConsoleApp.Http
{
    public partial class HttpConnector : IDisposable
    {
        public readonly HttpClient Client;

        public HttpConnector()
           : this(TimeSpan.FromMinutes(5))
        {
        }

        public HttpConnector(TimeSpan timeout)
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = true,
                PreAuthenticate = true
            };
            Client = new HttpClient(handler) { Timeout = timeout };
        }

        public HttpConnector(HttpMessageHandler handler)
        {
            Client = new HttpClient(handler);
        }

        public ContentResponse Get(string url, params object[] args)
        {
            var escapedUrl = NormaliseUrl(url, args);
            var task = Client.GetAsync(escapedUrl);
            return task.ContinueWith(httpTask =>
            {
                if (httpTask.IsFaulted)
                {
                    var response = new ContentResponse { Faulted = true };
                    if (httpTask.Exception != null)
                    {
                        response.Message = httpTask.Exception.Message;
                    }

                    return response;
                }

                var result = httpTask.Result;

                if (!result.IsSuccessStatusCode)
                {
                    return CreateResponseIfNotSuccessful<ContentResponse>(httpTask);
                }

                return new ContentResponse
                {
                    Faulted = false,
                    StatusCode = result.StatusCode,
                    Message = httpTask.Result.ReasonPhrase,
                    Content = result.Content.ReadAsStringAsync().Result
                };
            }).Result;
        }

        public RedirectResponse PostFormEncodedExpectRedirect<T>(T input, string url, params object[] args)
        {
            var operationUri = NormaliseUrl(url, args);

            var formvars = new FormUrlEncodedContent(input.PublicPropertiesToKeyValuePairs());

            var task = Client.PostAsync(operationUri, formvars);
            return task.ContinueWith(httpTask =>
            {
                if (httpTask.IsFaulted)
                {
                    return new RedirectResponse
                    {
                        Faulted = true,
                        Message = httpTask.Exception.ToString()
                    };
                }

                if (httpTask.Result.StatusCode != HttpStatusCode.Redirect)
                {
                    var content = httpTask.Result.Content.ReadAsStringAsync().Result;

                    return new RedirectResponse
                    {
                        Faulted = false,
                        StatusCode = httpTask.Result.StatusCode,
                        Content = content
                    };
                }

                return new RedirectResponse
                {
                    Faulted = false,
                    RedirectToLocation = httpTask.Result.Headers.Location.OriginalString,
                    StatusCode = httpTask.Result.StatusCode
                };
            }).Result;
        }

        public string NormaliseUrl(string url, params object[] args)
        {
            var uri = string.Format(url, args);

#if DEBUG
            uri = uri.Replace("://localhost", "://" + Environment.MachineName);
#endif

            return uri;
        }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
            }
        }
    }
}