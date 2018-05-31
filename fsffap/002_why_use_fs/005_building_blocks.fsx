open System
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
