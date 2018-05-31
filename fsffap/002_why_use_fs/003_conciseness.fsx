open System

(*
    Conciseness (pg 149)
*)

// type inference / domain driven design
let sumLengths strList =
    // strList |> List.map String.length |> List.sum
    // or
    strList |> List.sumBy String.length
sumLengths ["foo"; "bar"]

// low overhead type defs
type Person = {FirstName:string; LastName:string; DateOfBirth:DateTime}
type Coord = {Lat:float; Lon:float}
type TimePeriod = Hour | Day | Month | Year
type AppointmentType =
    | OneTime of DateTime
    | Recurring of DateTime list

// ddd (simple)
type PersonName = {FirstName:string; LastName:string}
type StreetAddress = {Line1:string; Line2:string option; Line3: string option}
type ZipCode = ZipCode of string
type StateAbbrev = StateAbbrev of string
type ZipAndState = {Zip:ZipCode; State:StateAbbrev}
type UsAddress = {Street:StreetAddress; Region:ZipAndState}

type UkPostCode = PostCode of string
type UkAddress = { Street:StreetAddress; Region:UkPostCode}

type InternationalAddress = {
    Street:StreetAddress; Region:string; CountryName: string
}

type Address = US of UsAddress | UK of UkAddress | International of InternationalAddress

type Email = Email of string

type CountryPrefix = Prefix of int
type Phone = { Prefix:CountryPrefix; LocalNumber:string}

type Contact = {
    Name:PersonName
    Address:Address option;
    Email:Email option;
    Phone:Phone option;
}

type CustomerAccountId = AccountId of int
type CustomerType = Prospect | Active | Inactive

[<CustomEquality; NoComparison>]
type CustomerAccount =
    {
        CustomerAccountId: CustomerAccountId
        CustomerType: CustomerType
        ContactInfo: Contact
    }

    override this.Equals(other) =
        match other with
        | :? CustomerAccount as ca ->
            this.CustomerAccountId = ca.CustomerAccountId
        | _ -> false

    override this.GetHashCode() = hash this.CustomerAccountId

let customerAddr: UsAddress = {
    Street = { Line1 = "111 Baz Dr"; Line2 = None; Line3 = None}
    Region = { Zip = ZipCode("73107"); State = StateAbbrev("OK")}
}
let customer = {
    CustomerAccountId = CustomerAccountId.AccountId(1)
    CustomerType = CustomerType.Prospect
    ContactInfo = 
    {
        Name = { FirstName = "foo"; LastName = "bar"}
        Email = Some(Email "foo@bar.com")
        Address = Some(Address.US customerAddr)
        Phone = None            
    }
}
customer

type FooType = { val1:int }
type BarType = { val1:bool }
type Baz = FooType of FooType | BarType of BarType
type Baz2 = FooType of int | BarType of bool
let baz2 = Baz2.FooType 1
let baz = Baz.FooType {val1 = 1}
