open System
open System.Net
open System.IO
open System
open System.Web.UI.WebControls

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


