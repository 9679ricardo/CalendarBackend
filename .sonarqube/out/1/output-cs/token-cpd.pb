ç
YE:\visual\ricardo\Backend-C#\CalendarBackend\Capa_Validacion\interfaces\IValidarCampos.cs
	namespace 	
Capa_Validacion
 
{ 
public 

	interface 
IValidarCampos #
{ 
bool 
ValidarEmail 
( 
string  
correo! '
)' (
;( )
} 
} ê
YE:\visual\ricardo\Backend-C#\CalendarBackend\Capa_Validacion\interfaces\IValidarEvento.cs
	namespace 	
Capa_Validacion
 
{ 
public 

	interface 
IValidarEvento #
{ 
object 
ValidarEventoId 
( 
int "
uid# &
)& '
;' (
object 
ValidarEvento 
( 
Evento #
evento$ *
)* +
;+ ,
}		 
}

 ß
ZE:\visual\ricardo\Backend-C#\CalendarBackend\Capa_Validacion\interfaces\IValidarUsuario.cs
	namespace 	
Capa_Validacion
 
{ 
public 

	interface 
IValidarUsuario $
{ 
object 
ValidarUsuario 
( 
UsuarioRegister -
usuario. 5
)5 6
;6 7
object 
ValidarLogin 
( 
UsuarioLogin (
usuario) 0
)0 1
;1 2
}		 
}

 ñ
WE:\visual\ricardo\Backend-C#\CalendarBackend\Capa_Validacion\services\SValidarCampos.cs
	namespace 	
Capa_Validacion
 
{ 
public 

class 
SValidarCampos 
:  !
IValidarCampos" 0
{ 
public 
bool 
ValidarEmail  
(  !
string! '
correo( .
). /
{ 	
string		 
	expresion		 
;		 
	expresion 
= 
$str I
;I J
if 
( 
Regex 
. 
IsMatch 
( 
correo $
,$ %
	expresion& /
)/ 0
)0 1
{ 
if 
( 
Regex 
. 
Replace !
(! "
correo" (
,( )
	expresion* 3
,3 4
string5 ;
.; <
Empty< A
)A B
.B C
LengthC I
==J L
$numM N
)N O
{ 
return 
false  
;  !
} 
else 
{ 
return 
true 
;  
} 
} 
else 
{ 
return 
true 
; 
} 
} 	
} 
} ò
WE:\visual\ricardo\Backend-C#\CalendarBackend\Capa_Validacion\services\SValidarEvento.cs
	namespace 	
Capa_Validacion
 
{ 
public 

class 
SValidarEvento 
:  !
IValidarEvento" 0
{ 
public 
object 
ValidarEventoId %
(% &
int& )
uid* -
)- .
{ 	
if		 
(		 
uid		 
<=		 
$num		 
)		 
return		  
new		! $
{		% &
ok		' )
=		* +
false		, 1
,		1 2
msg		3 6
=		7 8
$str		9 G
}		H I
;		I J
return 
null 
; 
} 	
public 
object 
ValidarEvento #
(# $
Evento$ *
evento+ 1
)1 2
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
evento% +
.+ ,
Title, 1
)1 2
)2 3
return4 :
new; >
{? @
okA C
=D E
falseF K
,K L
msgM P
=Q R
$strS m
}n o
;o p
if 
( 
string 
. 
IsNullOrEmpty $
($ %
evento% +
.+ ,
Start, 1
)1 2
)2 3
return4 :
new; >
{? @
okA C
=D E
falseF K
,K L
msgM P
=Q R
$strS t
}u v
;v w
if 
( 
string 
. 
IsNullOrEmpty $
($ %
evento% +
.+ ,
End, /
)/ 0
)0 1
return2 8
new9 <
{= >
ok? A
=B C
falseD I
,I J
msgK N
=O P
$strQ p
}q r
;r s
return 
null 
; 
} 	
} 
} è
XE:\visual\ricardo\Backend-C#\CalendarBackend\Capa_Validacion\services\SValidarUsuario.cs
	namespace 	
Capa_Validacion
 
{ 
public 

class 
SValidarUsuario  
:! "
IValidarUsuario# 2
{ 
readonly 
IValidarCampos 
mValidarCampos  .
;. /
public 
SValidarUsuario 
( 
IValidarCampos -
mValidarCampos. <
)< =
{		 	
this

 
.

 
mValidarCampos

 
=

  !
mValidarCampos

" 0
;

0 1
} 	
public 
object 
ValidarLogin "
(" #
UsuarioLogin# /
usuario0 7
)7 8
{ 	
if 
( 
mValidarCampos 
. 
ValidarEmail +
(+ ,
usuario, 3
.3 4
Email4 9
)9 :
): ;
return< B
newC F
{G H
okI K
=L M
falseN S
,S T
msgU X
=Y Z
$str[ u
}v w
;w x
if 
( 
string 
. 
IsNullOrEmpty $
($ %
usuario% ,
., -
Password- 5
)5 6
)6 7
return8 >
new? B
{C D
okE G
=H I
falseJ O
,O P
msgQ T
=U V
$strW u
}v w
;w x
return 
null 
; 
} 	
public 
object 
ValidarUsuario $
($ %
UsuarioRegister% 4
usuario5 <
)< =
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
usuario% ,
., -
Name- 1
)1 2
)2 3
return4 :
new; >
{? @
okA C
=D E
falseF K
,K L
msgM P
=Q R
$strS m
}n o
;o p
if 
( 
usuario 
. 
Password  
.  !
Length! '
<( )
$num* +
)+ ,
return- 3
new4 7
{8 9
ok: <
== >
false? D
,D E
msgF I
=J K
$strL w
}x y
;y z
if 
( 
mValidarCampos 
. 
ValidarEmail +
(+ ,
usuario, 3
.3 4
Email4 9
)9 :
): ;
return< B
newC F
{G H
okI K
=L M
falseN S
,S T
msgU X
=Y Z
$str[ u
}v w
;w x
return 
null 
; 
} 	
} 
}   