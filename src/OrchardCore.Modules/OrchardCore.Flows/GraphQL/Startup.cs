using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis;
using OrchardCore.Flows.Models;
using OrchardCore.Modules;

namespace OrchardCore.Flows.GraphQL
{
    [RequireFeatures("OrchardCore.Apis.GraphQL")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQLQueryType<FlowPart, FlowPartQueryObjectType>();
            services.AddGraphQLQueryType<FlowMetadata, FlowMetadataQueryObjectType>();
        }
    }
}