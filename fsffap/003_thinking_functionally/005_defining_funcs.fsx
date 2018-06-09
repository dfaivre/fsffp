
(*
    Defining Functions - p 315
*)

// lambdas
let add = fun x y -> x + y
let add' x y = x + y
let addGenerator x = (+) x
let addGenerator' x = fun y -> x + y
let addGenerator'' =
    fun x ->
        fun y -> x + y


// == pattern matching on params ==
type Name = { first:string; last:string}
let bob = {first="bob"; last="smith"}
let f1 name = 
    let {first=f; last=l} = name
    printfn "first=%s, last=%s" f l
let f2 {first=f; last=l} =
    printfn "decomp::: first=%s, last=%s" f l
let f3 (x::xs) = printfn "first element is: %A" x // warning...


// == tuples vs params ==
let addTwoParams x y = x + y
addTwoParams 1 2
// addTwoParams(1,2) // error, trying to pass tuple to x


// == .net libs and tuples ==
System.String.Compare("a","b")
// System.String.Compare "a" "b" // error
let input1 = ("a", "b")
// System.String.Compare(input1) // error

// partial apply .net functions
let strCompare x y = System.String.Compare(x, y)
let strCompareWithB = strCompare "b"
["a";"b";"c"] |> List.map strCompareWithB


// == to group or not to group params ==

// numbers are independent, so no group
let add'' x y = x + y
// coordinates need both
let locateOnMap (xCoord,yCoord) = ()
// records
let setCustomerName name = { first=name.first; last=name.last}
// Name and Credentials would be independent
let setCustomerNameAuthed myCredentials aName = ()


// operators
let (.*%) x y = x + y + 1
1 .*% 2
// '*' needs a space, or else is comment
let ( *.* ) x y = x * y * 3
3 *.* 2

// prefixed with ! or ~
let (~%%) (s:string) = s.ToCharArray ()
%% "fooBar"


// == point free ==
let addExpl x y = x + y // explicit
let addPF x = (+) x // left of the last param...
//  which in turn returns: y -> x + (y)


// combinators
// funcs that only combine inputs, no outside func or global ops
// like >>, <<, |>, <| etc

// y-combinator!
let rec Y f x = f (Y f) x
