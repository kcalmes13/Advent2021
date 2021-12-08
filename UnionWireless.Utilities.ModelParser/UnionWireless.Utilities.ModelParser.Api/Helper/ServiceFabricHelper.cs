using Microsoft.ServiceFabric.Services.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnionWireless.Utilities.ModelParser.Api.Helper
{
    /// <summary>
    /// helper for getting endpoint of service fabric api.
    /// </summary>
    public class ServiceFabricHelper
    {
        /// <summary>
        /// Returns a url for a stateless service fabric
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns>url</returns>
        public static string GetUrlForStatelessService(string applicationName)
        {
            var endpoints = ListEndpoints(applicationName);
            int index = new Random().Next(endpoints.Count);
            var address = endpoints[index].Replace("\\", "");
            return address;
        }

        /// <summary>
        /// Helper Method to get uri for stateful service
        /// </summary>
        /// <param name="AppUri">uri of applicaiton to get endpoint</param>
        /// <returns>endpoint for app</returns>
        public static string GetUrlForStatefulService(string AppUri)
        {
            ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();

            // pull partition by service uri
            ResolvedServicePartition partition = resolver
                .ResolveAsync(new Uri(AppUri), new ServicePartitionKey(), CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            // get endpoint for stateful service and build out endpoints
            ResolvedServiceEndpoint endpoint = partition.GetEndpoint();
            var address = endpoint.Address;
            address = address.Replace("\\", "");
            address = address.Replace("\"\"", "\"address\"");
            var deserialize
                = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(address);
            var url = deserialize["Endpoints"]["address"];

            return url;
        }

        /// <summary>
        /// Get Service Fabric Endpoints
        /// </summary>
        /// <returns></returns>
        private static List<string> ListEndpoints(string appName)
        {
            try
            {
                var resolver = ServicePartitionResolver.GetDefault();
                var fabricClient = new FabricClient();

                // find application by name
                var apps = fabricClient.QueryManager.GetApplicationListAsync().Result;
                List<string> endpointList = new List<string>();
                var app = apps.First(x => x.ApplicationTypeName.ToLower().Equals(appName.ToLower()));

                // get services of application
                var services = fabricClient.QueryManager.GetServiceListAsync(app.ApplicationName).Result;
                foreach (var service in services)
                {
                    // pull partitions for service
                    var partitions = fabricClient.QueryManager.GetPartitionListAsync(service.ServiceName).Result;
                    foreach (var partition in partitions)
                    {
                        ServicePartitionKey key = new ServicePartitionKey();
                        switch (partition.PartitionInformation.Kind)
                        {
                            case ServicePartitionKind.Singleton:
                                key = ServicePartitionKey.Singleton;
                                break;
                            case ServicePartitionKind.Int64Range:
                                var longKey = (Int64RangePartitionInformation)partition.PartitionInformation;
                                key = new ServicePartitionKey(longKey.LowKey);
                                break;
                            case ServicePartitionKind.Named:
                                var namedKey = (NamedPartitionInformation)partition.PartitionInformation;
                                key = new ServicePartitionKey(namedKey.Name);
                                break;
                        }

                        // Resolve partions for service
                        var resolved = resolver.ResolveAsync(service.ServiceName, key, CancellationToken.None).Result;
                        foreach (var endpoint in resolved.Endpoints)
                        {
                            var address = endpoint.Address;
                            address = address.Replace("\\", "");
                            address = address.Replace("\"\"", "\"address\"");
                            var deserialize
                                = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(address);
                            var url = deserialize["Endpoints"]["address"];

                            // build out uri for service
                            endpointList.Add(url);
                        }
                    }
                }

                return endpointList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}