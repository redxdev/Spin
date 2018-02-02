namespace Spin
{
    /// <summary>
    /// A reference to a variable in a sequence.
    /// </summary>
    /// <remarks>
    /// Primarily used when passing arguments to blocks/functions/commands, and acts as a signal to <see cref="Sequence.Resolve(object)"/>
    /// that an argument should be replaced by a variable's value.
    /// </remarks>
    public struct VariableRef
    {
        public string Name { get; }

        public VariableRef(string name)
        {
            Name = name;
        }
    }
}
