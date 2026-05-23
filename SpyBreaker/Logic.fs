module SpyBreaker.Logic

open System
open SpyBreaker.Domain

let rnd = Random()

let generateSecret () : Code =
    let rec loop acc =
        if List.length acc = 4 then acc
        else
            let n = rnd.Next(0, 10)
            if List.contains n acc then loop acc
            else loop (acc @ [n])
    loop []

let calculateResult (secret: Code) (guess: Code) =
    let strikes = List.zip secret guess |> List.filter (fun (s, g) -> s = g) |> List.length
    let secretSet = Set.ofList secret
    let common = guess |> List.filter (fun g -> Set.contains g secretSet) |> List.length
    (strikes, common - strikes)

let isValid (input: string) =
    input.Length = 4 && (input |> Seq.forall Char.IsDigit) && (input |> Seq.distinct |> Seq.length = 4)

let parseInput (input: string) =
    input |> Seq.map (fun c -> int c - int '0') |> List.ofSeq