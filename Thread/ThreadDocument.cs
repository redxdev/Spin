using System;
using System.Collections.Generic;
using System.Text;
using Thread.Parser;

namespace Thread
{
    public class ThreadDocument
    {
        private Dictionary<string, Line> _lines = new Dictionary<string, Line>();
        private IExpressionElement[] _initCommands;

        public ThreadDocument(Line[] lines, IExpressionElement[] initialCommands)
        {
            foreach (var line in lines)
            {
                _lines.Add(line.Name, line);
            }

            _initCommands = initialCommands;
        }

        public Line GetLine(string name)
        {
            if (!TryGetLine(name, out Line line))
                throw new SequenceDocumentException($"Unknown line named \"{name}\"");

            return line;
        }

        public bool TryGetLine(string name, out Line line)
        {
            return _lines.TryGetValue(name, out line);
        }

        public bool ContainsLine(string name)
        {
            return _lines.ContainsKey(name);
        }

        public IExpressionElement[] GetInitialCommands()
        {
            return _initCommands;
        }
    }
}
