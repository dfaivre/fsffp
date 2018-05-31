(*
    Active Patterns pg 186
*)

// int and bool parsing
let (|Int|_|) str =
    match System.Int32.TryParse(str) with
    | (true, int) -> Some(int)
    | _ -> None
let (|Bool|_|) str =
    match System.Boolean.TryParse(str) with
    | (true, b) -> Some(b)
    | _ -> None

let parseInt str =
    let parseResult = System.Int32.TryParse(str)
    match parseResult with
    | (true, int) -> Some(int)
    | _ -> None

let parseString str =
    match str with
    | Int i -> printfn "we parsed an int %i" i
    | Bool b -> printfn "we parsed a bool: %b" b
    | _ -> printfn "no parser type for: %s" str

parseString "1.304"
parseString "1"
parseInt "3"
parseString "true"

open System.Text.RegularExpressions

let (|FirstRegexGroup|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if (m.Success) then Some m.Groups.[1].Value else None

let regexTester str =
    match str with
    | FirstRegexGroup "http://(.*?)/(.*)" host ->
        printfn "the value is a url with host: %s" host
    | FirstRegexGroup ".*?@(.*)" host ->
        printfn "the value is an email with host: %s" host
    | _ -> 
        printfn "the value is not a url or an email: %s" str

regexTester "http://google.com/test"
regexTester "foo@bar.com"
regexTester "i am not a host"

// FizzBuzz https://blog.codinghorror.com/why-cant-programmers-program/
(* 
Write a program that prints the numbers from 1 to 100.
But for multiples of three print "Fizz" instead of the number 
and for the multiples of five print "Buzz". 
For numbers which are multiples of both three and five print "FizzBuzz".
*)

let (|MultOf3|_|) i =
    if i % 3 = 0 then Some(MultOf3) else None
let (|MultOf5|_|) i =
    if i % 5 = 0 then Some(MultOf5) else None

let fizzBuzz i =
    match i with
    | MultOf3 & MultOf5 -> printfn "FizzBuzz!: %i" i
    | MultOf3 -> printfn "Fizz!: %i" i
    | MultOf5 -> printfn "Buzz!: %i" i
    | _ -> printfn "%i" i

let fizzBuzzInline i =
    printf "%i " i
    match (i % 3 = 0, i % 5 = 0) with
    | (true, true) -> printfn "FizzBuzz!"
    | (true, false) -> printfn "Fizz!"
    | (false, true) -> printfn "Buzz!"
    | _ -> printfn ""

[1..100] |> List.iter fizzBuzz
[1..100] |> List.iter fizzBuzzInline