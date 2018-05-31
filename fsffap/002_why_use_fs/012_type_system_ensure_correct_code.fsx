(*
    Using the type system to ensure correct code - 202
*)

// type of type safety
type EmailAddress = EmailAddress of string
let sendEmail (EmailAddress email) =
    printfn "sending email to address: %s" email
let aliceEmail = EmailAddress "alice@foo.bar"
sendEmail aliceEmail
// sending straight string won't compile
// sendEmail "bob@foo.bar"

// == units of measure ==
[<Measure>]
type cm

[<Measure>]
type inches

[<Measure>]
type feet =
    // add conversion
    static member ToInches(feet: float<feet>): float<inches> =
        feet * 12.0<inches/feet>

let meter = 100.0<cm>
let yard = 3.0<feet>
// convert yard to inches
let yearInInches = feet.ToInches(yard)
// can't mix...
// let invalid = yard + meter

// currencies
[<Measure>]
type GBP

[<Measure>]
type USD

let gbp10 = 10.0<GBP>
let usd10 = 10.0<USD>
let gbp20 = gbp10 + gbp10
// let invalid1 = gbp10 + usd10
// let invalid2 = gbp10 + 10.0
let wildCard = gbp10 + 10.0<_>

// == type safe equality ==
open System
let obj = new Object()
let ex = new Exception()
// works in c#, not f#
// let b = (obj = ex)

[<NoEquality; NoComparison>]
type CustomerAccount = {CustomerAccountId:int}
let customerX = {CustomerAccountId = 1}
// error!
// customerX = customerX
customerX.CustomerAccountId = customerX.CustomerAccountId