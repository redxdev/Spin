# Thread Dialogue System

Thread is a simple library for dialogue trees. It includes a human-readable file format for
dialogue trees, and an interpreter for that format.

The reference interpreter is in C# and should work anywhere C# 7 can be used.

Inspired by [Yarn](https://github.com/InfiniteAmmoInc/Yarn), with a focus on easily editing files
without having a dedicated editor.

Available on [NuGet](https://www.nuget.org/packages/xbloom.Thread/)

## Quick Example

    # Example Document
    > begin hello1

    +hello1
    Hello, world! This is a quick example of a dialogue system.
    +> next hello2

    +hello2
    This is another piece of dialogue.
    +
    > opt "Option 1" hello3
    > opt "Option 2" hello4

    +hello3
    You picked option 1!
    +

    +hello4
    You picked option 2!
    +

_Note: `opt` is not a built-in command. Thread lets you define your own commands, and here we assume `opt` defines an option for a piece of dialogue._

A more complex example can be found at [ThreadExample/sequences/example.ths](https://github.com/redxdev/Thread/blob/master/ThreadExample/sequences/example.ths).

## Terminology

* _line / dialogue line_: A single unit of dialogue. Everything between each set of `+` is a singular line.
* _document_: A file containing dialogue data.
* _sequence_: The data and document storage for a set of dialogues. This is also the name of the interpreter class.

## Extending

_Thread_ has a number of places that can be extended.

### Variable Backend

_Thread_ supports storing and retrieving values in a dialogue sequence. Since you may want to access data outside _Thread_ itself (i.e. data from your game),
you can swap out the storage mechanism that is used in a sequence. The default backend that ships with _Thread_ is a simple `Dictionary<string, object>`, but
it can easily be replaced with the `IVariableBackend` interface.

### Document Loader

The mechanism used to load and parse documents is also replacable via the `IDocumentLoader` interface. This is used primarily by the `seq`/`sequence` command.

### Commands, blocks, and functions

Commands, blocks, and functions can all be added to a sequence. Attributes are used to mark methods that _Thread_ can use. See the following classes:

* `Sequence.ExpressionBlock` and `SequenceBlockAttribute`
* `Sequence.ExpressionFunction` and `SequenceFunctionAttribute`
* `Sequence.CommandFunction` and `SequenceCommandAttribute`

## Blocks vs Functions vs Commands

Blocks contain further text and are used for things like if statements. Blocks look like this:

    {if foo}
    foo is true!
    {/}

Functions and commands are similar, except that functions can emit text and commands cannot be natively used in
dialogue. Functions look like this:

    {{v foo}} # prints the value of foo

Commands are primarly found after each line of dialogue, and can be used to set what the next line of dialogue is.

    +hello1
    Hello, world!
    +> set foo true
    > next hello2

`set foo true` and `next hello2` are commands here.

You can call commands inside lines of dialogue as well, though generally you shouldn't need to:

    +hello1
    {{cmd set foo true}} Hello world!
    +

Finally, you can actually use blocks and functions inside commands:

    +hello1
    Hello, world!
    +
    > {if foo}next hello2{/}
    > {if bar}next hello3{/}

Since expressions just work with text, you can manipulate commands however you want as long as the result is either just whitespace or a valid command.

## Credits

Thread was created by [Sam Bloomberg](https://xbloom.io).

Thread makes use of [ANTLR 4](https://github.com/antlr/antlr4) for parser and lexer generation.