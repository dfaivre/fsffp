namespace Example

type PersonType = { name: string}

module Person =
    let create = { name="Bob"}

module Customer =
    type T = { accountId:int }

    let create id = { accountId = id}

type Fruit = Pear | Apple
module Fruit =
    let isPear f = f = Pear

module Main =
    let p = Person.create
    let pp : PersonType = {name = "Bobby"}

    let customer = Customer.create 4
    let cc : Customer.T = { accountId = 3 }

    let r1 = Fruit.isPear Fruit.Apple
    let pear:Fruit = Pear