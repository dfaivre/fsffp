(*
    Concurrency - pg 222
*)

open System

let userTimerWithCallback () =
    let event = new System.Threading.AutoResetEvent(false)

    // timer to trigger event
    let timer = new System.Timers.Timer(2000.0)
    timer.Elapsed.Add (fun _ -> event.Set() |> ignore)

    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()

    printfn "Doing some work, waiting for timer"

    event.WaitOne() |> ignore

    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

let userTimerWithAsync () =
    let timer = new System.Timers.Timer(2000.0)
    let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore

    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()

    printfn "Doing some work, waiting for timer"

    Async.RunSynchronously timerEvent

    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

let fileWriteWithAsync () =
    use stream = new System.IO.FileStream("_temp.temp", System.IO.FileMode.Create)

    printfn "starting async write"
    let asyncResult = stream.BeginWrite (Array.empty, 0, 0, null, null)
    let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore

    printfn "doing some work while write completes"

    Async.RunSynchronously async
    printfn "async completed."

let sleepWorkflow = async {
    printfn "starting async sleep"
    do! Async.Sleep 2000
    printfn "sleep finished."
}
Async.RunSynchronously sleepWorkflow

let nestedWorkflow = async {
    printfn "starting parent"
    let! childWorkflow = Async.StartChild sleepWorkflow
    do! Async.Sleep 100
    printfn "doing something else while parent runs"
    do! childWorkflow

    printfn "finished!"
}
Async.RunSynchronously nestedWorkflow

// == cancellation - p 227 ==

let testLoop = async {
    for i in [1..100] do
        printf "%i boefore.." i
        do! Async.Sleep 100
        printfn "..after"
}
Async.RunSynchronously testLoop
// now with cancel
let cancellationSource = new System.Threading.CancellationTokenSource()
Async.Start (testLoop, cancellationSource.Token)
System.Threading.Thread.Sleep(1000)
cancellationSource.Cancel()

let sleepWorkflowMs ms = async {
    printfn "%i ms sleep workflow started" ms
    do! Async.Sleep ms
    printfn "%i ms sleep workflow finished" ms
}
let workflowInSeries = async {
    do! sleepWorkflowMs 500
    do! sleepWorkflowMs 1000
}
#time
Async.RunSynchronously workflowInSeries
#time

let sleep1 = sleepWorkflowMs 500
let sleep2 = sleepWorkflowMs 1000
#time
[sleep1;sleep2]
    |> Async.Parallel
    |> Async.RunSynchronously
#time


// == Example: an async web downloader ==
open System.Net
open System.IO

let fetchUrl url = 
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading: %s" url
    printfn "preview:\n%s" (html.Substring(0, 200))
fetchUrl "http://www.google.com"
// code to time it
let sites = [
    "http://www.bing.com";
    "http://www.google.com";
    "http://www.reddit.com";
    "http://www.yahoo.com"]
#time
sites |> List.iter fetchUrl
#time

let fetchUrlAsync url = async {
    let req = WebRequest.Create(Uri(url))
    use! resp = req.AsyncGetResponse()
    use stream = resp.GetResponseStream()
    use reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading: %s" url
    printfn "preview:\n%s" (html.Substring(0, 200))
}
#time
sites
|> List.map fetchUrlAsync
|> Async.Parallel
|> Async.RunSynchronously
#time


// == cpu chewer - p 233 ==
let childTask () =
    for _ in [1..2000] do
        for _ in [1..1000] do
            "Hello".Contains("H") |> ignore
#time
childTask()

let parentTask =
    childTask
    |> List.replicate 20
    |> List.reduce (>>)
parentTask()

let asyncChildTask = async { childTask() }
let asyncParentTask() =
    asyncChildTask
    |> List.replicate 20
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
asyncParentTask()