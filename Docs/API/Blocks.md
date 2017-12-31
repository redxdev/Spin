# Block Reference

Blocks are functions that can contain a sub-expression.

## System Library

### {hide} subexpr {/}

_Aliases:_ `{h}`

Hides the output of `subexpr`. Effectively a comment, except that `subexpr` is still executed.

#### Example
Input:
```
Hello, {hide} this will not be shown {/} World!
```

Output:
```
Hello, World!
```

### {if value...} subexpr {/}

Executes `subexpr` if _all_ arguments resolve to `true`.

#### Example
Input:
```
{{set $foo true}}
{{set $bar false}}

{if $foo}Foo is true!{/}

{if $bar}Bar is true!{/}
```

Output:
```
Foo is true!
```

### {ifnot value...} subexpr {/}

Executes `subexpr` if _all_ arguments resolve to `false`.

#### Example
Input:
```
{{set $foo true}}
{{set $bar false}}

{ifnot $foo}Foo is false!{/}

{ifnot $bar}Bar is false!{/}
```

Output:
```
Bar is false!
```

### {ifeq a b} subexpr {/}

Executes `subexpr` if `a` is equal to `b`. This is equivalent to the following C# pseudocode:
```csharp
object a;
object b;
if (a.Equals(b))
    subexpr.Execute();
```

#### Example
Input:
```
{{set $foo 1}}
{{set $bar 2}}

{ifeq $foo 1}Foo is 1!{/}
{ifeq $foo 2}Foo is 2!{/}

{ifeq $bar 1}Bar is 1!{/}
{ifeq $bar 2}Bar is 2!{/}
```

Output:
```
Foo is 1!
Bar is 2!
```

### {ifset $var} subexpr {/}

Executes `subexpr` if `$var` is set.

### {ifunset $var} subexpr {/}

Executes `subexpr` if `$var` is not set.

### {ifgt a b} subexpr {/}

Executes `subexpr` if `a > b`. Internally, this uses `IComparable` and will work on any type that implements it.

### {ifgte a b} subexpr {/}

Executes `subexpr` if `a >= b`. Internally, this uses `IComparable` and will work on any type that implements it.

### {iflt a b} subexpr {/}

Executes `subexpr` if `a < b`. Internally, this uses `IComparable` and will work on any type that implements it.

### {iflte a b} subexpr {/}

Executes `subexpr` if `a <= b`. Internally, this uses `IComparable` and will work on any type that implements it.