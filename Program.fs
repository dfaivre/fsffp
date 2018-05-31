open System.Text.RegularExpressions


// Learn more about F# at http://fsharp.org
// https://www.youtube.com/watch?v=Up7LcbGZFuo

type String50 = String50 of string
type String1 = String1 of string

type PersonName = {
    FirstName: String50
    MiddleInitial: String1 option
    LastName: String50
}

type EmailAddress = EmailAddress of string

let createEmailAddress s =
    if Regex.IsMatch(s, @"^\S+@\S+\.\S+$")
        then Some(EmailAddress s)
        else None

type VerifiedEmail = VerifiedEmail of EmailAddress
type VerifiedEmailHash = VerifiedEmailHash of string
type VerificationService = 
    (EmailAddress * VerifiedEmailHash) -> VerifiedEmail option

type EmailContactInfo = 
    | Unverified of EmailAddress
    | Verified of VerifiedEmail

type PostalContactInfo = PostalContactInfo of string

type ContactInfo =
    | Email of EmailContactInfo
    | Address of PostalContactInfo

type Contact = {
    Name: PersonName
    PrimaryContactInfo: ContactInfo
    SecondaryContactInfo: ContactInfo option
}

[<EntryPoint>]
let main _ =
    printfn "Hello World from F#!"
    0 // return an integer exit code
