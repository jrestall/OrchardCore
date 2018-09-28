using System;
using GraphQL.Types;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public class BooleanFieldProvider : ContentFieldProvider
    {
        public override string Name => nameof(BooleanField);
        public override string Description => "Content boolean field";
        public override Type FieldType => typeof(BooleanGraphType);
        public override Func<ContentElement, object> FieldAccessor => field => (bool)field.Content.Value;
    }
}