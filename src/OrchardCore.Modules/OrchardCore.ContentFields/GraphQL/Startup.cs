using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.GraphQL.Fields;
using OrchardCore.ContentFields.GraphQL.Types;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.Modules;

namespace OrchardCore.ContentFields.GraphQL
{
    [RequireFeatures("OrchardCore.Apis.GraphQL")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentFieldProvider, ObjectGraphTypeFieldProvider>();
            services.AddScoped<IContentFieldProvider, BooleanFieldProvider>();
            services.AddScoped<IContentFieldProvider, DateFieldProvider>();
            services.AddScoped<IContentFieldProvider, DateTimeFieldProvider>();
            services.AddScoped<IContentFieldProvider, HtmlFieldProvider>();
            services.AddScoped<IContentFieldProvider, NumericFieldProvider>();
            services.AddScoped<IContentFieldProvider, TextFieldProvider>();
            services.AddScoped<IContentFieldProvider, TimeFieldProvider>();

            services.AddGraphQLQueryType<LinkField, LinkFieldQueryObjectType>();
        }
    }
}