using System;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.GraphQL.Types;
using OrchardCore.ContentManagement;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public class TimeFieldProvider : ContentFieldProvider
    {
        public override string Name => nameof(TimeField);
        public override string Description => "Content date time field";
        public override Type FieldType => typeof(TimeSpanGraphType);
        public override Func<ContentElement, object> FieldAccessor => field => (TimeSpan?)field.Content.Value;
    } 
}