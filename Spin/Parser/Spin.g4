grammar Spin;

@lexer::header
{
	#pragma warning disable 3021
}

@parser::header
{
	#pragma warning disable 3021

	using System.Text;
	using System.Linq;
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

//
// Document
//

document returns [SpinDocument doc]
	locals [List<Line> lines, IExpressionElement[] init]
	:	{$lines = new List<Line>(); $init = new IExpressionElement[] {};}
		(command_list {$init = $command_list.elements.ToArray();})?
		(line {$lines.Add($line.result);})*
		EOF
		{$doc = new SpinDocument($lines.ToArray(), $init);}
	;

//
// Lines
//

line returns [Line result]
	locals [IExpressionElement primaryElement, IExpressionElement[] commandElements]
	:	{$primaryElement = new NoopElement(); $commandElements = new IExpressionElement[] {};}
		LINE_DELIM name=IDENT
		(expr=expression {$primaryElement = $expr.element;})?
		LINE_DELIM
		(command_list {$commandElements = $command_list.elements.ToArray();})?
		{$result = new Line($name.text, $primaryElement, $commandElements);}
	;

//
// Commands
//

command returns [List<ParsedCommand> commands]
	:	{$commands = new List<ParsedCommand>();}
		first=single_command {$commands.Add($first.cmd);}
		(SEMI next=single_command {$commands.Add($next.cmd);})*
	;

single_command returns [ParsedCommand cmd]
	locals [List<object> argList]
	:	{$argList = new List<object>();}
		name=IDENT (args=arguments {$argList = $args.args;})?
		{$cmd = new ParsedCommand($name.text, $argList);}
	;

command_list returns [List<IExpressionElement> elements]
	:	{$elements = new List<IExpressionElement>();}
	(
		COMMAND_BEGIN expression {$elements.Add($expression.element);}
	)+
	;

//
// Expressions
//

expression returns [CollectionElement element]
	locals [List<IExpressionElement> subElements]
	: {$subElements = new List<IExpressionElement>(); $element = new CollectionElement($subElements); handleIgnoredChannel($subElements);}
	(	
		(	expression_function {$subElements.Add($expression_function.element);}
		|	expression_variable {$subElements.Add($expression_variable.element);}
		|	expression_block	{$subElements.Add($expression_block.element);}
		|	any_text			{$subElements.Add(new TextElement($any_text.value));}
		)
		{handleIgnoredChannel($subElements);}
	)+
	;

expression_block returns [BlockElement element]
	locals [List<object> argList, IExpressionElement subExpression]
	:	{$argList = new List<object>(); $subExpression = null;}
		BEGIN_BLOCK name=IDENT (args=arguments {$argList = $args.args;})? END_BLOCK
		(subexpr=expression {$subExpression = $subexpr.element;})?
		BEGIN_BLOCK SLASH END_BLOCK
		{$element = new BlockElement($name.text, $argList, $subExpression);}
	;

expression_function returns [FunctionElement element]
	locals [List<object> argList]
	:	{$argList = new List<object>();}
		BEGIN_BLOCK BEGIN_BLOCK name=IDENT (args=arguments {$argList = $args.args;})? END_BLOCK END_BLOCK
		{$element = new FunctionElement($name.text, $argList);}
	;

expression_variable returns [VariableElement element]
	:	BEGIN_BLOCK variable_value END_BLOCK {$element = new VariableElement($variable_value.value);}
	;

arguments returns [List<object> args]
	:	{$args = new List<object>();}
		first=raw_value {$args.Add($first.value);}
		(next=raw_value {$args.Add($next.value);})*
	;

raw_value returns [object value]
	:	IDENT {$value = $IDENT.text;}
	|	variable_value {$value = $variable_value.value;}
	|	boolean_value {$value = $boolean_value.value;}
	|	NUMBER {$value = double.Parse($NUMBER.text);}
	|	string {$value = $string.value;}
	;

string returns [string value]
	:	STRING {$value = $STRING.text.Substring(1, $STRING.text.Length-2);}
	;

boolean_value returns [bool value]
	:	BOOL_TRUE {$value = true;}
	|	BOOL_FALSE {$value = false;}
	;

variable_value returns [VariableRef value]
	:	VARIABLE {$value = new VariableRef($VARIABLE.text.Substring(1));}
	;

any_text returns [string value]
	locals [StringBuilder builder]
	:	{$builder = new StringBuilder();}
		(
			ESCAPE {$builder.Append($ESCAPE.text.Substring(1));}
		|	not_special_block {$builder.Append($not_special_block.text);}
		)+?
		{$value = $builder.ToString();}
	;

not_special_block
	:	~(BEGIN_BLOCK | END_BLOCK | LINE_DELIM | ESCAPE | COMMAND_BEGIN)
	;

// Lexer

ESCAPE
	:	'\\{'
	|	'\\}'
	|	'\\+'
	|	'\\>'
	|	'\\#'
	|	'\\$'
	|	'\\"'
	|	'\\\\'
	;

STRING
	:	('"' (ESCAPE | ~('\n' | '\r' | '$' | '{' | '}'))*? '"')
	;

LINE_DELIM
	:	'+'
	;

SEMI
	:	';'
	;

COMMAND_BEGIN
	:	'>'
	;

SLASH
	:	'/'
	;

VARIABLE
	:	'$' IDENT
	;

BEGIN_BLOCK
	:	'{'
	;

END_BLOCK
	:	'}'
	;

BOOL_TRUE
	:	[Tt][Rr][Uu][Ee]
	;

BOOL_FALSE
	:	[Ff][Aa][Ll][Ss][Ee]
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