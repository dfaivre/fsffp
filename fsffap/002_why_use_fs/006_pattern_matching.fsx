

(*
    pattern matching for conciseness pg 171
*)


// matching tuples directly
let firstPart, secondPart, _ = (1,2,3)
// now for lists
let elem1::elem2::rest = [1..5]
// lists inside match
let listMatcher aList =
    match aList with
    | [] -> printfn "empty list"
    | [singleElem] -> printfn "single elem: %A" singleElem
    | [firstElem;secondElem] -> printfn "two elems: %A, %A" firstElem secondElem
    | _ -> printfn "the list has more than two elements (%i). First five: %A" 
                        (List.length aList) (aList |> List.take 5)
listMatcher [1]
listMatcher [2;3]
listMatcher []
listMatcher [5..13]

// pattern match record types
type Address = { Street: string; City: string; }
type Contact = { ID: int; Name: string; Address: Address}
let customer1 = { 
        ID = 1
        Name = "foo"
        Address = {Street = "100 Bar St"; City = "Baz"}}
let { Name=name1} = customer1
name1
// extract multiple props
let { Name=name2; ID=id2} = customer1
printfn "name: %s, id: %i" name2 id2
// extract name and street address
let { Name=name3; Address={Street=street3}} = customer1