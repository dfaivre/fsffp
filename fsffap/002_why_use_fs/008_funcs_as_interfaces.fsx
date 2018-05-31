open System

(*
    Functions as Interfaces pg 187
*)

// == Decorator Pattern (Calculator) ==
let addingCalculator x = x + 1

let loggingCalculator calc x =
    printfn "calc (%A) val: %A" calc x
    let result = calc x
    printfn "result: %A" result
    result

loggingCalculator addingCalculator 3

// generic version
let add1 input = input + 1
let times2 input = input * 2
let genericLogger func input =
    printfn "invoking func on input: %A" input
    let result = func input
    printfn "result: %A" result
    result

genericLogger add1 3
genericLogger times2 7

[1..7] |> List.map (genericLogger add1)
// or
let add1Logged = genericLogger add1
[1..7] |> List.map add1Logged

// generic timer decorator
let genericTimer func input =
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    let result = func input
    stopwatch.Stop()
    printfn "function took: %i ticks.  Input: %A" stopwatch.ElapsedTicks input
    result

genericTimer add1 5


// == Strategy Pattern == pg 181
type Animal(noiseStrategy) =
    member __.MakeNoise =
        noiseStrategy() |> printfn "Making noise %s"

let meow() = "Meow!"
let cat = Animal(meow)
cat.MakeNoise
let bark() = if DateTime.Now.Second % 2 = 0 then "Bark!" else "Woof!"
let dog = Animal(bark)
dog.MakeNoise