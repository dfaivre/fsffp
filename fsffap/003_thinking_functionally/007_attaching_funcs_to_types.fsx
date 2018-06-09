
(*
    Attaching functions to types - p 345
*)

module Person =
    type T = { Name: string } with
        member this.FullName = this.Name

    let create name = {Name=name}

    // add member later...
    type T with
        member this.SortableName = this.Name
let p1  = Person.create "Bobby"
p1.FullName
p1.SortableName

// optional extensions (C# extension methods)
module PersonExtensions =
    type Person.T with
        member this.UppercaseName = this.Name.ToUpper ()

let p2 = Person.create "judy"
// p2.UppercaseName // error, not in scope

open PersonExtensions
p2.UppercaseName


// extending system types
open System

// error
// type int with
//     member this.IsEven = this % 2 = 0
type Int32 with
    member this.IsEven = this % 2 = 0
let i = 3    
i.IsEven


// static members
type Person.T with 
    static member Create name = Person.create name

Person.T.Create "billy-joe"

type Int32 with
    static member IsOdd x = x % 2 = 1
Int32.IsOdd 5


// attaching existing stand alone funcs
// ie: List.length and [1..3].length
module Fruit =
    type T = { Region:string }

    let create region = {Region = region}
    let fullName f = f.Region

    // attaching existing funcs with multiple params
    let hasSameRegion otherRegion (f:T) = f.Region = otherRegion
    
    type T with
        member this.FullName = this |> fullName
        member this.HasSameRegion otherRegion = this |> hasSameRegion otherRegion

let f = Fruit.create "rainforest"
Fruit.fullName f
f.FullName
f.HasSameRegion "artic"
f |> Fruit.hasSameRegion "rainforest"


// Tuple and optional params
type Product = { SKU:string; Price:float} with
    member this.CurriedTotal qty discount =
        (this.Price * float qty) - discount
    member this.TupleTotal (qty, discount) =
        (this.Price * float qty) - discount
    member this.TupleTotalOptDiscount(qty, ?discount) =
        let price = this.Price * float qty
        match discount with
        | Some discount -> price - discount
        | _ -> price
    member this.TupleTotal3(qty, ?discount) =
        let price = this.Price * float qty
        let discount = defaultArg discount 0.0
        price - discount

    // overloading, only on member functions with tuple param
    member this.TupleTotal4(qty, discount) =
        (this.Price * float qty) - discount
    member this.TupleTotal4(qty) = this.TupleTotal4(qty, 0.0)

let product = {SKU = "foo-bar-001"; Price = 2.0}
product.CurriedTotal 3 1.0
product.TupleTotal(3, 1.0)
// curried allows partial app
let totalFor10 = product.CurriedTotal 10
totalFor10 5.0

// but tuples allow for: named params, optional params, overloading
product.TupleTotal(discount=3.0, qty=5)
product.TupleTotalOptDiscount(qty=7)
product.TupleTotal4(1)


// don't overuse methods...