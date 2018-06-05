
(*
    Anything c# can do... - p 262
*)

open System

// classes and interfaces
type IEnumerator<'a> =
    abstract member Current: 'a
    abstract MoveNext : unit -> 'a

[<AbstractClass>]
type Shape() =
    abstract member Width : int with get
    abstract member Height: int with get, set

    member this.BoundingArea = this.Height * this.Width

    // vitual member with base impl
    abstract member Print: unit -> unit
    default __.Print () = printfn "I am a shape"

type Rectagle(x:int, y:int) =
    inherit Shape()

    let mutable _y = y
    override __.Width = x
    override __.Height with get() = y and set(value) = _y <- value
    override this.Print () = printfn "I am a rectange! (%i, %i)" this.Width this.Height

let r = Rectagle(2, 3)
r.Print()

// multiple constructors
type Circle(radius:int) =
    inherit Shape()

    let mutable _radius = radius

    new() = Circle(10)

    override __.Width = _radius * 2
    override __.Height 
        with get() = _radius  * 2
        and set(value) = _radius <- value / 2
    override __.Print () = printfn "I am a circle (radius: %i)" _radius
    member __.Radius
        with get() = _radius
        and set(v) = _radius <- v

let c1 = Circle()
let c2 = Circle(2)
c1.Print ()
c2.Print ()

c2.Radius <- 5
c2.Print ()


// Generics
type KeyValuePair<'a, 'b>(key:'a, value: 'b) =
    member __.Key = key
    member __.Value = value
let kvp = KeyValuePair(1, "hello")
kvp

type Container<'a, 'b
    when 'a : equality
    and 'b :> System.Collections.IList>
    (name:'a, values:'b) =

    member __.Name = name
    member __.Values = values
let container = Container(123.3, System.Collections.Generic.List<string>())
container

// Structs
type Point2D =
    struct
        val X:float
        val Y:float
        new (x: float, y: float) = 
            {X = x; Y = y}
    end

let p1 = Point2D()
p1
let p2 = Point2D(1.0,2.0)
p2


// exceptions
exception MyError of string

try
    let e = MyError "Testing"
    raise e
with
    | MyError msg -> printfn "MyError!: %s" msg
    | _ -> printfn "some other exceptions"

type System.String with
    static member FooBar () = "foo_bar"
    member this.StartsWithA = this.StartsWith "A"

String.FooBar ()
"A string".StartsWithA
"B string".StartsWithA


// param arrays
type MyConsole() =
    member __.WriteLine([<ParamArray>] args: Object[]) =
        args
        |> Array.iter (fun arg -> printfn "arg: %A" arg)

let console = MyConsole()
console.WriteLine("hello", 1, 1.2, true)


// events
type MyButton() =
    let clickEvent = Event<_>()

    [<CLIEvent>]
    member __.OnClick = clickEvent.Publish

    member this.TestEvent arg =
        clickEvent.Trigger(this, arg)

let btn = MyButton()
btn.OnClick.Add(fun (sender, arg) ->
    printfn "clicked with arg: %O" arg)
btn.TestEvent("hello world!")


// delegates
type MyDelegate = delegate of int -> int
let f = MyDelegate (fun x -> x * x)
f.Invoke(5)
// cant do:
// f 5


// enums
type Color = | Red=1 | Green=2 | Blue=3
let color1 = Color.Red
// can't do: let color1 = Red
let color2:Color = enum 2
// :?> is "downcast"
let color3 = System.Enum.Parse(typeof<Color>, "Green") :?> Color

// flags
[<System.Flags>]
type FileAccess = | Read=1 | Write=2 | Execute=4
let fileAccess = FileAccess.Read ||| FileAccess.Execute
