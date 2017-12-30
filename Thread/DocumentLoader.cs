using Antlr4.Runtime;
using System.IO;
using Thread.Parser;

namespace Thread
{
    public interface IDocumentLoader
    {
        ThreadDocument LoadDocument(string path);
    }

    public class FileDocumentLoader : IDocumentLoader
    {
        public ThreadDocument LoadDocument(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return ParserUtilities.ParseDocument(new AntlrInputStream(reader));
            }
        }
    }
}
