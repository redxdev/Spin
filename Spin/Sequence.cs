using System.Collections.Generic;
using Spin.Builder;
using Spin.Parser;

namespace Spin
{
    public class Sequence
    {
        public delegate void ExpressionBlock(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments);
        public delegate void ExpressionFunction(Sequence sequence, LineBuilder builder, object[] arguments);
        public delegate void CommandFunction(Sequence sequence, object[] arguments);

        private LineBuilder _currentText = null;
        public Line? CurrentLine { get; private set; }
        public Line? NextLine { get; private set; }
        public SpinDocument CurrentDocument { get; private set; }

        /// <summary>
        /// When true, line breaks will be removed except when there are multiple in a row.
        /// </summary>
        public bool AutomaticLineBreaks { get; set; } = true;

        /// <summary>
        /// When true, extra whitespace at the beginning and end of text will be removed.
        /// </summary>
        public bool AutomaticWhitespaceTrim { get; set; } = true;

        private Dictionary<string, ExpressionBlock> _blocks = new Dictionary<string, ExpressionBlock>();
        private Dictionary<string, ExpressionFunction> _functions = new Dictionary<string, ExpressionFunction>();
        private Dictionary<string, CommandFunction> _commands = new Dictionary<string, CommandFunction>();

        private IVariableBackend _backend;
        private IDocumentLoader _loader;

        public Sequence(IVariableBackend backend, IDocumentLoader loader)
        {
            _backend = backend;
            _loader = loader;
        }

        /// <summary>
        /// Resets line data in this sequence.
        /// </summary>
        public void ResetSequence(bool resetDocument = true)
        {
            CurrentLine = null;
            NextLine = null;
            _currentText = null;

            if (resetDocument)
                CurrentDocument = null;
        }

        public void ResetVariables()
        {
            _backend.Reset();
        }

        public void LoadAndStartDocument(string path)
        {
            if (_loader == null)
            {
                throw new SequenceDocumentException($"No document loader provided, cannot load \"{path}\"");
            }

            StartDocument(_loader.LoadDocument(path));
        }

        public void StartDocument(SpinDocument document)
        {
            CurrentDocument = document;

            if (CurrentDocument != null)
            {
                foreach (var expr in CurrentDocument.GetInitialCommands())
                {
                    ParsedCommand[] command;
                    var str = ExecuteExpression(expr).BuildString();
                    if (string.IsNullOrWhiteSpace(str))
                        continue;

                    try
                    {
                        command = ParserUtilities.ParseCommandString(str);
                    }
                    catch (SequenceParseException e)
                    {
                        throw new SequenceException($"Unable to parse \"{str}\" from expression", e);
                    }

                    ExecuteCommand(command);
                }
            }
        }

        public Line? StartNextLine()
        {
            CurrentLine = NextLine;
            NextLine = null;
            _currentText = null;
            return CurrentLine;
        }

        public LineBuilder ExecuteCurrentLine()
        {
            if (!CurrentLine.HasValue)
            {
                _currentText = null;
                return _currentText;
            }

            _currentText = ExecuteExpression(CurrentLine.Value.PrimaryElement);

            foreach (var expr in CurrentLine.Value.CommandElements)
            {
                ParsedCommand[] command;
                var str = ExecuteExpression(expr).BuildString();
                if (string.IsNullOrWhiteSpace(str))
                    continue;

                try
                {
                    command = ParserUtilities.ParseCommandString(str);
                }
                catch (SequenceParseException e)
                {
                    throw new SequenceException($"Unable to parse \"{str}\" from expression", e);
                }

                ExecuteCommand(command);
            }

            return _currentText;
        }

        public void SetNextLine(string name)
        {
            NextLine = CurrentDocument.GetLine(name);
        }

        public void SetNextLine(Line? line)
        {
            NextLine = line;
        }

        public void AddDataLibrary(DataLibrary library)
        {
            foreach (var entry in library.Blocks)
                AddBlock(entry.Key, entry.Value);
            foreach (var entry in library.Functions)
                AddFunction(entry.Key, entry.Value);
            foreach (var entry in library.Commands)
                AddCommand(entry.Key, entry.Value);
        }
        
        public void AddBlock(string name, ExpressionBlock func)
        {
            _blocks.Add(name, func);
        }

        public void AddFunction(string name, ExpressionFunction func)
        {
            _functions.Add(name, func);
        }

        public void AddCommand(string name, CommandFunction func)
        {
            _commands.Add(name, func);
        }

        public LineBuilder ExecuteExpression(IExpressionElement expression)
        {
            var builder = new LineBuilder();
            expression.Execute(this, builder);
            return builder;
        }

        public void ExecuteBlock(string name, LineBuilder builder, IExpressionElement subElement, params object[] arguments)
        {
            if (!_blocks.TryGetValue(name, out ExpressionBlock block))
            {
                throw new SequenceException($"Unknown block \"{name}\"");
            }

            block(this, builder, subElement, arguments);
        }

        public void ExecuteFunction(string name, LineBuilder builder, params object[] arguments)
        {
            if (!_functions.TryGetValue(name, out ExpressionFunction func))
            {
                throw new SequenceException($"Unknown function \"{name}\"");
            }

            func(this, builder, arguments);
        }

        public void ExecuteCommand(string name, params object[] arguments)
        {
            if (!_commands.TryGetValue(name, out CommandFunction command))
            {
                throw new SequenceException($"Unknown command \"{name}\"");
            }

            command(this, arguments);
        }

        public void ExecuteCommand(IEnumerable<ParsedCommand> commands)
        {
            foreach (var cmd in commands)
            {
                ExecuteCommand(cmd);
            }
        }

        public void ExecuteCommand(ParsedCommand command)
        {
            command.Execute(this);
        }

        public void RegisterStandardLibrary()
        {
            AddDataLibrary(DataLibrary.StandardLibrary);
        }

        public object GetVariable(VariableRef vref)
        {
            return GetVariable(vref.Name);
        }

        public object GetVariable(string name)
        {
            if (!TryGetVariable(name, out object value))
            {
                throw new SequenceVariableException($"Undefined variable \"{name}\"");
            }

            return value;
        }

        public bool TryGetVariable(VariableRef vref, out object value)
        {
            return TryGetVariable(vref.Name, out value);
        }

        public bool TryGetVariable(string name, out object value)
        {
            return _backend.TryGetVariable(name, out value);
        }

        public void SetVariable(VariableRef vref, object value)
        {
            SetVariable(vref.Name, value);
        }

        public void SetVariable(string name, object value)
        {
            _backend.SetVariable(name, value);
        }

        public bool ContainsVariable(VariableRef vref)
        {
            return ContainsVariable(vref.Name);
        }

        public bool ContainsVariable(string name)
        {
            return _backend.ContainsVariable(name);
        }

        /// <summary>
        /// Check if the given input is a variable. If it is, return the value of the variable, otherwise return the input as-is.
        /// </summary>
        public object Resolve(object input)
        {
            if (input is VariableRef vref)
            {
                return GetVariable(vref);
            }

            return input;
        }
    }
}
