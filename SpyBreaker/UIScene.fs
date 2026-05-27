namespace SpyBreaker

open Raylib_cs
open SpyBreaker.Domain
open SpyBreaker.UICommon
open SpyBreaker.UIDecoration

module UIScene =
    let drawScene (font: Font) (state: GameState) (introText: string) (showIntro: bool) =
        drawBackground()

        if showIntro then
            drawIntro font introText
        else
            drawShell()
            drawHeader font state
            drawAlertBanner font state
            drawInputSlots font state
            drawHintButton font state
            drawStatusCard font state
            drawLogCard font state
            drawEnding font state