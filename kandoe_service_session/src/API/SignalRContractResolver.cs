using System;
using System.Reflection;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace API
{
    // Zorgt ervoor dat entiteiten die we serialiseren en verzenden via SignalR camelcase namen hebben 
    // Bron: http://stackoverflow.com/questions/37832165/signalr-net-core-camelcase-json-contract-resolver
    public class SignalRContractResolver : IContractResolver
    {
        private readonly Assembly assembly;
        private readonly IContractResolver camelCaseContractResolver;
        private readonly IContractResolver defaultContractSerializer;

        public SignalRContractResolver()
        {
            defaultContractSerializer = new DefaultContractResolver();
            camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            assembly = typeof(Connection).GetTypeInfo().Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().Assembly.Equals(assembly))
            {
                return defaultContractSerializer.ResolveContract(type);

            }
            return camelCaseContractResolver.ResolveContract(type);
        }
    }
}