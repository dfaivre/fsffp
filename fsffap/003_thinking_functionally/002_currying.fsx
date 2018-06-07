
(*
    Currying - p 294
*)

open System.IO


// example 1
let printTwoParams x y =
    printfn "x=%i, y=%i" x y
// int -> int -> unit 
//  ... equals
let printTwoParams' x =
    let g y =
        printfn "x=%i, y=%i" x y
    
    g
// techically, fsi shows int -> (int -> unit)

printTwoParams 1
let x = 6
let y = 10
let intermediateFn = printTwoParams x

intermediateFn y
(printTwoParams x) y
printTwoParams x y

// plus (+) is just a func >> (+) x y = x + y
let intermediateAdd = (+) x
intermediateAdd y
(+) x y
x + y


// incomplete params
let printHello () = printfn "hello"
printHello // returns the func (unit -> unit)
printHello () // prints

let addXY x y =
    // printfn "x: %i, y: %i" x // compiler warning...
    x + y

let reader = new StringReader("hello")
reader.ReadLine // returns a func
reader.ReadLine() // returns string


// too many params
// printfn "hello" 42
// printfn "hello: %i" 2 1

let add1 x = x + 1
// let x = add1 2 3 // error FS0003: This value is not a function and cannot be applied.
// let add1To3 = add1 3
// add1To3 3 // same... error FS0003: This value is not a function and cannot be applied.

