using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Clay.Javascript.Ast
{
    public partial class JSNodeConverter : JsonConverter
    {
        public JSNodeConverter()
        {

        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));
            return typeof(IJSNode).IsAssignableFrom(objectType);
        }
    }
}
