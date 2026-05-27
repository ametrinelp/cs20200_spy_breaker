namespace SpyBreaker

open System
open System.Numerics
open Raylib_cs
open SpyBreaker.Domain
open SpyBreaker.UICommon

module UIDecoration =
    let private drawBombIcon (font: Font) (centerX: float32) (centerY: float32) (radius: float32) (fuseBroken: bool) (label: string) (accent: Color) =
        let _body = Rectangle(centerX - radius, centerY - radius, radius * 2f, radius * 2f)
        Raylib.DrawCircleV(Vector2(centerX + 6f, centerY + 8f), radius + 6f, rgba 0 0 0 70)
        Raylib.DrawCircleV(Vector2(centerX, centerY), radius, rgba 44 45 50 255)
        Raylib.DrawCircleV(Vector2(centerX - radius * 0.28f, centerY - radius * 0.28f), radius * 0.42f, rgba 255 255 255 10)
        Raylib.DrawCircleV(Vector2(centerX - radius * 0.08f, centerY + radius * 0.12f), radius * 0.18f, rgba 255 255 255 8)

        let fuseColor = if fuseBroken then rgba 255 165 96 255 else accent
        Raylib.DrawLineEx(Vector2(centerX - 10f, centerY - radius + 4f), Vector2(centerX + 18f, centerY - radius - 26f), 8f, fuseColor)
        Raylib.DrawLineEx(Vector2(centerX + 18f, centerY - radius - 26f), Vector2(centerX + 36f, centerY - radius - 34f), 3f, fuseColor)
        Raylib.DrawCircleV(Vector2(centerX + 40f, centerY - radius - 38f), 8f, fuseColor)

        for i in 0..3 do
            let angle = float32 i * 1.57f + 0.2f
            let studX = centerX + MathF.Cos(angle) * (radius - 18f)
            let studY = centerY + MathF.Sin(angle) * (radius - 18f)
            Raylib.DrawCircleV(Vector2(studX, studY), 4.5f, rgba 255 255 255 34)

        centerText font label 28f 1f centerX (centerY - 14f) Color.White
        centerText font "TURN" 13f 1f centerX (centerY + 18f) (rgba 205 214 228 255)

    let private drawExplosionBurst (centerX: float32) (centerY: float32) (progress: float32) =
        let radius = 72f + progress * 132f
        let smokeAlpha = byte (170f * (1f - progress))
        let coreAlpha = byte (255f * (1f - progress))
        Raylib.DrawCircleV(Vector2(centerX, centerY), radius, makeAlpha (rgba 255 129 61 255) smokeAlpha)
        Raylib.DrawCircleV(Vector2(centerX, centerY), radius * 0.62f, makeAlpha (rgba 255 208 91 255) coreAlpha)
        Raylib.DrawCircleV(Vector2(centerX, centerY), radius * 0.28f, makeAlpha (rgba 255 245 214 255) coreAlpha)
        for i in 0..7 do
            let angle = float32 i * 0.78f
            let start = Vector2(centerX + MathF.Cos(angle) * 36f, centerY + MathF.Sin(angle) * 36f)
            let finish = Vector2(centerX + MathF.Cos(angle) * (radius + 40f), centerY + MathF.Sin(angle) * (radius + 40f))
            Raylib.DrawLineEx(start, finish, 6f, makeAlpha (rgba 255 188 88 255) smokeAlpha)

    let drawBackground () =
        Raylib.DrawRectangle(0, 0, int canvasW, int canvasH, rgba 18 19 22 255)

    let drawShell () =
        let shell = Rectangle(28f, 28f, 1224f, 764f)
        Raylib.DrawRectangleRounded(shell, 0.08f, 16, rgba 24 25 29 255)
        Raylib.DrawRectangleRoundedLines(shell, 0.08f, 16, 2f, rgba 68 70 76 255)

        let leftPanel = Rectangle(48f, 48f, 642f, 724f)
        let rightPanel = Rectangle(712f, 48f, 520f, 724f)
        Raylib.DrawRectangleRounded(leftPanel, 0.08f, 16, rgba 31 32 36 255)
        Raylib.DrawRectangleRoundedLines(leftPanel, 0.08f, 16, 1.5f, rgba 74 76 82 255)
        Raylib.DrawRectangleRounded(rightPanel, 0.08f, 16, rgba 29 30 34 255)
        Raylib.DrawRectangleRoundedLines(rightPanel, 0.08f, 16, 1.5f, rgba 74 76 82 255)

    let drawHeader (font: Font) (state: GameState) =
        let header = Rectangle(68f, 62f, 618f, 84f)
        Raylib.DrawRectangleRounded(header, 0.1f, 16, rgba 34 35 40 255)
        Raylib.DrawRectangleRoundedLines(header, 0.1f, 16, 1.5f, rgba 82 84 90 255)
        Raylib.DrawRectangleRounded(Rectangle(68f, 62f, 618f, 8f), 0.08f, 12, rgba 178 181 191 255)

        centerText font "BOMB DISARMER" 18f 1f 358f 82f (rgba 235 236 242 255)
        centerText font "폭탄 해체 하기" 28f 1f 358f 104f Color.White

        let badgeColor = if state.UsedSpy then rgba 92 96 104 255 else rgba 84 87 95 255
        let badgeText = if state.UsedSpy then "힌트 사용됨" else "힌트 가능"
        Raylib.DrawRectangleRounded(Rectangle(574f, 88f, 106f, 32f), 0.45f, 12, badgeColor)
        centerText font badgeText 14f 1f 627f 97f Color.White

    let drawAlertBanner (font: Font) (state: GameState) =
        if state.AlertAnimTime > 0f && not (String.IsNullOrWhiteSpace state.AlertText) then
            let progress = max 0f (min 1f (state.AlertAnimTime / 2.8f))
            let alpha = byte (255f * progress)
            let slide = (1f - progress) * -18f
            let banner = Rectangle(72f, 156f + slide, 578f, 50f)
            Raylib.DrawRectangleRounded(banner, 0.2f, 14, makeAlpha (rgba 34 35 40 255) alpha)
            Raylib.DrawRectangleRounded(Rectangle(banner.X, banner.Y, 6f, banner.Height), 0.2f, 8, makeAlpha (rgba 182 184 192 255) alpha)
            drawTextScaled font "알림" (Vector2(94f, banner.Y + 6f)) 16f 1f (makeAlpha (rgba 230 231 237 255) alpha)
            drawTextScaled font state.AlertText (Vector2(94f, banner.Y + 24f)) 18f 1f (makeAlpha Color.White alpha)

    let drawInputSlots (font: Font) (state: GameState) =
        let startX = 142f
        let topY = 330f
        for i in 0..3 do
            let slotX = startX + float32 i * 114f
            let slotRect = Rectangle(slotX, topY, 92f, 104f)
            let focused = i = state.CurrentInput.Length && not state.IsGameOver
            let hasDigit = i < state.CurrentInput.Length
            let fill = if hasDigit then rgba 44 45 49 255 else rgba 28 29 33 255
            let border = if focused then rgba 214 215 220 255 else rgba 85 87 94 255
            Raylib.DrawRectangleRounded(slotRect, 0.18f, 10, fill)
            Raylib.DrawRectangleRoundedLines(slotRect, 0.18f, 10, 2f, border)

            if hasDigit then
                let digit = string state.CurrentInput.[i]
                centerText font digit 38f 1f (slotX + 46f) (topY + 28f) Color.White
            elif focused then
                Raylib.DrawLineEx(Vector2(slotX + 26f, topY + 70f), Vector2(slotX + 66f, topY + 70f), 2f, rgba 214 215 220 255)

        drawTextScaled font "폭탄 해체 코드 4자리를 입력한 뒤 Enter" (Vector2(136f, 456f)) 16f 1f (rgba 205 207 214 255)

    let drawHintButton (font: Font) (state: GameState) =
        let isDisabled = state.UsedSpy || state.IsGameOver
        let baseColor = if isDisabled then rgba 52 53 57 255 else rgba 74 75 82 255
        let glowColor = if isDisabled then rgba 0 0 0 0 else rgba 255 255 255 18
        Raylib.DrawRectangleRounded(Rectangle(hintButtonRect.X - 3f, hintButtonRect.Y - 3f, hintButtonRect.Width + 6f, hintButtonRect.Height + 6f), 0.22f, 14, glowColor)
        Raylib.DrawRectangleRounded(hintButtonRect, 0.22f, 14, baseColor)
        Raylib.DrawRectangleRoundedLines(hintButtonRect, 0.22f, 14, 2f, rgba 165 167 175 180)

        let label =
            if state.IsGameOver then "게임 종료"
            elif state.UsedSpy then "힌트 사용 완료"
            else "힌트"

        let shortcut = if state.UsedSpy || state.IsGameOver then "" else "키보드 S 또는 버튼 클릭하기"
        centerText font label 26f 1f (hintButtonRect.X + hintButtonRect.Width / 2f) 530f Color.White
        if shortcut <> "" then
            centerText font shortcut 15f 1f (hintButtonRect.X + hintButtonRect.Width / 2f) 559f (rgba 226 228 234 255)

    let drawStatusCard (font: Font) (state: GameState) =
        let card = Rectangle(712f, 200f, 520f, 114f)
        Raylib.DrawRectangleRounded(card, 0.1f, 14, rgba 29 30 34 255)
        Raylib.DrawRectangleRoundedLines(card, 0.1f, 14, 1.5f, rgba 78 80 86 255)

        drawTextScaled font "카운트다운" (Vector2(736f, 214f)) 15f 1f (rgba 230 231 237 255)
        drawTextScaled font state.Message (Vector2(736f, 236f)) 19f 1f Color.White
        drawTextScaled font (sprintf "%02d" state.TurnsLeft) (Vector2(1114f, 206f)) 48f 1f (rgba 235 236 242 255)
        drawTextScaled font "TURN" (Vector2(1136f, 262f)) 16f 1f (rgba 185 188 196 255)

        let hintLine =
            match state.LastHint with
            | Some hint -> sprintf "최근 힌트: %d번째 숫자 = %d" hint.Position hint.Value
            | None -> "최근 힌트 없음"

        drawTextScaled font hintLine (Vector2(736f, 272f)) 16f 1f (rgba 185 188 196 255)
        drawTextScaled font "폭탄이 터지기 전에 해체하세요" (Vector2(736f, 294f)) 16f 1f (rgba 235 236 242 255)

    let drawLogCard (font: Font) (state: GameState) =
        let card = Rectangle(712f, 336f, 520f, 420f)
        Raylib.DrawRectangleRounded(card, 0.1f, 14, rgba 29 30 34 255)
        Raylib.DrawRectangleRoundedLines(card, 0.1f, 14, 1.5f, rgba 78 80 86 255)

        drawTextScaled font "시도 기록" (Vector2(736f, 352f)) 20f 1f Color.White
        drawTextScaled font "최근 입력이 위에 표시됩니다" (Vector2(736f, 380f)) 14f 1f (rgba 185 188 196 255)

        let recent = state.History |> List.truncate 5
        if recent.IsEmpty then
            drawTextScaled font "아직 기록이 없습니다" (Vector2(736f, 412f)) 18f 1f (rgba 185 188 196 255)
        else
            recent
            |> List.iteri (fun i (guess, strikes, balls) ->
                let rowY = 410f + float32 i * 70f
                let row = Rectangle(732f, rowY - 2f, 456f, 56f)
                let rowColor = if i % 2 = 0 then rgba 35 36 40 255 else rgba 32 33 37 255
                Raylib.DrawRectangleRounded(row, 0.15f, 8, rowColor)
                let guessText = guess |> List.map string |> String.concat ""
                drawTextScaled font guessText (Vector2(748f, rowY + 12f)) 20f 1f Color.White
                drawTextScaled font (sprintf "%dS %dB" strikes balls) (Vector2(1060f, rowY + 13f)) 18f 1f (rgba 204 205 211 255))

    let drawIntro (font: Font) (introText: string) =
        let introCard = Rectangle(120f, 120f, 1040f, 580f)
        Raylib.DrawRectangleRounded(introCard, 0.1f, 18, rgba 26 27 31 245)
        Raylib.DrawRectangleRoundedLines(introCard, 0.1f, 18, 1.5f, rgba 82 84 90 255)

        drawBombIcon font 300f 320f 108f false "10" (rgba 235 236 242 255)
        centerText font "폭탄 해체" 40f 1f 740f 176f Color.White
        centerText font "4자리를 맞히면 됩니다" 18f 1f 740f 226f (rgba 205 207 214 255)
        drawMultilineText font introText 520f 302f 22f 1f 8f (rgba 235 236 242 255)

        Raylib.DrawRectangleRounded(Rectangle(550f, 602f, 360f, 56f), 0.22f, 16, rgba 74 75 82 255)
        centerText font "ENTER를 누르면 시작합니다" 20f 1f 730f 617f Color.White

    let drawEnding (font: Font) (state: GameState) =
        match state.Result with
        | None -> ()
        | Some Win ->
            let progress = min 1f state.EndingAnimTime
            Raylib.DrawRectangle(0, 0, int canvasW, int canvasH, rgba 5 8 14 170)
            let panel = Rectangle(250f, 190f, 780f, 430f)
            Raylib.DrawRectangleRounded(panel, 0.12f, 20, rgba 25 26 30 245)
            Raylib.DrawRectangleRoundedLines(panel, 0.12f, 20, 2f, rgba 122 124 130 255)
            drawBombIcon font 640f 315f (106f - progress * 8f) true "STOP" (rgba 235 236 242 255)
            centerText font "성공" 38f 1f 640f 410f (rgba 235 236 242 255)
            centerText font "정답을 맞혔습니다" 18f 1f 640f 462f (rgba 205 207 214 255)
            centerText font (sprintf "남은 턴 %d" state.TurnsLeft) 18f 1f 640f 508f (rgba 205 207 214 255)
            centerText font "ENTER를 누르면 종료됩니다" 15f 1f 640f 548f (rgba 185 188 196 255)
        | Some Lose ->
            let progress = min 1f state.EndingAnimTime
            Raylib.DrawRectangle(0, 0, int canvasW, int canvasH, rgba 10 4 6 175)
            let panel = Rectangle(250f, 190f, 780f, 430f)
            Raylib.DrawRectangleRounded(panel, 0.12f, 20, rgba 32 18 20 245)
            Raylib.DrawRectangleRoundedLines(panel, 0.12f, 20, 2f, rgba 180 96 106 255)
            drawExplosionBurst 640f 308f progress
            centerText font "실패" 42f 1f 640f 410f (rgba 255 227 229 255)
            centerText font "정답을 맞히지 못했습니다" 18f 1f 640f 466f (rgba 205 207 214 255)
            centerText font (sprintf "정답은 %s였습니다" (state.SecretCode |> List.map string |> String.concat "")) 18f 1f 640f 510f (rgba 235 236 242 255)
            centerText font "ENTER를 누르면 종료됩니다" 15f 1f 640f 548f (rgba 185 188 196 255)