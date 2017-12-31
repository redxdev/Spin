# Function Reference

## System Library

### {{$var}}

Returns the value of the variable `$var`. Technically, this is not a function but for the purposes of documentation it acts like one.

#### Example
Input:
```
{{set $foo "bar"}}
{{$foo}}
```

Output:
```
bar
```

### {{echo value...}}

_Aliases:_ `{{v}}`, `{{value}}`, `{{e}}`

Returns the value of `value` (all arguments will be concatinated).

### {{set $var value}}

_Aliases:_ `{{s}}`

Sets the value of `$var` to `value`.

### {{cmd name args...}}

_Aliases:_ `{{c}}`

Runs a command named `name` with the arguments `args...`

### {{noop}}

Does nothing.