namespace SpyBreaker

open System.Numerics
open Raylib_cs
open SpyBreaker.Domain

module UI =
    let isTrue (b: CBool) : bool = CBool.op_Implicit(b)

    let drawScene (font: Font) (state: GameState) (introText: string) (showIntro: bool) =
        Raylib.ClearBackground(Color(30, 30, 35, 255))
        
        if showIntro then
            // 6개 인수를 모두 채움 (font, text, position, fontSize, spacing, tint)
            Raylib.DrawTextEx(font, introText, Vector2(50f, 50f), 20f, 1f, Color.Green)
        else
            Raylib.DrawRectangleRounded(Rectangle(50f, 50f, 400f, 600f), 0.1f, 20, Color(20, 20, 20, 255))
            Raylib.DrawRectangle(60, 100, 380, 540, Color.White)
            
            let alertColor = if state.UsedSpy then Color.Gray else Color.Magenta
            Raylib.DrawRectangle(60, 100, 380, 50, alertColor)
            Raylib.DrawTextEx(font, "친구: 야 너 단톡에 뭐야?", Vector2(80f, 115f), 20f, 1f, Color.White)
            
            for i in 0..3 do
                let digit = if i < state.CurrentInput.Length then state.CurrentInput.[i].ToString() else ""
                Raylib.DrawRectangle(80 + (i * 90), 200, 70, 70, Color.LightGray)
                Raylib.DrawText(digit, 110 + (i * 90), 220, 40, Color.Black)

            let btnColor = if state.UsedSpy then Color.Gray else Color.Purple
            Raylib.DrawRectangle(100, 400, 320, 60, btnColor)
            Raylib.DrawTextEx(font, if state.UsedSpy then "힌트 사용 완료" else "힌트 요청 (S)", Vector2(120f, 420f), 20f, 1f, Color.White)

            Raylib.DrawTextEx(font, "남은 턴: " + state.TurnsLeft.ToString(), Vector2(100f, 550f), 20f, 1f, Color.Red)
            Raylib.DrawTextEx(font, state.Message, Vector2(100f, 580f), 20f, 1f, Color.Black)

            Raylib.DrawTextEx(font, "=== LOG ===", Vector2(500f, 50f), 20f, 1f, Color.Green)
            state.History |> List.iteri (fun i (g, s, b) ->
                let gStr = g |> List.map string |> String.concat ""
                Raylib.DrawTextEx(font, gStr + " : " + s.ToString() + "S " + b.ToString() + "B", Vector2(500f, 90f + float32(i*25)), 18f, 1f, Color.Gray))