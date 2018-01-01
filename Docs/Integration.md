# Project Integration Guide

Looking to integrate Spin into your project? Well, here's a handy guide on how to do so!

## Example Project

In case you don't want to read through this whole guide, or if you just want a reference project,
then head over to the [example project](C:\Users\sam\Workspace\Desktop\Spin\SpinExample) and take
a look!

## Setting up Spin

The first thing to do is to add a reference to the Spin library. Generally this will be throught
[nuget](https://www.nuget.org/packages/xbloom.Spin/).

## Creating a Sequence

Your primary point of interaction with spin is the `Sequence` object. A sequence, conceptually,
is a single "set" of dialogues. This differs from a document in that a document is a single file
containing dialogue lines, while a sequence can execute multiple documents.

The first thing to do is to create a sequence and configure a few items.

### Variable Backend

The first thing you need is a variable backend. This is used for storage of variables. The simplest
backend is the `DictionaryBackend`, which uses a simple `Dictionary<string, object>` to store data.
If you don't have any special storage requirements (such as being able to access variables inside
your game) then this should be fine. Otherwise, you should take a look at the `IVariableBackend`
interface found [here](https://github.com/redxdev/Spin/blob/master/Spin/VariableBackend.cs).

### Document Loader

The next object you need is the document loader. This defines how a sequence can load and execute
documents. If you just want one that opens documents based on a filesystem path, you can use the
built-in `FileDocumentLoader`. If you want to implement a loader yourself, look at the `IDocumentLoader`
interface found [here](https://github.com/redxdev/Spin/blob/master/Spin/DocumentLoader.cs).

### The Sequence Object

Once you've decided on the backend and document loader, you can create your sequence object. Here
we're using the built-in backend and document loader, but you can swap them out for whatever you
need to use.

```csharp
using Spin;
/* ... */
var sequence = new Sequence(new DictionaryBackend(), new FileDocumentLoader());
```

Next, we need to register Spin's standard library into this sequence. Sequences contain the list
of all functions, blocks, and commands that can be used but by default they don't have any registered.
To do so, just call the following function:

```csharp
sequence.RegisterStandardLibrary();
```

This is also around when you'd want to register any custom functions (we'll get to that later).

## Running a Document

Once we have our `Sequence`, we can run a document.

```csharp
sequence.LoadAndStartDocument("path/to/document.spd");
```

This will throw a `SequenceException` if there are any problems. The function `LoadAndStartDocument()`
uses the document loader to read a document into the sequence (replacing an existing one, if there is one)
and executes any initialization commands in that document (any commands at the top of the document that
aren't associated with a piece of dialogue).

From here, we can start running the sequence.

```csharp
while (sequence.StartNextLine().HasValue)
{
    /* ... */
}
```

This while loop will keep loading the next line until a line isn't set as the "next line", which would be
at the end of a dialogue.

Inside that while loop, we can execute commands in the current line and get text back.

```chsarp
var text = sequence.ExecuteCurrentLine().BuildString();
Console.WriteLine(text);
```

`ExecuteCurrentLine()` executes the current line and returns a `LineBuilder`. This is a utility object that contains
the result of executing the current dialogue line, but split into "elements" (`ILineElement`). the call to `BuildString()`
takes all of the elements and creates the final string result.

The reason for the intermediate `LineBuilder` object is that you may want to eventually define your own custom elements
and then process them. These can be used for formatting and markup (i.e. you might have a BoldLineElement to mark that
a section of text is bold).

And that's about it! A simple example of executing a dialogue sequence.

## Custom Functions, Blocks, and Commands

You can easily define your own custom functions, blocks, and commands to be used inside a document.

First of all, the difference between them. __Commands__ are the simplest to use - they are primarily seen
at the end of a piece of dialogue and generally are used to tell Spin or your game information about the dialogue.
For example, `next` is a built-in command that sets what the next piece of dialogue is.

Next, __functions__. Functions work similarly to commands, except that they can emit their own text for the current
dialogue.

Finally, __blocks__. Blocks are effectively functions that have a sub-expression inside them. They are generally used
for markup or conditional statements.

_Note:_ Commands, functions, and blocks are registered separately and can have the same names as each other. That means
you can have potentially three different version of `foo`: a command, a function, and a block.

### Static vs Instance Methods

Spin supports both static and instance methods for commands, functions, and blocks. Examples here will mostly use static
methods, but defining instance methods will work much the same way.

### Handling Variables

Functions, commands, and blocks all receive a list of arguments as an array of `object`. This array may contain references
to variables in the current sequence, and as such you must process the array before using it. A simple way to do so is as
follows:

```csharp
for (var i = 0; i < arguments.length; ++i)
{
    arguments[i] = sequence.Resolve(arguments[i]);
}
```

Or, if you want to use Linq:

```csharp
arguments = arguments.Select(arg => sequence.Resolve(arg)).ToArray();
```

The call to `sequence.Resolve()` checks if the passed object is a variable reference. If it is, it will try to get the value
of that variable. If not, it returns the object unchanged. If you need to check for a variable reference manually for whatever
reason, you can use the following:

```csharp
object arg = /* assume arg is something passed in from the arguments array */
if (arg is VariableRef)
{
    // arg is a variable reference
}
else
{
    // not a variable reference
}
```

### Argument Utilities

A small utility library that does checks on arguments can be found [here](https://github.com/redxdev/Spin/blob/master/Spin/Utility/ArgumentUtils.cs).

### Defining a Custom Command

Custom commands use the following signature:

```csharp
[SequenceCommand("mycommand")] // requires the Spin.Attributes namespace
void MyCommand(Sequence sequence, object[] arguments)
{
    /* ... */
}
```

The `SequenceCommand` attribute may be used multiple times on the same command to give it aliases.

Here's an example command that adds `1` to the first argument given, then prints it.

```csharp
[SequenceCommand("add_and_print")]
public static void AddAndPrint(Sequence sequence, object[] arguments)
{
    ArgumentUtils.Count("add_and_print", arguments, 1); // errors if more than 1 or less than 1 arguments were passed in
    var arg = sequence.Resolve(arguments[0]); // resolve variables, if there are any
    var num = Convert.ToDouble(arg, CultureInfo.InvariantCulture) // requires the System and System.Globalization namespaces
    num += 1;
    Console.WriteLine(num);
}
```

### Defining a Custom Function

Custom functions use the following signature:

```csharp
[SequenceFunction("myfunction")]
void MyFunction(Sequence sequence, LineBuilder builder, object[] arguments)
{
    /* ... */
}
```

The `SequenceFunction` attribute may be used multiple times on the same function to give it aliases.

__WARNING:__ If a function is not going to emit any text, then it should run `builder.PushEmpty()`. Failure to do so may
result in extra whitespace appearing before calls to the function, as `PushEmpty()` is used to determine when to cleanup
extra whitespace.

Here's an example that adds `1` to the first argument given, then adds it to the output text.

```csharp
[SequenceFunction("addone")]
public static void AddOne(Sequence sequence, LineBuilder builder, object[] arguments)
{
    ArgumentUtils.Count("addone", arguments, 1); // errors if more than 1 or less than 1 arguments were passed in
    var arg = sequence.Resolve(arguments[0]); // resolve variables, if there are any
    var num = Convert.ToDouble(arg, CultureInfo.InvariantCulture) // requires the System and System.Globalization namespaces
    num += 1;
    builder.PushString(num.ToString()); // emit the result
}
```

### Defining a Custom Block

Custom blocks use the following signature:

```csharp
[SequenceBlock("myblock")]
void MyBlock(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
{
    /* ... */
}
```

The `SequenceBlock` attribute may be used multiple times on the same block to give it aliases.

__WARNING:__ If a block is not going to emit any text, then it should run `builder.PushEmpty()`. Failure to do so may
result in extra whitespace appearing before the block, as `PushEmpty()` is used to determine when to cleanup
extra whitespace.

Here's an example that only executes the block's sub-expression if the first argument is greater than or
equal to `1.5`.

```csharp
[SequenceFunction("myblock")]
public static void MyBlock(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
{
    ArgumentUtils.Count("addone", arguments, 1); // errors if more than 1 or less than 1 arguments were passed in
    var arg = sequence.Resolve(arguments[0]); // resolve variables, if there are any
    var num = Convert.ToDouble(arg, CultureInfo.InvariantCulture) // requires the System and System.Globalization namespaces
    if (num >= 1.5)
    {
        // execute the sub-expression
        subElement.Execute(sequence, builder);
    }
    else
    {
        builder.PushEmpty(); // this must be called if we aren't going to emit anything.
    }
}
```

You may want to capture and work on the text that a sub-expression emits. This is possible by creating a new LineBuilder:

```csharp
[SequenceFunction("myblock")]
public static void MyBlock(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
{
    ArgumentUtils.Count("addone", arguments, 1); // errors if more than 1 or less than 1 arguments were passed in
    var arg = sequence.Resolve(arguments[0]); // resolve variables, if there are any
    var num = Convert.ToDouble(arg, CultureInfo.InvariantCulture) // requires the System and System.Globalization namespaces
    if (num >= 1.5)
    {
        // execute the sub-expression
        var subBuilder = new LineBuilder();
        subElement.Execute(sequence, subBuilder);

        /* do something with the data in subBuilder */

        // add the subBuilder's data to the parent builder
        builder.PushBuilder(subBuilder); // by default, this will PushEmpty() if subBuilder is empty. To turn off this
                                         //behavior, pass false as the second argument.
    }
    else
    {
        builder.PushEmpty(); // this must be called if we aren't going to emit anything.
    }
}
```

### Registering Blocks, Functions, and Commands

Registering your own methods should generally be done right before or after registering the standard library on a sequence.

To register all static methods in an entire assembly, use `Sequence.RegisterAssembly()`. For example, if your game is a single
project:

```csharp
sequence.RegisterAssembly(GetType().Assembly);
```

This will register all methods marked with the appropriate attributes in your assembly.

To register static methods from a specific type:

```csharp
sequence.RegisterType(typeof(MyLibrary));
```

To register methods on a specific object (this will also register static methods):

```csharp
sequence.RegisterObject(myObject);
```

If you want to register methods manually, you may use `AddBlock()`, `AddFunction()`, and `AddCommand()` on the `Sequence` object.