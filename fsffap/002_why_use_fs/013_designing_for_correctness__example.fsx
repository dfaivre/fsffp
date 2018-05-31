(*
    Worked example: Designing for correctness - pg 207

    Shopping Cart Domain
    - You can only pay for a cart once.
    - Once a cart is paid for, you cannot change the items in it.
    - Empty carts cannot be paid for.
*)

// placeholder for more complicated type
type CartItem = CartItem of string


// cart states
type EmptyState = NoItems
type ActiveState = { UnpaidItems: CartItem list }
type PaidForState = { PaidItems: CartItem list; Payment: decimal}

// cart, which is basically a state bag
type Cart =
    | Empty of EmptyState
    | Active of ActiveState
    | PaidFor of PaidForState

// ops on cart states
let addToEmptyState item =
    Cart.Active { UnpaidItems = [item]}
let addToActiveState state item =
    Cart.Active {
        state with UnpaidItems = item :: state.UnpaidItems
    }
let removeFromActiveState state item =
    let newItems = state.UnpaidItems |> List.filter (fun i -> item <> i)
    match newItems with
    | [] -> Cart.Empty NoItems
    | _ -> Cart.Active { state with UnpaidItems = newItems }

let payForActiveState state payment =
    Cart.PaidFor {
        PaidItems = state.UnpaidItems
        Payment = payment
    }

// attach methods to state
type EmptyState with
    member __.Add = addToEmptyState
type ActiveState with
    member this.Add = addToActiveState this
    member this.Remove = removeFromActiveState this
    member this.Pay = payForActiveState this

// cart item methods
let addItemToCart cart item =
    match cart with
    | Empty s -> s.Add item
    | Active s -> s.Add item
    | PaidFor _ -> 
        printfn "ERROR: the cart is paid for"
        cart

let removeItemFromCart cart item =
    match cart with
    | Empty _ -> 
        printfn "ERROR: cart is empty"
        cart
    | Active s -> s.Remove item
    | PaidFor _ -> 
        printfn "ERROR: cart is paid for"
        cart

let displayCart cart =
    match cart with
    | Empty _ -> printfn "the cart is empty"
    | Active s -> 
        printfn "cart is active an has %i items: %A"
            (List.length s.UnpaidItems) 
            (s.UnpaidItems |> List.truncate 5)
    | PaidFor s ->
        printfn "cart is paid for.  Amount: %f.  Items: %A"
            s.Payment 
            (s.PaidItems |> List.truncate 5)

// add helpers to Cart
type Cart with
    static member NewCart = Cart.Empty NoItems
    member this.Add = addItemToCart this
    member this.Remove = removeItemFromCart this
    member this.Display = displayCart this

// == exercise the design ==
let emptyCart = Cart.NewCart
emptyCart.Display
let cartA = emptyCart.Add (CartItem "foo")
cartA.Display
let cartAB = cartA.Add (CartItem "bar")
cartAB.Display
let cartB = cartAB.Remove (CartItem "foo")
cartB.Display
let emptyCart2 = cartB.Remove (CartItem "bar")
emptyCart2.Display

emptyCart2.Remove (CartItem "bar")

// client handling of payment
let payCart cart =
    match cart with
        | Empty _ | PaidFor _ -> cart
        | Active state -> state.Pay 100m
let cartAPaid = cartA |> payCart
cartAPaid.Display

let emptyCartPaid = emptyCart |> payCart
emptyCartPaid.Display

let cartAPaidAgain = cartAPaid |> payCart
cartAPaidAgain.Display

// client can't force bad state. compile errors.
// let payCartBad cart =
//     match cart with
//     | Empty s -> s.Pay 100m
//     | PaidFor s -> s.Pay 100m
//     | Active s -> s.Pay 100m