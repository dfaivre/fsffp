
(*
    Attaching functions to types - p 345
*)

// don't overuse methods... - p 355
module Person =
    type T = { FirstName:string; LastName:string}

    let create first last = {FirstName=first; LastName=last}    

    let fullName (p:T) = p.FirstName + " " + p.LastName

    type T with
        member this.FullName = this |> fullName

// open Person

// can infer Person.T type
let printFullName person =
    printfn "Name is %s" (person |> Person.fullName)

// // can't infer!
// let printFullName' person =
//     printfn "Name (from instance member) is: %s" person.FullName
let printFullName' (person:Person.T) =
    printfn "Name (from instance member) is: %s" person.FullName


// higher order funcs
let list = [
    Person.create "Billy" "Bob"
]

list |> List.map Person.fullName
// object methods need a full lambda
list |> List.map (fun p -> p.FullName)