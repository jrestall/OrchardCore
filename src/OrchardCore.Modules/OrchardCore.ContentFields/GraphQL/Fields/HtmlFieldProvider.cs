using System;
using GraphQL.Types;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public class HtmlFieldProvider : ContentFieldProvider
    {
        public override string Name => nameof(HtmlField);
        public override string Description => "Content HTML field";
        public override Type FieldType => typeof(StringGraphType);
        public override Func<ContentElement, object> FieldAccessor => field => field.Content.Html;
    }
}