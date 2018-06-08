
(*
    Function associativity and composition p -309
*)

let F x y z = x y z
let F' x y z = (x y) z

let F'' x y z = x (y z)
let F''' x y z = y z |> x // using forward pipe
let F'''' x y z = x <| y z // using backward pipe


// composition

// 'a -> 'b >> 'b -> 'c = 'a -> 'c
let f (x:int) = float x * 3.0
let g (y:float) = y > 1.0
let h' x = g (f (x))
// our custom comp operator
let (|>>|) x y z =  y (x z)
let h'' = f |>>| g
let h = f >> g


// == comp in practice - p 312 ==

// precidence order (>> evaled last)
let add n x = n + x
let times n x = x * n
// add 1 and times 2 evaled first, then comped
let add1Times2 = add 1 >> times 2
add1Times2 3 // 8?

let twice f = f >> f
let add1Twice = twice (add 1)
add1Twice 9
// but not this...
// let addThenMult = (+) >> (*)
// but THIS
let addXThenMult x = (+) x >> (*)
(addXThenMult 1) 3 4

// backwards comp
let times2Add1 = add 1 << times 2
times2Add1 3
// ... allows more English like APIs
let myList = []
myList |> List.isEmpty |> not
myList |> (not << List.isEmpty)