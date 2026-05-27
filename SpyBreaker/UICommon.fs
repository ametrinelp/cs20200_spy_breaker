namespace SpyBreaker

open System
open System.Numerics
open Raylib_cs

module UICommon =
    let isTrue (b: CBool) : bool = CBool.op_Implicit(b)

    let hintButtonRect = Rectangle(72f, 516f, 602f, 60f)

    let canvasW = 1280f
    let canvasH = 820f
    let textScale = 1.26f

    let rgba r g b a = Color(byte r, byte g, byte b, byte a)

    let makeAlpha (color: Color) (alpha: byte) = Color(color.R, color.G, color.B, alpha)

    let centerText (font: Font) (text: string) (fontSize: float32) (spacing: float32) (centerX: float32) (y: float32) (color: Color) =
        let scaledSize = fontSize * textScale
        let measured = Raylib.MeasureTextEx(font, text, scaledSize, spacing)
        Raylib.DrawTextEx(font, text, Vector2(centerX - (measured.X / 2f), y), scaledSize, spacing, color)

    let drawTextScaled (font: Font) (text: string) (position: Vector2) (fontSize: float32) (spacing: float32) (color: Color) =
        Raylib.DrawTextEx(font, text, position, fontSize * textScale, spacing, color)

    let drawMultilineText (font: Font) (text: string) (x: float32) (y: float32) (fontSize: float32) (spacing: float32) (lineGap: float32) (color: Color) =
        let scaledSize = fontSize * textScale
        let scaledGap = lineGap * textScale
        text.Split([| '\n' |], StringSplitOptions.None)
        |> Array.iteri (fun i line ->
            Raylib.DrawTextEx(font, line, Vector2(x, y + float32 i * (scaledSize + scaledGap)), scaledSize, spacing, color))