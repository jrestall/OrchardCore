using System;
using GraphQL.Types;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public class NumericFieldProvider : ContentFieldProvider
    {
        public override string Name => nameof(NumericField);
        public override string Description => "Content numeric field";
        public override Type FieldType => typeof(DecimalGraphType);
        public override Func<ContentElement, object> FieldAccessor => field => (decimal?)field.Content.Value;
    } 
}