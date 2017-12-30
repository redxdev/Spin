using System;
using System.Collections.Generic;
using System.Text;

namespace Thread
{
    public interface IVariableBackend
    {
        bool ContainsVariable(string name);
        bool TryGetVariable(string name, out string value);
        void SetVariable(string name, string value);
    }

    public class DictionaryBackend : IVariableBackend
    {
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();

        public bool ContainsVariable(string name)
        {
            return Variables.ContainsKey(name);
        }

        public bool TryGetVariable(string name, out string value)
        {
            return Variables.TryGetValue(name, out value);
        }

        public void SetVariable(string name, string value)
        {
            Variables[name] = value;
        }
    }
}
