grammar Thread;

@lexer::header
{
	#pragma warning disable 3021
}

@parser::header
{
	#pragma warning disable 3021

	using System.Text;
}

@parser::members
{
	public static int IGNORED_CHANNEL = 1;
	// This is not ideal, but in certain situations we actually do want to be able to
	// parse through whitespace.
	protected void handleIgnoredChannel(List<IExpressionElement> elements) {
		int idx = CurrentToken.TokenIndex - 1;
		StringBuilder builder = new StringBuilder();
		var oldTokens = new List<IToken>();
		while (idx >= 0)
		{
			var token = _input.Get(idx);
			if (token.Channel != IGNORED_CHANNEL)
				break;

			oldTokens.Add(token);
			--idx;
		}

		if (oldTokens.Count == 0)
			return;

		oldTokens.Reverse();
		foreach (var token in oldTokens)
		{
			builder.Append(token.Text);
		}

		elements.Add(new TextElement(builder.ToString()));
	}
}

// Parser

expression returns [CollectionElement element]
	locals [List<IExpressionElement> subElements]
	: {$subElements = new List<IExpressionElement>(); $element = new CollectionElement($subElements); handleIgnoredChannel($subElements);}
	(	
		(	expression_function {$subElements.Add($expression_function.element);}
		|	expression_block	{$subElements.Add($expression_block.element);}
		|	any_text			{$subElements.Add(new TextElement($any_text.text));}
		)
		{handleIgnoredChannel($subElements);}
	)+
	;

expression_block returns [BlockElement element]
	locals [List<string> argList, IExpressionElement subExpression]
	:	{$argList = new List<string>(); $subExpression = null;}
		BEGIN_BLOCK name=IDENT (args=arguments {$argList = $args.args;})? END_BLOCK
		(subexpr=expression {$subExpression = $subexpr.element;})?
		BEGIN_BLOCK SLASH END_BLOCK
		{$element = new BlockElement($name.text, $argList, $subExpression);}
	;

expression_function returns [FunctionElement element]
	locals [List<string> argList]
	:	{$argList = new List<string>();}
		BEGIN_BLOCK BEGIN_BLOCK name=IDENT (args=arguments {$argList = $args.args;})? END_BLOCK END_BLOCK
		{$element = new FunctionElement($name.text, $argList);}
	;

arguments returns [List<string> args]
	:	{$args = new List<string>();}
		first=raw_value {$args.Add($first.value);}
		(next=raw_value {$args.Add($next.value);})*
	;

raw_value returns [string value]
	:	IDENT {$value = $IDENT.text;}
	|	NUMBER {$value = $NUMBER.text;}
	|	string {$value = $string.value;}
	;

string returns [string value]
	:	STRING {$value = $STRING.text.Substring(1, $STRING.text.Length-2);}
	;

any_text returns [string value]
	locals [StringBuilder builder]
	:	{$builder = new StringBuilder();}
		(
			ESCAPE {$builder.Append($ESCAPE.text.Substring(1));}
		|	not_begin_block {$builder.Append($not_begin_block.text);}
		)+?
	;

not_begin_block
	:	~BEGIN_BLOCK
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

SLASH
	:	'/'
	;

ESCAPE
	:	'\\{'
	|	'\\}'
	;

BEGIN_BLOCK
	:	'{'
	;

END_BLOCK
	:	'}'
	;

STRING
	:	('"' (~('\n' | '\r'))*? '"')
	|	('\'' (~('\n' | '\r'))*? '\'')
	;

IDENT
	:	[a-zA-Z_] [a-zA-Z0-9_]*
	;

NUMBER
	:	'-'? [0-9]+ ('.' [0-9]+)?
	;

COMMENT
	:	'#' ~[\r\n]* -> channel(1)
	;

WS
	:	[ \n\t\r]+ -> channel(1)
	;

TEXT
	:	.+?
	;