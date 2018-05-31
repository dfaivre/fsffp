namespace Foo


module Cart =
    type CartItem = string // placeholder
    type EmptyState = NoItems // this is a single union type, duh
                                // trying to force Client apps to handle empty state explicitly

    type ActiveState = {
        UnpaidItems : CartItem list
    }

    type PaidForState = {
        PaidItems : CartItem list;
        Payment : decimal
    }

    type Cart =
        | Empty of EmptyState
        | Active of ActiveState
        | PaidFor of PaidForState

    // actions
    let addToEmptyState item =
        Cart.Active { 
            UnpaidItems = [item]
        }

    let addToActiveState state item =
        let newItemList = item :: state.UnpaidItems
        Cart.Active {
            state with UnpaidItems = newItemList
        }    
    
    let removeFromActiveState state item =
        let newItemList = 
            state.UnpaidItems
            |> List.filter (fun i -> i <> item)

        match newItemList with
        | [] -> Cart.Empty NoItems
        | _ -> Cart.Active { state with UnpaidItems = newItemList }
    
    let payForActiveState state amount =
        Cart.PaidFor {PaidItems = state.UnpaidItems; Payment = amount}
    
    type EmptyState with 
        member __.Add = addToEmptyState

    type ActiveState with
        member this.Add = addToActiveState this
        member this.Remove = removeFromActiveState this
        member this.Pay = payForActiveState this
    

    let addItemToCart cart item =
        match cart with
        | Empty state -> state.Add item
        | Active state -> state.Add item
        | PaidFor _ -> 
            printfn "ERROR: the cart is paid for"
            cart

    let removeItemFromCart cart item =
        match cart with
        | Empty _ -> 
            printfn "ERROR: the cart is empty"    
            cart
        | Active state -> state.Remove item
        | PaidFor _ ->
            printfn "ERROR: the cart is paid for"
            cart

    let displayCart cart =
        match cart with 
        | Empty _ ->
            printfn "The cart is empty"
        | Active state ->
            printfn "The cart contains %A unpaid items" 
                state.UnpaidItems
        | PaidFor state ->
            printfn "The cart cotains %A paid items.  Amount paid: %f" 
                state.PaidItems state.Payment

    type Cart with
        static member NewCart = Cart.Empty NoItems
        member this.Add = addItemToCart this
        member this.Remove = removeItemFromCart this
        member this.Display = displayCart this