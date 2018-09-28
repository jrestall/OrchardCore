using System;
using GraphQL.Resolvers;
using GraphQL.Types;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.ContentManagement.Metadata.Models;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public abstract class ContentFieldProvider : IContentFieldProvider
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Type FieldType { get; }    
        public abstract Func<ContentElement, object> FieldAccessor { get; }

        public FieldType GetField(ContentPartFieldDefinition field)
        {
            if (field.FieldDefinition.Name != Name) return null;

            return new FieldType
            {
                Name = field.Name,
                Description = Description,
                Type = FieldType,
                Resolver = new FuncFieldResolver<ContentItem, object>(context =>
                {
                    var contentPart = context.Source.Get(typeof(ContentPart), field.PartDefinition.Name);
                    if (contentPart == null) return null;
                    var contentField = contentPart.Get(typeof(ContentField), context.FieldName.FirstCharToUpper());
                    return FieldAccessor(contentField);
                })
            }; 
        }
    }
}