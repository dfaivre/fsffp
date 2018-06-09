
(*
    Organizing Functions - p 333
*)


// nested recursion
let sumNumbersUpTo max =
    let rec recursiveSum n sum =
        match n with
        | n when n = max -> sum + n
        | _ -> recursiveSum (n+1) (sum + n)
    recursiveSum 0 0
sumNumbersUpTo 7


// modules
module MathStuff =
    do 
        printfn "hello static constructor!"
        printfn "multi line static constructor"

    let add x y = x + y
    let subract x y = x - y

    type DegreesOrRadians = Deg | Rad

    // "constant"
    let PI = 3.141
    // "variable/field"
    let mutable Foo = Deg

    // shadowing
    [<RequireQualifiedAccess>]
    module FloatStuff =
        let add x y : float = x + y

// shodowing usuage
open MathStuff
// open MathStuff.FloatStuff // [<RequireQualifiedAccess>] makes this an error
// add 1 2 // error, expects float

module OtherStuff =
    let add1 = MathStuff.add 1

module FooStuff =
    open MathStuff

    let add2  = add 2




