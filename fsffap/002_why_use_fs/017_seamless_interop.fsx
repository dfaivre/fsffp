(*
    Seamless interop with .net - p 255
*)

open System

let printTryResult (success, result) =
    if success then 
        printfn "TRY successful, result: %A" result 
    else 
        printfn "TRY not successful"

// out vars (TryParse, etc)
Int32.TryParse("123") |> printTryResult
Int32.TryParse("abc") |> printTryResult

let dict = dict [ ("a", "hello") ]
dict.TryGetValue "a" |> printTryResult
dict.TryGetValue "b" |> printTryResult


// named args

// err,
// let readerFactory1 path = new System.IO.StreamReader(path)
// no err
let readerFactory2 path = new System.IO.StreamReader (path=path)


// active patterns for .net functions
// System.Char.IsXXX matching
let (|Digit|Letter|Whitespace|Other|) ch =
    if System.Char.IsDigit ch then Digit
    elif System.Char.IsLetter ch then Letter
    elif System.Char.IsWhiteSpace ch then Whitespace
    else Other

let printChar ch =
    match ch with
    | Digit -> printfn "%c is digit" ch
    | Letter -> printfn "%c is letter" ch
    | Whitespace -> printfn "'%c' is whitespace" ch
    | Other -> printfn "%cc is other" ch
printChar '9'

// parse error codes, exceptions, etc
open System.Data.SqlClient
let (|ConstraintError|ForeignKeyError|Other|) (ex:SqlException) =
    if ex.Number = 2601 then ConstraintError
    elif ex.Number = 2627 then ForeignKeyError
    else Other
let execNonQuery (sqlCmd:SqlCommand) =
    try
        sqlCmd.ExecuteNonQuery() |> ignore
    with
    | :?SqlException as sqlEx ->
        match sqlEx with
        | ConstraintError -> printfn "contraint!"
        | ForeignKeyError -> printfn "FK error!"
        | _ -> reraise()


// object expressions -- create objects directly from interface

let makeResource name = 
    { 
        new System.IDisposable with
        member __.Dispose() = printfn "%s disposed" name
    }
let useAndDisposeResources() =
    use r1 = makeResource "r1"
    printfn "using r1"
    for i in [1..3] do
        let rName = sprintf "\tinner resource %d" i
        use temp = makeResource rName
        printfn "\tdoing something with %s..." rName
    use r2 = makeResource "r2"
    printfn "using r2"
    printfn "done"


// mixing interfaces with pure f# types
type IAnimal =
    abstract member MakeNoise : unit -> string
let showTheNoiseAnAnimalMakes (animal:IAnimal) =
    animal.MakeNoise() |> printfn "Making noise: %s"

type Cat = Felix | Socks
type Dog = Butch | Lassie

type Cat with
    member __.AsAnimal = 
        {
            new IAnimal with
            member __.MakeNoise() = "meow"
        }
type Dog with
    member __.AsAnimal =
        {
            new IAnimal with
            member __.MakeNoise() = "roof!"
        }
let dog = Lassie
let cat = Felix
showTheNoiseAnAnimalMakes dog.AsAnimal
showTheNoiseAnAnimalMakes cat.AsAnimal


// reflection
open System.Reflection
open Microsoft.FSharp.Reflection

type Account = {Id: int; Name: string}
let fields = 
    FSharpType.GetRecordFields typeof<Account>
    |> Array.map (fun propInfo -> propInfo.Name, propInfo.PropertyType.Name)
fields

type Choices = | A of int | B of string
let choices = 
    FSharpType.GetUnionCases typeof<Choices>
    |> Array.map (fun info -> info.Name)
choices