using Antlr4.Runtime;
using System.IO;
using Spin.Parser;

namespace Spin
{
    public interface IDocumentLoader
    {
        SpinDocument LoadDocument(string path);
    }

    public class FileDocumentLoader : IDocumentLoader
    {
        public SpinDocument LoadDocument(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return ParserUtilities.ParseDocument(new AntlrInputStream(reader));
            }
        }
    }
}
