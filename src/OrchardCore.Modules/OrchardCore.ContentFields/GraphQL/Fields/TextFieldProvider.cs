using System;
using GraphQL.Types;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace OrchardCore.ContentFields.GraphQL.Fields
{
    public class TextFieldProvider : ContentFieldProvider
    {
        public override string Name => nameof(TextField);
        public override string Description => "Content text field";
        public override Type FieldType => typeof(StringGraphType);
        public override Func<ContentElement, object> FieldAccessor => field => field.Content.Text;
    }
}