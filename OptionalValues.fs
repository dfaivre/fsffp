// namespace Foo

module OptionalValues

type Person = {
    FirstName: string
    MiddleInitial: string option
    LastName: string
}

type OrderLineQty = OrderLineQty of int