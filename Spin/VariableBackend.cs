using System;
using System.Collections.Generic;
using System.Text;

namespace Spin
{
    /// <summary>
    /// Interface for storing and retrieving variables within a sequence.
    /// </summary>
    public interface IVariableBackend
    {
        bool ContainsVariable(string name);
        bool TryGetVariable(string name, out object value);
        void SetVariable(string name, object value);
        void Reset();
    }

    /// <summary>
    /// Simple dictionary-based local variable storage.
    /// </summary>
    public class DictionaryBackend : IVariableBackend
    {
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        public bool ContainsVariable(string name)
        {
            return Variables.ContainsKey(name);
        }

        public bool TryGetVariable(string name, out object value)
        {
            return Variables.TryGetValue(name, out value);
        }

        public void SetVariable(string name, object value)
        {
            Variables[name] = value;
        }

        public void Reset()
        {
            Variables.Clear();
        }
    }
}
