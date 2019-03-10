%{
    #include <string.h>
    #include <stdio.h>
    #include <stdlib.h>

    extern int yylex();
    extern int yyparse();
    extern FILE *yyin;
    void yyerror(const char *s);
%}

%token BEGINN
%token END
%token NEW
%token IF
%token WHILE
%token ELSE
%token NULLL
%token READ
%token WRITE
%token CLOSE

%token ID
%token CONST
%token CONST_INT
%token CONST_CHAR
%token CONST_STR

%token INTEGER
%token CHAR
%token STRING

%token ATR
%token NE
%token EQ
%token GT
%token LT
%token GE
%token LE

%%

program: BEGINN dec_list stmt_list END
		;

dec_list: var_declaration 
		| var_declaration dec_list
		;
		
var_declaration: type var_declarator ';' 
		| type var_declarator declarator ';'
		;
		
declarator: ',' var_declarator 
		| ',' var_declarator declarator
		;
		
var_declarator: ID 
		| ID ATR expression
		;
		
type:  '[' ']' type_spec 
		| type_spec
		;
		
type_spec: INTEGER
		| CHAR
		| STRING
		; 
		
stmt_list: /* empty */ 
		| stmt stmt_list
		;
		
stmt: simple_stmt 
		| struct_stmt
		;
		
simple_stmt: assign_stmt
		| io_stmt
		;
	
assign_stmt: ID ATR expression
		;
		
io_stmt: READ '(' ID ')' 
		| WRITE '(' ID ')'
		;

expression: num_exp 
		| NULLL
		| create_exp
		| CONST_CHAR
		| CONST_STR
		;
		
num_exp: n_exp
		| n_exp '+' num_exp
		| n_exp '-' num_exp
		| n_exp '*' num_exp
		| n_exp '/' num_exp
		;
		
n_exp: CONST_INT
		| ID
		| '(' num_exp ')'
		;
		
test_exp: expression GT expression
		| expression LT expression
		| expression EQ expression
		| expression NE expression
		| expression GE expression
		| expression LE expression
		;
		
create_exp: NEW type '[' CONST ']'
		;
		
struct_stmt: if_stmt
		| while_stmt
		;
		
if_stmt: IF '(' test_exp ')' '{' stmt_list '}'
		| IF '(' test_exp ')' '{' stmt_list '}' ELSE '{' stmt_list '}'
		;
		
while_stmt: WHILE '(' test_exp ')' '{' stmt_list '}'
		;

%%

int main(int argc, char *argv[]) {
    ++argv, --argc; /* skip over program name */ 
    
    // sets the input for flex file
    if (argc > 0) 
        yyin = fopen(argv[0], "r"); 
    else 
        yyin = stdin; 
    
    //read each line from the input file and process it
    while (!feof(yyin)) {
        yyparse();
    }
    printf("The file is sintactly correct!\n");
    return 0;
}

void yyerror(const char *s) {
    printf("Error: %s\n", s);
    exit(1);
}