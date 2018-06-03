
(*
    Functional Reactive Programming - p 244
*)

open System
open System.Threading

// simple event stream
let createTimer interval handler =
    let timer = new System.Timers.Timer(float interval)
    timer.AutoReset <- true
    timer.Elapsed.Add handler

    // async task that runs the timer for 5 sec
    async {
        timer.Start()
        Thread.Sleep(3000)
        timer.Stop()
    }

let basicHandler _ = printfn "tick %A" DateTime.Now
createTimer 1000 basicHandler |> Async.RunSynchronously

let createObservableTimer interval =
    let timer = new System.Timers.Timer(float interval)
    timer.AutoReset <- true

    let observable = timer.Elapsed
    let task = async {
        timer.Start ()
        Thread.Sleep 3000
        timer.Stop ()
    }
    (task, observable)

let obsTimer, eventStream = createObservableTimer 1000
eventStream
|> Observable.subscribe basicHandler

obsTimer |> Async.RunSynchronously


// == counting events - p  ==

// imperitive
type ImperativeTimerCounter() =
    let mutable count = 0
    member __.HandleTick _ =
        count <- count + 1
        printfn "timer ticked with count: %i" count
let imperativeHandler = new ImperativeTimerCounter()
createTimer 500 imperativeHandler.HandleTick |> Async.RunSynchronously

// obs
let obsTimer2, eventStream2 = createObservableTimer 500
eventStream2
// scan is like fold.  Takes ('State, 'EventArgs) and initial 'State
|> Observable.scan (fun countState _ -> countState + 1) 0
|> Observable.subscribe (fun count -> printfn "[obs] timer tick with count: %i" count)

obsTimer2 |> Async.RunSynchronously


// == merging multiple event streams p 248 ==

(* 
    Fizz Buzz Events
    a) for all events, print the id of the time and the time
    b) when a tick is simultaneous with a previous tick, print 'FizzBuzz'
    otherwise:
    c) when the '3' timer ticks on its own, print 'Fizz'
    d) when the '5' timer ticks on its own, print 'Buzz'
*)

type FizzBuzzEvent = { id: int; time: DateTime }
let areSimulataneous (earlierEvent, laterEvent) =
    let {time=t1} = earlierEvent
    let {time=t2} = laterEvent
    t2.Subtract(t1) <= TimeSpan.FromMilliseconds(50.0)

// imperative
type ImperativeFizzBuzzHandler() =
    let mutable prevEvent = None
    let printFizzBuzz (prevEvent:FizzBuzzEvent option) currEvent =
        // let {id=id1;time=ts1} = prevEvent
        // let {id=id2;time=ts2} = currEvent

        printf 
            "EVENT: %i, %i.%03i" 
            currEvent.id currEvent.time.Second currEvent.time.Millisecond

        let simulatanious =
            prevEvent.IsSome 
            && areSimulataneous (prevEvent.Value, currEvent)

        if simulatanious then printf " FIZZBUZZ!"
        elif currEvent.id = 3 then printf " FIZZ"
        elif currEvent.id = 5 then printf " BUZZ"

        printf "\n"

    let handler id =
        let e = {id=id; time=DateTime.Now}
        printFizzBuzz prevEvent e 
        prevEvent <- Some e

    member __.Handle3 _ = handler 3
    member __.Handle5 _ = handler 5
        
let impHandler = new ImperativeFizzBuzzHandler()
let timer3 = createTimer 300 impHandler.Handle3
let timer5 = createTimer 500 impHandler.Handle5
[timer3; timer5]
|> Async.Parallel
|> Async.RunSynchronously

let obsTimer3, timerStream3 = createObservableTimer 300
let obsTimer5, timerStream5 = createObservableTimer 500
let stream3 = timerStream3 |> Observable.map (fun _ -> {id=3;time=DateTime.Now})
let stream5 = timerStream5 |> Observable.map (fun _ -> {id=5;time=DateTime.Now})

let combinedStream = stream3 |> Observable.merge stream5
let pairwiseStream = combinedStream |> Observable.pairwise
let simultaneous, nonSimultaneous =
    pairwiseStream |> Observable.partition areSimulataneous
let fizzStream, buzzStream =
    nonSimultaneous 
    |> Observable.map (fun (_, curr) -> curr)
    |> Observable.partition (fun {id=id} -> id = 3)

combinedStream 
|> Observable.subscribe (fun {id=id;time=t} -> 
    printf "OBS-EVENT: %i, %i.%03i" id t.Second t.Millisecond)
simultaneous
|> Observable.subscribe (fun e -> printfn " FIZZBUZZ!")
fizzStream
|> Observable.subscribe (fun _ -> printfn " fizz")
buzzStream
|> Observable.subscribe (fun _ -> printfn " buzz")

// debug code
// simultaneous |> Observable.subscribe (fun e -> printfn "sim %A" e)
// nonSimultaneous |> Observable.subscribe (fun e -> printfn "non-sim %A" e)

[obsTimer3; obsTimer5]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

// exploring pairwise tuple ordering.  It is (prev, curr)
let t, s =createObservableTimer 500
s 
|> Observable.map (fun _ -> DateTime.Now)
|> Observable.pairwise
|> Observable.subscribe (fun (ts1, ts2) -> 
    printfn 
        "ts1: %i.%03i, ts2: %i.%03i"
        ts1.Second ts1.Millisecond
        ts2.Second ts2.Millisecond)
t |> Async.RunSynchronously