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

(* 
    Four Key Concepts 
*)
let squareConcept x = x * x
let squareConceptClone = squareConcept
squareConceptClone 4
[1..10] |> List.map squareConceptClone
let funcInvoker func param = func param
funcInvoker squareConcept 4

type IntAndBoolTuple = int * bool
type IntAndBool = {
    intPart:int
    boolPart:bool
    tupleTypePart: IntAndBoolTuple
}
let intAndBool1 = {
    intPart = 7
    boolPart = true
    tupleTypePart = 2, false
}
printfn "intAndBool1: %A" intAndBool1

type IntOrBool =
    | IntChoice of int
    | BoolChoice of bool
let intChoice1 = IntChoice 2
let boolChoice1 = BoolChoice true
printfn "intChoice: %A, boolChoice: %A" intChoice1 boolChoice1

// partern matching with union types (pg 148)
type Shape =
| Circle of int
| Square of int
| Rectangle of int * int
| Polygon of (int * int) list
| Point of int * int

let draw shape =
    match shape with
    | Circle radius -> 
        printfn "circle with radius: %A" radius
    | Square side ->
        printfn "sqaure with side: %A" side
    | Rectangle (len, width) ->
        printfn "rectange (l: %A, w: %A)" len width
    | Polygon points ->
        printfn "polygon with points: %A" points
    | Point (x, y) ->
        printfn "point with coord: (%A, %A)" x y
let circle = Circle(10)
let square = Square(4)
let rect = Rectangle(7,8)
let polygon = Polygon([(1,1);(2,2);(3,3)])
let point = Point(9,9)
[circle;square;rect;polygon;point] |> List.iter draw


(*
    Conciseness (pg 149)
*)

// type inference / domain driven design
let sumLengths strList =
    // strList |> List.map String.length |> List.sum
    // or
    strList |> List.sumBy String.length
sumLengths ["foo"; "bar"]

// low overhead type defs
type Person = {FirstName:string; LastName:string; DateOfBirth:DateTime}
type Coord = {Lat:float; Lon:float}
type TimePeriod = Hour | Day | Month | Year
type AppointmentType =
    | OneTime of DateTime
    | Recurring of DateTime list

// ddd (simple)
type PersonName = {FirstName:string; LastName:string}
type StreetAddress = {Line1:string; Line2:string option; Line3: string option}
type ZipCode = ZipCode of string
type StateAbbrev = StateAbbrev of string
type ZipAndState = {Zip:ZipCode; State:StateAbbrev}
type UsAddress = {Street:StreetAddress; Region:ZipAndState}

type UkPostCode = PostCode of string
type UkAddress = { Street:StreetAddress; Region:UkPostCode}

type InternationalAddress = {
    Street:StreetAddress; Region:string; CountryName: string
}

type Address = US of UsAddress | UK of UkAddress | International of InternationalAddress

type Email = Email of string

type CountryPrefix = Prefix of int
type Phone = { Prefix:CountryPrefix; LocalNumber:string}

type Contact = {
    Name:PersonName
    Address:Address option;
    Email:Email option;
    Phone:Phone option;
}

type CustomerAccountId = AccountId of int
type CustomerType = Prospect | Active | Inactive

[<CustomEquality; NoComparison>]
type CustomerAccount =
    {
        CustomerAccountId: CustomerAccountId
        CustomerType: CustomerType
        ContactInfo: Contact
    }

    override this.Equals(other) =
        match other with
        | :? CustomerAccount as ca ->
            this.CustomerAccountId = ca.CustomerAccountId
        | _ -> false

    override this.GetHashCode() = hash this.CustomerAccountId

let customerAddr: UsAddress = {
    Street = { Line1 = "111 Baz Dr"; Line2 = None; Line3 = None}
    Region = { Zip = ZipCode("73107"); State = StateAbbrev("OK")}
}
let customer = {
    CustomerAccountId = CustomerAccountId.AccountId(1)
    CustomerType = CustomerType.Prospect
    ContactInfo = 
    {
        Name = { FirstName = "foo"; LastName = "bar"}
        Email = Some(Email "foo@bar.com")
        Address = Some(Address.US customerAddr)
        Phone = None            
    }
}
customer

type FooType = { val1:int }
type BarType = { val1:bool }
type Baz = FooType of FooType | BarType of BarType
type Baz2 = FooType of int | BarType of bool
let baz2 = Baz2.FooType 1
let baz = Baz.FooType {val1 = 1}

(*
    Boilerplate pg 156
*)
let productTo n =
    [1..n] |> List.reduce (*)
    // or
    // [1..n] |> List.reduce (fun acc next -> acc * next)
productTo 3

let sumOfOddsTo n =
    let isOdd x = x % 2 = 1
    let sumIfOdd sum x =
        match isOdd x with
        | true -> sum + x
        | false -> sum

    [1..n] 
    // |> List.filter (fun i -> i % 2 = 1)
    // |> List.sum
    // or aggr
    |> List.reduce sumIfOdd
sumOfOddsTo 5

let sumAlternatingNegPosTo n =
    let sumAlternating (isNeg, sum) x = 
        match isNeg with
        | true -> (false, sum - x)
        | false -> (true, sum + x)

    [1..n]
    |> List.fold sumAlternating (true, 0)
    |> fun (_, result) -> result

// max elem by prop
type NameAndSize = { Name:string; Size:int}

let maxNameAndSizeFold list =
    let maxNameSizeInternal x y =
        if x.Size >= y.Size then x else y
    
    match list with
    | [] -> None
    | y::ys -> Some(ys |> List.fold maxNameSizeInternal y)
let nameAndSizes = [ 
    {Name="foo";Size=1};
    {Name="bar";Size=2}]
maxNameAndSizeFold nameAndSizes
maxNameAndSizeFold []

let maxNameAndSize list =
    match list with
    | [] -> None
    | _ -> Some(list |> List.maxBy (fun x -> x.Size))
maxNameAndSize nameAndSizes
maxNameAndSize []

(* 
    building blocks (pg 164)
*)
let add2 x = x + 2
let mult3 x = x * 3
let power2 x = x * x
let add2Mult3 = add2 >> mult3
add2Mult3 2
let mult3Power2 = mult3 >> power2
mult3Power2 3
add2 2 |> mult3
mult3 3 |> power2
// extend existing funcs 165
let logMsg msg x = 
    printf "%s%i" msg x
    x
let logMsgN msg x = 
    printfn "%s%i" msg x
    x
let mult3Power2Logged =
    logMsg "before="
    >> mult3
    >> logMsg ";after mult3="
    >> power2
    >> logMsgN ";result="
mult3Power2Logged 3
// OR
let mathPipeComposition = 
    [
        logMsg "before=";
        mult3;
        logMsg ";after mult3=";
        power2;
        logMsgN ";result="
    ] |> List.reduce ( >> )
mathPipeComposition 3

// mini languages (pg 166)
// -- date calculator
type DateScale = Hour | Hours | Day | Days | Week | Weeks
type DateDirection = Ago | Hence
let getDate interval scale direction =
    let absHours = 
        match scale with
        | Hour | Hours -> interval
        | Day | Days -> interval * 24
        | Week | Weeks -> interval * 24 * 7

    let signedHours =
        match direction with
        | Ago -> absHours * -1
        | Hence -> absHours
    
    DateTime.Now.AddHours(float signedHours)
getDate 2 Hours Ago
getDate 3 Days Hence

// -- shape configuration fluent config
type FluentShape = {
    label:string;
    color:string;
    onClick: FluentShape -> FluentShape
}
let defaultShape = 
    {label=""; color=""; onClick=fun shape -> shape}
let click shape =
    shape.onClick shape
let display shape =
    printfn "shape: %s, color: %s" shape.label shape.color
    shape

let setLabel label shape =
    { shape with label=label }
let setColor color shape =
    { shape with color=color }
let appendClickAction action shape =
    { shape with onClick = shape.onClick >> action}

let setRedBox = setColor "red" >> setLabel "box"
let setBlueBox = setColor "blue" >> setLabel "box"
let changeColorOnClick color = appendClickAction (setColor color)

let redBox = defaultShape |> setRedBox
let blueBox = defaultShape |> setBlueBox

redBox 
    |> display
    |> changeColorOnClick "green"
    |> click
    |> display

blueBox
    |> display
    |> appendClickAction (setLabel "circle" >> setColor "brown")
    |> click
    |> display

let rainbowColors = ["red"; "orage"; "yellow"; "green"; "blue"; "purple"]
let showRainbow =
    let setColorAndDisplay color = setColor color >> display

    rainbowColors
    |> List.map setColorAndDisplay
    |> List.reduce ( >> )

{ defaultShape with label = "square" } |> showRainbow


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


