
(*
    Partial Application pg 182
*)

let add x y = x + y
let z = add 1 2
let add42 = add 42
add42 3

let genericLogger1 func input =
    printfn "input is %A" input
    let result = func input
    printfn "result is: %A" result
    result

let genericLogger beforeLogger afterLogger func input =
    beforeLogger input
    let result = func input
    afterLogger result
    result

let add1 = add 1
genericLogger
    (printfn "input: %A")
    (printfn "result: %A")
    add1 2

let consoleLogger func input = 
    genericLogger 
        (fun i -> printfn "input: %A" i) 
        (fun r -> printfn "result: %A" r)
        func input

consoleLogger add1 2
