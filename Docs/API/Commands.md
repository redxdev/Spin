# Command Reference

## System Library

### set $var value

_Aliases:_ `s`

Set the value of `$var` to `value`.

### next id

_Aliases:_ `begin`

Set the next piece of dialogue to `id`.

### skip

Skip the current piece of dialogue. This will immediately load and execute the next piece of dialogue, though execution of other commands in the current dialogue will continue afterwards.

Make sure you use `next` to set the next piece of dialogue _before_ calling `skip`. Otherwise this will skip to an invalid piece of dialogue and result in an empty string.

### sequence path

_Aliases:_ `seq`

Load and execute the document at `path`. This will immediately execute the document's initialization, though execution of other commands in the current dialogue will continue afterwards.

If you want to jump to a specific piece of dialogue in the given document, use `next` immediately after `sequence`.