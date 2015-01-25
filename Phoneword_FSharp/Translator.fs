namespace Phoneword_FSharp

open System
open System.Text

module Translator =

    let translate (s:string) = 
        let isNumType c = Seq.exists ((=) c) " -0123456789" in
            seq { for c in s do 
                    match c with 
                    | _ when isNumType c    -> yield c
                    | 'A' | 'B' | 'C'       -> yield '2'
                    | 'D' | 'E' | 'F'       -> yield '3'
                    | 'G' | 'H' | 'I'       -> yield '4'
                    | 'J' | 'K' | 'L'       -> yield '5'
                    | 'M' | 'N' | 'O'       -> yield '6'
                    | 'P' | 'Q' | 'R' | 'S' -> yield '7'
                    | 'T' | 'U' | 'V'       -> yield '8'
                    | 'W' | 'X' | 'Y' | 'Z' -> yield '9'
                    | _                     -> ()
                }
            |> String.Concat
    
    let toNumber raw = 
        if String.IsNullOrWhiteSpace raw 
            then "" else translate (raw.ToUpperInvariant())
    