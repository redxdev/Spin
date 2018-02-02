using Antlr4.Runtime;
using System.IO;
using Spin.Parser;

namespace Spin
{
    /// <summary>
    /// Interface for loading documents.
    /// </summary>
    public interface IDocumentLoader
    {
        SpinDocument LoadDocument(string path);
    }

    /// <summary>
    /// A simple filesystem document loader.
    /// </summary>
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
