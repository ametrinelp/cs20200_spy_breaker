module SpyBreaker.Game

open SpyBreaker.Domain
open SpyBreaker.Logic

let private alertDuration = 2.8f

let private withAlert text state =
    { state with AlertText = text; AlertAnimTime = alertDuration }

let initialState () =
    { SecretCode = generateSecret()
      TurnsLeft = 10
      History = []
      UsedSpy = false
      LastHint = None
      Result = None
      CurrentInput = ""
      IsGameOver = false
      Message = "4자리 숫자를 입력하세요."
      AlertText = ""
      AlertAnimTime = 0f
      EndingAnimTime = 0f }

let requestHint state =
    if state.UsedSpy then
        state |> withAlert "힌트는 이미 사용했습니다."
    elif state.TurnsLeft < 2 then
        state |> withAlert "턴이 부족해 힌트를 쓸 수 없습니다."
    else
        let index = rnd.Next(0, 4)
        let value = state.SecretCode.[index]
        let hint = { Position = index + 1; Value = value }
        let nextState =
            state
            |> withAlert (sprintf "힌트: %d번째 숫자는 %d" hint.Position hint.Value)
            |> fun next ->
                { next with
                    UsedSpy = true
                    TurnsLeft = next.TurnsLeft - 2
                    LastHint = Some hint
                    Message = "힌트를 받았습니다." }

        if nextState.TurnsLeft <= 0 then
            { nextState with
                IsGameOver = true
                Result = Some Lose
                Message = "실패"
                EndingAnimTime = 0f }
        else
            nextState

let submitGuess state =
    if state.CurrentInput.Length <> 4 then
        state |> withAlert "4자리 숫자를 입력하세요."
    else
        let guess = parseInput state.CurrentInput
        let strikes, balls = calculateResult state.SecretCode guess
        let newTurns = state.TurnsLeft - 1
        let isWin = strikes = 4
        let isLose = newTurns <= 0 && not isWin
        let result = if isWin then Some Win elif isLose then Some Lose else None
        let message =
            if isWin then "성공"
            elif isLose then "실패"
            else sprintf "%dS %dB" strikes balls

        let alertText =
            if isWin then "정답입니다"
            elif isLose then "턴이 끝났습니다"
            else sprintf "%d Strike, %d Ball" strikes balls

        { state with
            TurnsLeft = newTurns
            History = (guess, strikes, balls) :: state.History
            CurrentInput = ""
            Message = message
            IsGameOver = isWin || isLose
            Result = result
            EndingAnimTime = 0f
        }
        |> withAlert alertText