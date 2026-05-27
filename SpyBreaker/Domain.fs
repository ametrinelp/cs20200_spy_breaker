module SpyBreaker.Domain

type Rectangle = { X: float32; Y: float32; Width: float32; Height: float32 }

type Code = int list
type SpyHint = { Position: int; Value: int }
type GameResult = Win | Lose

type GameState = {
    SecretCode: Code
    TurnsLeft: int
    History: (Code * int * int) list
    UsedSpy: bool
    LastHint: SpyHint option
    Result: GameResult option
    CurrentInput: string
    IsGameOver: bool
    Message: string
    AlertText: string
    AlertAnimTime: float32
    EndingAnimTime: float32
}
