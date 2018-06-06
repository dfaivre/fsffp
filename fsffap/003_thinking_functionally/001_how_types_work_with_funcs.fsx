
(*
    How types work with functions - p 283
*)

let intToString x = sprintf "x is %i" x
let stringToInt x = System.Int32.Parse x
let intToFloat x = float x
let intToBool x = (x = 1)
let stringToString x = x + " world!"

// doesn't work:
// let stringLength x = x.Length
let stringLength (x:string) = x.Length
// non-parens ':' is return type...:
let stringLengthAsInt (x:string) : int = x.Length


// function types as params (higher order functions/HOF) - p 284
let evalWith5ThenAdd2 fn = (fn 5) + 2
let add1 x = x + 1
evalWith5ThenAdd2 add1
let times3 x = x * 3
evalWith5ThenAdd2 times3

// funcs as output
let adderGenerator numberToAdd = (+) numberToAdd
let add2 = adderGenerator 2


// types to constrain funcs - p 287

// generic
let evalWith5 fn = fn 5
// constrained
let evalWith5AsString fn : string = fn 5


// unit
let whatIsThis = () // a unit!
// parameterless funcs
let printHelloConst = printf "hello world"
let printHelloFunc () = printf "hello!"
// ignore
// do 1+1 // warning
do 1+1 |> ignore

let something = 
    // 2+2 // warning
    2+2 |> ignore
    "hello"


// Generic Types - p 290
let onAStick x = x.ToString () + " on a stick!"
onAStick 1
onAStick 'c'
// multiple generics
let concatString x y = x.ToString () + y.ToString ()
// infer single generic type
let isEqual x y = (x=y)


// other
(1, "string") // tuple
[1..10] // int list
["hello", "world"] // string list
seq{1..10} // seq -- lazy evaled?
[|1..4|] //array
Some(1) // option
None
type Foo = | Bar | Baz
Some(Bar)


// test
let testA = float 2
let testB x = float 2
let testC x = float 2 + x
let testD x = x.ToString().Length
let testE (x:float) = x.ToString().Length
let testF x = printfn "%s" x
let testG x = printfn "%f" x
let testH = 2 * 2 |> ignore
let testI x = 2 * 2 |> ignore
let testJ (x:int) = 2 * 2 |> ignore
let testK = "hello"
let testL() = "hello"
let testM x = x=x
let testN x = x 1 // hint: what kind of thing is x?
let testO x:string = x 1 // hint: what does :string modify?