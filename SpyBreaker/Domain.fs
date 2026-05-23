module SpyBreaker.Domain

type Rectangle = { X: float32; Y: float32; Width: float32; Height: float32 }

type Code = int list
type SpyHint = { Position: int; Value: int }

type GameState = {
    SecretCode: Code
    TurnsLeft: int
    History: (Code * int * int) list
    UsedSpy: bool
    LastHint: SpyHint option
    CurrentInput: string
    IsGameOver: bool
    Message: string
}