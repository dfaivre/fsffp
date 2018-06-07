
(*
    Partial application - p 302
*)

// simple examples
let add42 = (+) 42
open System.Security.Cryptography
add42 1
[1;2;3] |> List.map add42

let twoIsLessthan = (<) 2
twoIsLessthan 1
[1;2;3] |> List.map twoIsLessthan

let printer = printfn "printing param=%i"
printer 1
[1;2;3] |> List.iter printer

let add1 = (+) 1
let add1ToEach = List.map add1
add1ToEach [1..3]

let filterEvens = List.filter (fun i -> i % 2 = 0)
filterEvens [1..9]


// plugin architecture
let adderWithPluggableLogger logger x y =
    logger "x" x
    logger "y" y
    let result = x + y
    logger "x+y" result
    result

let consoleLogger argName argValue =
    printfn "%s: %A" argName argValue

let addWithConsoleLogger = adderWithPluggableLogger consoleLogger
addWithConsoleLogger 1 2

let add42WithLogger = addWithConsoleLogger 42
[1..3] |> List.map add42WithLogger


// wrapping bcl functions
let replace oldStr newStr (s:string) =
    s.Replace(oldValue=oldStr, newValue=newStr)
let startsWith x (s:string) =
    s.StartsWith(x)

"hello"
|> replace "h" "j"
|> startsWith "j"

["the";"quick";"brown"]
|> List.filter (startsWith "q")


// PIPE
// |> is just: let (|>) x f = f x  !!!!
// it's not _really_ assigning to the _last_ param
// it's just when used with things like List.xxx,
// the first params are usually specified... (like the fun -> ...)
let doSomething x y z = x+y*z
let (||||>) x f = f x
let doSomething' x = 
    let g y =
        let g' z = x + y * z
        g'
    g
let f' = 5 ||||> doSomething'
f' 1 2
let f'' = 5 ||||> doSomething
f'' 1 2
let g' = 5 |> doSomething
g' 1 2
5 ||||> doSomething 1 2
5 ||||> doSomething' 1 2
doSomething 1 2 3
3 |> doSomething 1 2


// reverse pipe - p 308
// let (<|) f x = f x

// printf "%i" 1+2 //err
printf "%i" (1+2)
printf "%i" <| 1 + 2

// psuedo infix notation...
let sub x y = x - y
// 1 add 2 // err
1 |> sub <| 2
// ... partial app (sub 1), then invoke (sub 1) y where y = 2
(1 |> sub) 2
// not the same?
// ... partial app (sub x) where x = 2, then (sub x) y where y = 1
1 |> (sub <| 2)
