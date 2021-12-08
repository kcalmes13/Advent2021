using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using System.Security.Cryptography.X509Certificates;
using Autofac.Extensions.DependencyInjection;
using System.Net;

namespace UnionWireless.Utilities.ModelParser.Api
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Api : StatelessService
    {
        /// <summary>
        /// Constructor for staless service
        /// </summary>
        /// <param name="context">context for stateless service</param>
        public Api(StatelessServiceContext context) : base(context)
        {
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            if (Environment.GetEnvironmentVariable("WebEnvironment")!.ToLower().Equals("https"))
            {
                yield return new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpointHttps", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel in https mode on {url}");

                        return new WebHostBuilder()
                            .UseKestrel(options =>
                            {
                                var port = serviceContext.CodePackageActivationContext.GetEndpoint("ServiceEndpointHttps").Port;
                                options.Listen(IPAddress.IPv6Any, port, listenOptions => { listenOptions.UseHttps(GetCertificateFromStore()); });
                            })
                            .ConfigureServices(services => services.AddSingleton(serviceContext).AddAutofac())
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseStartup<Startup>()
                            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                            .UseUrls(url)
                            .Build();
                    }));
            }
            else
            {
                yield return new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpointHttp", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel in http mode on {url}");

                        return new WebHostBuilder()
                            .UseKestrel()
                            .ConfigureServices(services => services.AddSingleton(serviceContext).AddAutofac())
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseStartup<Startup>()
                            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                            .UseUrls(url)
                            .Build();
                    }));
            }
        }

        /// <summary>
        /// Finds the ASP .NET Core HTTPS development certificate in development environment. Update this method to use the appropriate certificate for production environment.
        /// </summary>
        /// <returns>Returns the ASP .NET Core HTTPS development certificate</returns>
        private static X509Certificate2 GetCertificateFromStore()
        {
            var certStoreLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), Environment.GetEnvironmentVariable("CertLocation")!);
            var certStoreName = (StoreName)Enum.Parse(typeof(StoreName), Environment.GetEnvironmentVariable("CertStore")!);
            var certCommonName = Environment.GetEnvironmentVariable("HttpsCertCommonName")!;

            using var store = new X509Store(certStoreName, certStoreLocation);
            store.Open(OpenFlags.ReadOnly);

            var certCollection = store.Certificates;
            var currentCerts = certCollection.Find(X509FindType.FindBySubjectName, certCommonName, true);

            return currentCerts.Count == 0 ? null : currentCerts[0];
        }
    }
}
