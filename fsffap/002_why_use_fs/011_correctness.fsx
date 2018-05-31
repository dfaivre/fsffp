(*
    Correctness - ~ pg 188
*)

type PersonName = { FirstName:string; LastName:string;}

let john = {FirstName="John"; LastName="Doe"}

let alice = {john with FirstName="Alice"}

// complex data structures are shared
let list1 = [1;2;3;4]
let list2 = 0::list1
let list3 = list2.Tail
// check ref equality
System.Object.ReferenceEquals(list1, list3)

// exhaustive pattern matching
type State = New | Draft | Published | Inactive | Discontinued
// compiler will complain (warn?)
let handleState state = 
    match state with
    New -> ()

// avoid nulls with option type
let getFileInfo filePath =
    let fi = new System.IO.FileInfo(filePath)
    if fi.Exists then Some fi else None
let goodFileName = getFileInfo "Cart.fs"
let badFilename = getFileInfo "foobar.txt"

let logFileInfo (fi : System.IO.FileInfo option) =
    match fi with
        | Some fileInfo -> 
            printfn "file exists %s" fileInfo.FullName
        | None -> printfn "the file does not exist"

logFileInfo goodFileName
logFileInfo badFilename

// exhaustive pattern matching for edge cases
let rec pairAverages list =
    match list with
    | [] | [_] -> []
    // bug on purpose...
    | x::y::rest ->
        let avg = (x + y) / 2.0
        avg :: pairAverages (y::rest)

// == exhaustive patter matching as an error handling technique - p 197 ==
type Result<'a, 'b> =
    | Success of 'a
    | Failure of 'b
    // | Indeterminate

type FileErrorReason =
    | FileNotFound of string
    | UnauthorizedAccess of string * System.Exception

let performActionOnFile action filePath =
    try
        use sr = new System.IO.StreamReader(filePath:string)
        let result = action sr
        sr.Close()
        Success(result)
    // catch some exceptions and convert to errors (remember :? -> type pattern match)
    with
        | :? System.IO.FileNotFoundException ->
            Failure (FileNotFound filePath)
        | :? System.Security.SecurityException as e ->
            Failure (UnauthorizedAccess (filePath, e))

// middle layer, pass through result.
let middleLayerDo action filePath =
    let result = performActionOnFile action filePath
    // do some stuff, then retrun
    result

// top layer, actually use the result
let readFirstLine filePath =
    let result = middleLayerDo (fun sr -> sr.ReadLine()) filePath
    // do some stuff
    match result with
    | Success line -> printfn "read line from path (%s): %s" filePath line
    | Failure e ->
        match e with
        | UnauthorizedAccess (path, ex) -> 
            printfn "unauthed access: %s. %A" path ex
        | FileNotFound path -> 
            printfn "file not found handling: %s" path
            
readFirstLine "Cart.fs"
readFirstLine "bad-file-name.foo"

// == Exhaustive pattern matching as a change management tool p 200 ==

// change Result type above to add Indeterminate, or remove a choice, compiler will know.