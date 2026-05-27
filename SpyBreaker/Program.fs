namespace SpyBreaker

open System
open System.IO
open Raylib_cs
open SpyBreaker.Game
open SpyBreaker.UI

module Program =
    let private introTypingSpeed = 0.045f
    let private fontChars = Array.append [| 0x0020 .. 0x007E |] [| 0xAC00 .. 0xD7A3 |]
    let private introText =
        "두둥....폭탄 해체를 시작합니다.\n코드는 중복 없는 4자리 숫자입니다.\n10번의 턴 안에 코드를 입력하면 됩니다.\n\n힌트는 4자리 중 하나의 위치와 숫자를 알려주지만,\n힌트를 쓰면 턴이 2개 줄어듭니다.\n힌트는 딱! 한 번만 주어집니다."

    [<EntryPoint>]
    let main _ =
        Raylib.InitWindow(1280, 820, "Spy Breaker")
        Raylib.SetTargetFPS(60)

        let fontPath = Path.Combine(AppContext.BaseDirectory, "MALGUN.TTF")
        let font = Raylib.LoadFontEx(fontPath, 40, fontChars, fontChars.Length)

        let mutable state = initialState ()
        let mutable showIntro = true
        let mutable introTimer = 0f
        let mutable introIndex = 0
        let mutable isRunning = true

        while isRunning && not (UI.isTrue (Raylib.WindowShouldClose())) do
            let dt = Raylib.GetFrameTime()

            if showIntro then
                introTimer <- introTimer + dt
                if introTimer >= introTypingSpeed && introIndex < introText.Length then
                    introIndex <- introIndex + 1
                    introTimer <- 0f

                if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.Enter)) then
                    showIntro <- false
                    introIndex <- introText.Length
            else
                if state.AlertAnimTime > 0f then
                    state <- { state with AlertAnimTime = max 0f (state.AlertAnimTime - dt) }

                if state.IsGameOver && state.Result.IsSome then
                    state <- { state with EndingAnimTime = min 1.2f (state.EndingAnimTime + dt) }

                if not state.IsGameOver then
                    let mutable cp = Raylib.GetCharPressed()
                    while cp > 0 do
                        let c = char cp
                        if Char.IsDigit c && state.CurrentInput.Length < 4 then
                            state <- { state with CurrentInput = state.CurrentInput + string c }
                        cp <- Raylib.GetCharPressed()

                    if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.Backspace)) && state.CurrentInput.Length > 0 then
                        state <- { state with CurrentInput = state.CurrentInput.Substring(0, state.CurrentInput.Length - 1) }

                    if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.Enter)) then
                        state <- submitGuess state

                    let hintClicked =
                        if UI.isTrue (Raylib.IsMouseButtonPressed(MouseButton.Left)) then
                            let mousePosition = Raylib.GetMousePosition()
                            UI.isTrue (Raylib.CheckCollisionPointRec(mousePosition, UI.hintButtonRect))
                        else
                            false

                    if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.S)) || hintClicked then
                        state <- requestHint state
                elif UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.Enter)) then
                    isRunning <- false

            Raylib.BeginDrawing()
            let introPreview = if introIndex >= introText.Length then introText else introText.Substring(0, introIndex)
            UI.drawScene font state introPreview showIntro
            Raylib.EndDrawing()

        Raylib.UnloadFont(font)
        Raylib.CloseWindow()
        0
