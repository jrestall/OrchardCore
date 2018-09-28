using GraphQL.Types;
using OrchardCore.ContentManagement;
using OrchardCore.Forms.Models;

namespace OrchardCore.Forms.GraphQL
{
    public class LabelPartQueryObjectType : ObjectGraphType<LabelPart>
    {
        public LabelPartQueryObjectType(IContentManager contentManager)
        {
            Name = "LabelPart";

            Field(x => x.For, nullable: true);
            FieldAsync<StringGraphType>("value", resolve: async context =>
            {
                var contentItem = context.Source.ContentItem;
                var contentItemMetadata = await contentManager.PopulateAspectAsync<ContentItemMetadata>(contentItem);
                return contentItemMetadata.DisplayText;
            });
        }
    }
}