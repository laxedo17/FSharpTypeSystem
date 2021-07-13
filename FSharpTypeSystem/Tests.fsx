//CONTROL DE FLUXO CON F#

//LOOPS FOR
//Conta hacia adiante con un loop for en F#
for numero in 1 .. 10 do
    printfn "Ti eres o numero %d" numero

//Conta hacia atras usando un loop for en F#
for numero in 10 .. -1 .. 1 do
    printfn "Eres o numero %d" numero

//Tipico loop estilo for each
let idsClientes = [ 45 .. 99 ]
for idCliente in idsClientes do
    printfn "O cliente con id %d fixo unha compra" idCliente

//Range -rango- con pasos personalizados
for par in 2 .. 2 .. 10 do
    printfn "O numero %d e un numero par" par

//LOOPS WHILE
open System.IO  
let lector = new StreamReader(File.OpenRead @"Arquivo.txt") //Abrindo un alista a un arquivo de texto
while (not lector.EndOfStream) do //loop while que corre mentres o lector non este ao final do stream
    printn "%s" (lector.ReadLine())

// Romper o loop (break)
// A principal restrición dos loops en F # é que non hai ningún concepto do comando break
// non podes saír dun circuíto (loop) prematuramente. Se queres simular a saída prematura de
// un bucle, deberías considerar substituír o bucle por unha secuencia de valores que filtres
// (ou usar takeWhile), e facer un bucle sobre esa secuencia. De feito, por se non te decataches
// aínda así, ningún código que devolviches nunha función utiliza "devolución anticipada", porque non se admite en F #. Unha vez máis, porque todo é unha expresión, cada rama (branch) debe ter un resultado equivalente. 

//COMPREHENSIONS
//Son unha maneira poderosa de xerar listas, arrays e sequences de datos basadas na sintaxis estilo loop for
//O equivalente mais proximo seria o uso do metodo System.Linq.Enumerable.Range() en C#, excepto que en vez de ser un soporte de libreria, F# ten soporte nativo.
//Asi funcionan as comprehensions
open System 
//Xerando un array coas letras do alfabeto en maiusculas
let arrayDeCaracteres = [| for letra in 'a' .. 'z' -> Char.ToUpper letra |]
//Xerando os cadrados dos numeros 1 ao 10
let listaDeCadrados = [ for i in 1 .. 10 -> i * i]
//Xerando cadeas de texto string arbitrarias basandose en cada cuarto numero entre 2 e 20
let seqDeStrings = seq { for i in 2 .. 4 .. 20 -> sprintf "Numero %d" i }
//As comprehensions son utiles para xerar coleccions de datos rapidamente basandose nun set de numeros -por exemplo, chamar a unha stored procedure de SQL para cargar todos os clientes entre dous rangos de datos

//LOXICA DE BRANCHING -ramificacions- en F#
//Equivalencia de F# con if/then e switch case

//PATTERN MATCHING
// Digamos que queres escribir código que calcule o límite de crédito dun cliente, en función da puntuación de crédito de terceiros do cliente e o número de anos que leva esta persoan cliente teu. Por exemplo, se o cliente ten unha puntuación de crédito media e
// estivo un ano contigo, daríaslle un límite de crédito de 500€.

//Sen PATTERN MATCHING, usando if/else
let getLimite = 
    if puntuacion = "media" && anos = 1 then 500 //clausula simple
    elif puntuacion = "boa" && (anos = 0 || anos = 1) then 750 //clausula complexa, AND e OR combinados
    elif puntuacion = "boa" && anos = 2 then 1000
    elif score = "boa" then 2000 //un abarcatodo multifuncion para bos clientes
    else 250 //Multifuncion para outros clientes

 let cliente = "bo", 1
 getLimite cliente

// Podes pensar que se trata dun código estándar, pero o certo é que razoar sobre as relacións de todas as cláusulas como unha peza unificada da lóxica empresarial pode ser difícil; cada cláusula
// non está completamente relacionado. Non hai nada que impida comparar accidentalmente
// contra outra cousa en lugar de comparar a puntuacion nunha soa rama, por exemplo. Ademais, fíxate en que as partes multifuncion  do código son algo implícitas; isto pode levar a todo tipo de estraños
// e marabillosos bugs, especialmente cando tes máis que un par de cláusulas 


// A solución de F # para modelar a lóxica de ramificación é unha construción completamente diferente chamada patrón correspondente/coincidencia de patróns (PATTERN MATCHING) e tamén é un mecanismo de ramificación baseado na expresión permite a vinculacion inline para unha gran variedade de construcións de F#, noutras palabras, a capacidade de desconstruír unha tupla ou record mentres coinciden os patróns. Quizais a forma máis acertada de describilo é que é como un switch/case vitaminado; os principios son similares, pero pattern matching leva as cousas moito máis alá. 

//Agora con PATTERN MATCHING
let getLimitePatternMatching =
    match cliente with //facendo coinbcidencia de patrons implicitamente con tupla de puntuacion e anos
    | "medio", 1 -> 500 //se ten puntuacion media e un ano, o limite e 500€
    | "bo", 0 | "bo", 1 -> 750 //Duas condicions coincidentes que conlevan un limite de 750€
    | "b0", 2 -> 1000
    | "bo", _ -> 2000 //Multifuncion catchall para os clientes con boa puntuacion
    | _ -> 250 //Catchall multifuncion para todos os outros clientes

getLimitePatternMatching cliente

//Guards
// F # tamén ofrece unha boa escotilla de escape para o patter matching e para que poidas facer calquera forma de chequear dentro dun patrón en lugar de só coincidir con valores. Isto coñécese como cláusula de garda when. Por exemplo, poderías combinar dous dos patróns anteriores nun só co seguinte:

let getLimiteDeCredito cliente =
    match cliente with
    | "medio", 1 -> 500
    | "bo", anos when anos < 2 -> 750 //usando a clausula de guarda -guard- when para especificar patron personal
    // | ... etc

// Cando (when) non usar cando (when)?
// Obviamente tes moito control ao usar a cláusula when na combinación de patróns, pero a
// o custo está asociado a isto: o compilador non intentará descubrir nada do que ocorre
// dentro do guard. En canto uses un, o compilador non poderá realizar unha concordancia de patróns exhaustiva para ti (aínda que esgotará todas as posibilidades que poida demostrar). 

//Nested matches -- coincidencias anidadas
let getLimiteDeCreditoNested cliente =
    match cliente with
    | "medio", 1 -> 500
    | "bo", anos -> //buscando coincidencias en "bo" e asociando anos a un symbol
        match anos with //match anidado no valor de anos
        | 0 | 1 -> 750 //match de valor unico
        | 2 -> 1000
        | _ -> 2000
    | _ -> 250 //catchall global

//Con collections. Practica
// F # permíteche combinar patróns de forma segura cunha list ou array. En vez de ter código que primeiro comproba a lonxitude dunha list antes de indexala, podes facer que o compilador de forma segura
// extraia valores da list nunha operación. Por exemplo, digamos que temos un código que
// debería operar nunha lista de clientes:
//  Se non se fornecen clientes (a lista está baleira), lanzará un erro.
//  Se hai un cliente, imprime o nome do cliente.
//  Se hai dous clientes, desexamos imprimir a suma dos seus saldos.
//  Se non, imprimimos o número total de clientes subministrados 


// 1 Crea un tipo de record de cliente que teña campos Saldo: int e Nome: string.
// 2 Crea unha función chamada xestionarClientes que inclúa unha lista de rexistros de clientes.
// 3 Implementar a lóxica anterior empregando a lóxica estándar if / then. Podemos usar
// List.length para calcular a lonxitude dos clientes ou escribir explícitamente unha type-annotate do cliente como lista de clientes e obtén a propiedade Length na lista.
// 4 Usa failwith para crear unha excepción (por exemplo, failwith "Non hai clientes subministrados!").
// 5 Agora entra a seguinte versión de pattern matching para comparación 

//Practica
type Cliente = { Saldo: int; Nome : string; }

let xestionarClientes clientes =
    match clientes with 
    | [] -> failwith "Non se subministraron clientes!" //match contra lista vacia
    | [ cliente ] -> printfn "Cliente unico, de nome %s" cliente.Nome //match contra lista dun cliente
    | [ primeiro; segundo ] -> printfn "Dous clientes, saldo: %d" (primeiro.Saldo + segundo.Saldo) //match contra lista de dous clientes
    | clientes -> "Clientes subministrados, %d" clientes.Length //facendo matching con outras list

xestionarClientes [] //lanza excepcion
xestionarClientes [ {Saldo = 10; Nome = "Laurie"} ] //imprime o nome ao ser un so cliente

//Records
//Tamen podemos facer pattern matching en Records. Exemplo de pattern matching contra o type ficticio Cliente para devolver unha descripcion de records.
let getEstado cliente =
    match cliente with
    | { Saldo = 0 } -> "Este cliente non ten un peso" //Pattern matching contra o campo Saldo
    | { Nome = "Laurie" } -> "Esta e unha muller moi guapa" //Match contra o campo Nome
    | { Nome = nome; Saldo = 50 } -> sprintf "Aqui hai carto, verdade cliente %d" nome
    | { Nome = nome } -> sprintf "%s e un cliente normal" //Catchall, asociando Nome co symbol nome
{ Saldo = 50; Nome = "Laurie" } |> getEstado

// Incluso podes mesturar e combinar patróns. Que tal comprobar as seguintes tres condicións ao mesmo tempo:
// 1 A lista de clientes ten tres elementos.
// 2 O primeiro cliente chámase "Lina".
// 3 O segundo cliente ten un saldo de 25. 
let clientela = [ { Saldo = 10; Nome = "Puri" }]
match clientela with  
//Pattern matchin contra unha lista de tres items con campos (fields) especificos
| [ { Nome = "Lina"}; { Saldo = 25}; _ ] -> "Temos unha coincidencia co que buscabamos!"
| _ -> "Non se corresponde"

// Con dous mecanismos de ramificación á túa disposición, que debes empregar: if/then ou pattern matching? O meu consello é usar de xeito predeterminado a pattern matching. É máis potente, máis doado de razoar e moito máis flexible. A única vez que é máis sinxelo de usar if/then é
// cando estás a traballar con código que devolve unit e implicitamente estraficitamente falta o
// rama predeterminada default
//Exemplo
let clienteDous = { Saldo = 150; Nome = "Laurie" }
if clienteDous.Nome = "Laurie" then printfn "Bon jour mon'amour"

//O mesmo match pero con clausula default explicita
match clienteDous.Nome with
| "Laurie" -> printfn "Bon jour mon'amour"
| _ -> ()

// O compilador F # é o suficientemente intelixente con if/then para poñer automaticamente un controlador default por ti para o ramal else, pero a construct de matchsempre espera un valor predeterminado explícito manexador 