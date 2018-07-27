using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;

namespace SourceCode.Clay.Javascript.Ast
{
    public partial class JSWriter : IndentedTextWriter
    {
        public bool Minify { get; set; }

        public JSWriter(TextWriter writer) 
            : base(writer)
        {
        }

        public JSWriter(TextWriter writer, string tabString) 
            : base(writer, tabString)
        {
        }

        public override void WriteLine() => base.WriteLine(string.Empty);

        public override Task WriteLineAsync() => base.WriteLineAsync(string.Empty);
    }
}
