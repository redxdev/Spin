using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Thread.Attributes;
using Thread.Parser;

namespace Thread
{
    public class Sequence
    {
        public delegate void ExpressionBlock(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments);
        public delegate void ExpressionFunction(Sequence sequence, StringBuilder builder, string[] arguments);
        public delegate void CommandFunction(Sequence sequence, string[] arguments);

        public Line? CurrentLine { get; private set; }
        public Line? NextLine { get; private set; }

        private Dictionary<string, ExpressionBlock> _blocks = new Dictionary<string, ExpressionBlock>();
        private Dictionary<string, ExpressionFunction> _functions = new Dictionary<string, ExpressionFunction>();
        private Dictionary<string, CommandFunction> _commands = new Dictionary<string, CommandFunction>();
        
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

        public void ExecuteBlock(string name, StringBuilder builder, IExpressionElement subElement, params string[] arguments)
        {
            if (!_blocks.TryGetValue(name, out ExpressionBlock block))
            {
                throw new SequenceException($"Unknown block \"{name}\"");
            }

            block(this, builder, subElement, arguments);
        }

        public void ExecuteFunction(string name, StringBuilder builder, params string[] arguments)
        {
            if (!_functions.TryGetValue(name, out ExpressionFunction func))
            {
                throw new SequenceException($"Unknown function \"{name}\"");
            }

            func(this, builder, arguments);
        }

        public void ExecuteCommand(string name, params string[] arguments)
        {
            if (!_commands.TryGetValue(name, out CommandFunction command))
            {
                throw new SequenceException($"Unknown command \"{name}\"");
            }

            command(this, arguments);
        }
        
        /// <summary>
        /// Registers all static blocks, functions, and commands within an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
            {
                RegisterType(type);
            }
        }
        
        /// <summary>
        /// Registers all blocks, functions, and commands within an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void RegisterObject(object obj)
        {
            RegisterType(obj.GetType().GetTypeInfo(), obj);
        }

        // Registers a single type, optionally bound to a specific object.
        /// <summary>
        /// Registers all blocks, functions, and commands for single type, optionally bound to a specific object.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <param name="obj">The object to bind to. If null, only static methods will be registered.</param>
        public void RegisterType(TypeInfo type, object obj = null)
        {
            foreach (var method in type.GetMethods())
            {
                if (obj == null && !method.IsStatic)
                    continue;

                var attrs = method.GetCustomAttributes(typeof(SequenceBlockAttribute), false);
                foreach (var attr in attrs.Select(a => a as SequenceBlockAttribute).Where(a => a != null))
                {
                    var del = (ExpressionBlock)method.CreateDelegate(typeof(ExpressionBlock), obj);
                    AddBlock(attr.Name, del);
                }

                attrs = method.GetCustomAttributes(typeof(SequenceFunctionAttribute), false);
                foreach (var attr in attrs.Select(a => a as SequenceFunctionAttribute).Where(a => a != null))
                {
                    var del = (ExpressionFunction)method.CreateDelegate(typeof(ExpressionFunction), obj);
                    AddFunction(attr.Name, del);
                }

                attrs = method.GetCustomAttributes(typeof(SequenceCommandAttribute), false);
                foreach (var attr in attrs.Select(a => a as SequenceCommandAttribute).Where(a => a != null))
                {
                    var del = (CommandFunction)method.CreateDelegate(typeof(CommandFunction), obj);
                    AddCommand(attr.Name, del);
                }
            }
        }

        public void RegisterStandardLibrary()
        {
            RegisterAssembly(this.GetType().Assembly);
        }
    }
}
