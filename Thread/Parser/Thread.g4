grammar Thread;

@parser::header
{
	#pragma warning disable 3021
}

@lexer::header
{
	#pragma warning disable 3021
}

// Parser

expression
	:
	(	expression_function
	|	expression_block
	|	.
	)*?
	;

expression_block
	:	BEGIN_BLOCK name=IDENT (args=arguments)? END_BLOCK
		subexpr=expression
		BEGIN_BLOCK END END_BLOCK
	;

expression_function
	:	BEGIN_BLOCK BEGIN_BLOCK name=IDENT (args=arguments)? END_BLOCK END_BLOCK
	;

arguments returns [List<string> args]
	:	{$args = new List<string>();}
		first=raw_value {$args.Add($first.value);}
		(COMMA next=raw_value {$args.Add($next.value);})*
	;

raw_value returns [string value]
	:	IDENT {$value = $IDENT.text;}
	|	string {$value = $string.value;}
	;

string returns [string value]
	:	STRING {$value = $STRING.text.Substring(1, $STRING.text.Length-2);}
	;

// Lexer

LINE_DELIM
	:	'+'
	;

INIT
	:	'@'
	;

COMMAND_BEGIN
	:	'>'
	;

END
	:	'/'
	;

BEGIN_BLOCK
	:	'{'
	;

END_BLOCK
	:	'}'
	;

COMMA
	:	','
	;

STRING
	:	('"' (~('\n' | '\r'))*? '"')
	|	('\'' (~('\n' | '\r'))*? '\'')
	;

IDENT
	:	[a-zA-Z_] [a-zA-Z0-9_]*
	;

COMMENT
	:	'#' ~[\r\n]* -> channel(HIDDEN)
	;

WS
	:	[ \n\t\r]+ -> channel(HIDDEN)
	;

TEXT
	:	.+?
	;