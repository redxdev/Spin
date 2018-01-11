using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Spin.Attributes;

namespace Spin
{
    public class DataLibrary
    {
        public static readonly DataLibrary StandardLibrary = DataLibrary.FromAssembly(typeof(DataLibrary).Assembly);

        public static DataLibrary FromAssembly(Assembly assembly)
        {
            var lib = new DataLibrary();
            lib.RegisterAssembly(assembly);
            return lib;
        }

        public Dictionary<string, Sequence.ExpressionBlock> Blocks
        {
            get;
            private set;
        } = new Dictionary<string, Sequence.ExpressionBlock>();

        public Dictionary<string, Sequence.ExpressionFunction> Functions
        {
            get;
            private set;
        } = new Dictionary<string, Sequence.ExpressionFunction>();

        public Dictionary<string, Sequence.CommandFunction> Commands
        {
            get;
            private set;
        } = new Dictionary<string, Sequence.CommandFunction>();

        public void AddBlock(string name, Sequence.ExpressionBlock func)
        {
            Blocks.Add(name, func);
        }

        public void AddFunction(string name, Sequence.ExpressionFunction func)
        {
            Functions.Add(name, func);
        }

        public void AddCommand(string name, Sequence.CommandFunction func)
        {
            Commands.Add(name, func);
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
                    var del = (Sequence.ExpressionBlock)method.CreateDelegate(typeof(Sequence.ExpressionBlock), method.IsStatic ? null : obj);
                    AddBlock(attr.Name, del);
                }

                attrs = method.GetCustomAttributes(typeof(SequenceFunctionAttribute), false);
                foreach (var attr in attrs.Select(a => a as SequenceFunctionAttribute).Where(a => a != null))
                {
                    var del = (Sequence.ExpressionFunction)method.CreateDelegate(typeof(Sequence.ExpressionFunction), method.IsStatic ? null : obj);
                    AddFunction(attr.Name, del);
                }

                attrs = method.GetCustomAttributes(typeof(SequenceCommandAttribute), false);
                foreach (var attr in attrs.Select(a => a as SequenceCommandAttribute).Where(a => a != null))
                {
                    var del = (Sequence.CommandFunction)method.CreateDelegate(typeof(Sequence.CommandFunction), method.IsStatic ? null : obj);
                    AddCommand(attr.Name, del);
                }
            }
        }
    }
}
