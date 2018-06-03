
(*
    Messages and Agents - p 234
*)

open System
open System.Threading
open System.Diagnostics

let printerAgent = MailboxProcessor.Start(fun inbox ->
    let rec messageLoop () = async {
        // read
        let! msg = inbox.Receive()
        // process        
        printfn "message recieved: %s" msg
        // now wait for next
        return! messageLoop()
    }

    messageLoop()
)
printerAgent.Post "hello!"

// == shared state - p 236 ==

// lock based
type Utility() =
    static let rand = new Random()
    static member RandomSleep() =
        let ms = rand.Next(10,20)
        Thread.Sleep ms
type LockedCounter() =
    static let _lock = new Object()
    static let mutable count = 0
    static let mutable sum = 0
    static let updateState i =
        sum <- sum + i
        count <- count + i
        printfn "sum: %i, count: %i" sum count
        Utility.RandomSleep ()

    static member Add i =
        let stopwatch = new Stopwatch()
        stopwatch.Start()

        // same as c# lock
        lock _lock (fun () ->
            stopwatch.Stop()
            updateState i
            printfn "client waited %i ms" stopwatch.ElapsedMilliseconds
        )
LockedCounter.Add 4
LockedCounter.Add 5

let makeCountingTask addFunc taskId = async {
    let name = sprintf "Task_%i" taskId
    for i in [1..3] do
        addFunc i
}
let task = makeCountingTask LockedCounter.Add 1
task |> Async.RunSynchronously
// run it in parallel

[1..10]
    |> List.map (fun i -> makeCountingTask LockedCounter.Add i)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

// message based
type MessageBasedCounter() =
    static let updateState (count, sum) msg =
        let newSum = sum + msg
        let newCount = count + 1
        printfn "sum: %i, count: %i" newSum newCount
        Utility.RandomSleep()
        (newCount, newSum)

    static let agent = MailboxProcessor.Start(fun inbox ->
        let rec messageLoop state  = async {
            let! msg = inbox.Receive()
            let newState = updateState state msg
            return! messageLoop newState
        }

        messageLoop (0,0)
    )    

    static member Add = agent.Post

MessageBasedCounter.Add 2

[1..10]
    |> List.map (fun i -> makeCountingTask MessageBasedCounter.Add i)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore


// == Shared IO ==
let slowConsoleWrite msg =
    msg |> String.iter (fun c -> 
        // simulate slow...
        Thread.Sleep 1
        Console.Write c
    )
    Console.Write "\n"

slowConsoleWrite "abc"

let makeTask logger taskId = async {
    let name = sprintf "task_%i" taskId
    for i in [1..3] do
        let msg = sprintf "-%s:Loop:%i-" name i
        logger msg
}
makeTask slowConsoleWrite 1 |> Async.RunSynchronously

// non-thread safe logger
type UnserializedLogger() =
    member __.Log msg = slowConsoleWrite msg

// thread safe with messages
type SerializedLogger() =
    let agent = MailboxProcessor.Start(fun inbox ->
        let rec messageLoop() = async {
            let! msg = inbox.Receive()

            slowConsoleWrite msg
            return! messageLoop()
        }
        
        messageLoop()
    )

    member __.Log = agent.Post

// write in parallel
let runParallel logger =
    [1..5]
        |> List.map (fun i -> makeTask logger i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

let unserializedLogger = new UnserializedLogger()
unserializedLogger.Log "hello"
runParallel unserializedLogger.Log

let serializedLogger = new SerializedLogger()
serializedLogger.Log "hello"
runParallel serializedLogger.Log
