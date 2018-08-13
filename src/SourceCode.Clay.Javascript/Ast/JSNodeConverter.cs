using Newtonsoft.Json;
using System;

namespace SourceCode.Clay.Javascript.Ast
{
    public partial class JSNodeConverter : JsonConverter
    {
        public JSNodeConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType is null) throw new ArgumentNullException(nameof(objectType));
            return typeof(IJSNode).IsAssignableFrom(objectType);
        }
    }
}
