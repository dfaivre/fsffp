// single line
(* 
    multi line (* embedded? *)
    -end multi line -
*)

let myInt = 5
let myFloat = 3.14
let myStr = "hello world"

let twoToFive = [2;3;4;5]
let oneToFive = 1 :: twoToFive
let zeroToFive = [0;1] @ twoToFive

// === Functions ===
let square x = x * x
square 2

let add x y = x + y
add 2 3

let filterEvens list =
    let isEven x = x%2 = 0

    list
    |> List.filter isEven

[0;1;2] |> filterEvens

let sumOfSquares100 =
    List.sum (List.map square [1..100])
sumOfSquares100

let sumOfSquares100Piped =
    [1..100] |> List.map square |> List.sum
sumOfSquares100Piped

let sumOfSquares100PipedLambda =
    [1..100] |> List.map (fun x -> x*x) |> List.sum
sumOfSquares100PipedLambda

let sumOfSquares100SumByLambda =
    [1..100] |> List.sumBy (fun x -> x*x)
sumOfSquares100SumByLambda

let sumOfSquares n =
    [1..n] |> List.sumBy square
sumOfSquares 1
sumOfSquares 2
sumOfSquares 3

let matchAB x =
    match x with
    | "a" -> printfn "matched 'a'"
    | "b" -> printfn "matched 'b'"
    | _ -> printfn "could not match %A" x

let validVal = Some(99)
let invalidVal = None

let optionalMatcher x =
    match x with
    | Some i -> printfn "found Some: %A" i
    | None -> printfn "none! %A" x

// complex data types
let tupleA = 1,2
let tuple3 = 1,"a",true
printfn "tupleA: %A" tupleA
printfn "tuple3: %A" tuple3

// record types
type Person = {
    FirstName:string
    LastName:string }

let person1 = {   
    FirstName = "foo"
    LastName = "bar" }

printfn "person1: %A" person1
printfn "person with baz: %A" { person1 with LastName = "baz" }

type Temp =
    | DegreesC of float
    | DegreesF of float

let degreeC = DegreesC 10.0
let degreeF = DegreesF 11.0
printfn "degree C: %A, degree F: %A" degreeC degreeF

// printing
printfn "int %i, float %.1f, bool %b, string %s" 1 2.0 true "hello!"

// tuples vs params pg 28
let addTuple (a, b) =
    a + b

let listTuple = [1,2,3,4]
printfn "list1: %A" listTuple
let listInts = [1;2;3;4]
printfn "list1: %A" listInts

// function pointers
let reader = new System.IO.StringReader("hello")
let nextLineFn = reader.ReadLine

let return1() = 1

printfn "next line: %s, return 1: %i" (nextLineFn()) (return1())