namespace SpyBreaker

open System
open Raylib_cs
open SpyBreaker.Domain
open SpyBreaker.Logic
open SpyBreaker.UI

module Program =
    [<EntryPoint>]
    let main _ =
        Raylib.InitWindow(900, 700, "Smartphone Lock")
        let font = Raylib.LoadFontEx("malgun.ttf", 32, Array.append [| 0x0020 .. 0x007E |] [| 0xAC00 .. 0xD7A3 |], 11172)
        
        let mutable state = { SecretCode = generateSecret(); TurnsLeft = 10; History = []; UsedSpy = false; LastHint = None; CurrentInput = ""; IsGameOver = false; Message = "4자리 입력" }
        let introText = "MT 다음 날, 핸드폰이 잠겼다!\n친구가 보낸 카톡: 단톡에 왜 그랬냐?\n\n[ENTER]를 눌러 잠금 해제 시작"
        let mutable showIntro = true
        let mutable timer = 0.0f
        let mutable charIndex = 0

        while UI.isTrue (Raylib.WindowShouldClose()) |> not do
            if showIntro then
                timer <- timer + Raylib.GetFrameTime()
                if timer > 0.05f && charIndex < introText.Length then charIndex <- charIndex + 1; timer <- 0.0f
                if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.Enter)) then showIntro <- false
            
            Raylib.BeginDrawing()
            UI.drawScene font state (introText.Substring(0, charIndex)) showIntro
            
            if not showIntro && not state.IsGameOver then
                let mutable cp = Raylib.GetCharPressed()
                while cp > 0 do
                    let c = char cp
                    if Char.IsDigit(c) && state.CurrentInput.Length < 4 then state <- { state with CurrentInput = state.CurrentInput + string c }
                    cp <- Raylib.GetCharPressed()
                
                if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.Enter)) && state.CurrentInput.Length = 4 then
                    let (s, b) = calculateResult state.SecretCode (parseInput state.CurrentInput)
                    let newTurns = state.TurnsLeft - 1
                    let isWin = (s = 4)
                    let isLose = (newTurns <= 0 && not isWin)
                    let msg = if isWin then "해제 성공!" else if isLose then "실패! 잠김" else s.ToString() + "S " + b.ToString() + "B"
                    state <- { state with TurnsLeft = newTurns; History = (parseInput state.CurrentInput, s, b) :: state.History; CurrentInput = ""; Message = msg; IsGameOver = (isWin || isLose) }
                
                if UI.isTrue (Raylib.IsKeyPressed(KeyboardKey.S)) && not state.UsedSpy && state.TurnsLeft >= 2 then
                    let idx = rnd.Next(0, 4)
                    let msg = (idx+1).ToString() + "번째는 " + state.SecretCode.[idx].ToString() + "임"
                    state <- { state with UsedSpy = true; TurnsLeft = state.TurnsLeft - 2; Message = msg }

            Raylib.EndDrawing()
        Raylib.UnloadFont(font)
        Raylib.CloseWindow()
        0