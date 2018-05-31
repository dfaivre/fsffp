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