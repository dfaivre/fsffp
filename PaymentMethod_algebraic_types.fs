namespace Foo

module PayementMethodsAlgebraicTypes =
    type ChequeNumber = ChequeNumber of int

    type CardType =
        | Visa
        | MasterCard
    type CardNumber = CardNumber of int

    type PayementMethod =
        | Cash
        | Cheque of ChequeNumber
        | Card of CardType * CardNumber

    let printPayement method =
        match method with
        | Cash -> 
            printfn "Paid in cash"    
        | Cheque chequeNo -> 
            printfn "Paid by cheque: %A" chequeNo
        | Card (cardType, number) -> printfn "Paid by card: %A, %A" cardType number 