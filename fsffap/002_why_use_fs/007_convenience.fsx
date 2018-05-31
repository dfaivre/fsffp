(*
    Convenience pg 173
*)

// out of box behavior for types (pg 174)
// -- structural equality
type PersonName = {FirstName: string; LastName: string}
open System.Security.AccessControl
let bobName = {FirstName = "bob"; LastName = "bar"}
let alice1Name = {FirstName = "alice"; LastName = "foo"}
let alice2Name = {FirstName = "alice"; LastName = "foo"}
alice1Name = alice2Name
alice1Name = bobName

// -- comparison
type Suit = Club | Diamond | Spade | Heart
type Rank = Two | Three | Four | Five | Six
            | Seven | Eight | Nine | Ten
            | Jack | Queen | King | Ace
let compareCard card1 card2 =
    if card1 < card2
    then printfn "%A is less than %A" card1 card2
    else printfn "%A is greater than %A" card1 card2

let aceHearts = Ace, Heart
let twoDiamonds = Two, Diamond
let kingSpades = King, Spade
compareCard aceHearts twoDiamonds
compareCard twoDiamonds kingSpades
// note this is false! compares first,first, THEN second second if needed
(Heart, Two) < (Spade, Ace)
// instant sorting
let hand = [
    (Eight, Diamond); (Ace, Heart); (Jack, Spade)
]
List.sort hand |> printfn "sorted hand: %A"
// min max too!
List.min hand
List.max hand


