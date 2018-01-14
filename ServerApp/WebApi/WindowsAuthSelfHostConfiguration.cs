using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;

namespace ServerApp.WebApi
{
    public class WindowsAuthSelfHostConfiguration : HttpSelfHostConfiguration
    {
        public WindowsAuthSelfHostConfiguration(string baseAddress)
            : base(baseAddress)
        {
        }

        public WindowsAuthSelfHostConfiguration(Uri baseAddress)
            : base(baseAddress)
        {
        }

        protected override BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            httpBinding.Security.Mode = HttpBindingSecurityMode.TransportCredentialOnly;
            httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            return base.OnConfigureBinding(httpBinding);
        }
    }
}
