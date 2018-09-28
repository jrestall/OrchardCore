using System;
using GraphQL.Types;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public class DateFieldProvider : ContentFieldProvider
    {
        public override string Name => nameof(DateField);
        public override string Description => "Content date field";
        public override Type FieldType => typeof(DateGraphType);
        public override Func<ContentElement, object> FieldAccessor => field => (DateTime?)field.Content.Value;
    } 
}