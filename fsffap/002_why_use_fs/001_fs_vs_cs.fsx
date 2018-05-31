open System
open System.Net
open System.IO
open System
open System.Web.UI.WebControls

(* 
    F# vs C#
*)
// Quick Sort
let rec quickSortFoo list =
    match list with
    | x::xs -> 
        // bug here -- not handling duplicate elements... use <= for one of the groups
        let lessThanElements = xs |> List.filter (fun e -> e < x) |> quickSortFoo
        let greaterThanElements = xs |> List.filter (fun e -> e > x) |> quickSortFoo
        lessThanElements @ x :: greaterThanElements
    | []  -> list

let rec quickSortFoo2 list =
    match list with
    | first::rest -> 
        let smaller, larger = 
            rest 
                |> List.partition ((>=) first)
        // quickSortFoo2 smaller @ x :: quickSortFoo larger
        // or
        List.concat [quickSortFoo2 smaller; [first]; quickSortFoo2 larger]
    | []  -> list

// Download Web Page
let fetchUrl callback url =
    let req = WebRequest.CreateHttp(Uri(url))
    use resp = req.GetResponse()
    use content = resp.GetResponseStream()
    use reader = new IO.StreamReader(content)

    callback reader url

let printUrlContentAsString (reader:StreamReader) url =
    let content = reader.ReadToEnd()
    printfn "content: %A" url
    printfn "%A" (content.Substring(0, 200))

fetchUrl printUrlContentAsString "http://www.google.com"

let fetchUrlAndPrint = fetchUrl printUrlContentAsString
fetchUrlAndPrint "http://www.google.com"

let sites = [
    "http://www.google.com";
    "http://www.bing.com";
    "http://www.yahoo.com"
]
sites |> List.map fetchUrlAndPrint