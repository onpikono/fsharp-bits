let findTheMutuals friends1 friends2 =
    friends1
    |> List.filter (fun friend -> List.contains friend friends2)
    |> List.distinct


let printTheMutuals friends1 friends2 =
    match findTheMutuals friends1 friends2 with
    | [] -> printfn "You have no mutual friends."
    | ms ->
        let formattedFriends =
            ms
            |> List.sort
            |> String.concat ", "

        printfn $"You have {ms.Length} friends in common."
        printfn $"Mutual friends: {formattedFriends}"


let myFriends = [ "b"; "a"; "d"; "c"; "c" ]
let theirFriends = [ "f"; "c"; "a"; "d" ]
let noFriends = [ "x"; "z" ]

printTheMutuals myFriends theirFriends // Prints: You have 3 friends in common.\n Mutual friends: a, c, d
printTheMutuals myFriends noFriends // Prints: You have no mutual friends
