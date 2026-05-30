# SpyBreaker

SpyBreaker is a graphical bomb-defusal puzzle game written in F# using Raylib-cs.

## Game Overview

The game generates a secret 4-digit code with unique digits. The player has 10 turns to discover the code. After each guess, the game provides Strike and Ball feedback indicating how close the guess is to the correct answer. A single-use hint is available at the cost of 2 turns.

## Game Rules / How to Play

1. Start the game using the command below and press Enter on the intro screen.
2. The objective is to guess the secret 4-digit code within 10 turns.
3. All digits in the secret code are unique.
4. Type digits using the keyboard. Only numeric characters are accepted and input is limited to 4 digits.
5. Use Backspace to delete a digit and Enter to submit a guess.
6. After each valid guess:

   * **Strikes (S)** indicate digits that are correct and in the correct position.
   * **Balls (B)** indicate digits that exist in the code but are in the wrong position.
7. A hint can be requested once per game by pressing the **S** key or clicking the on-screen hint button.

   * The hint reveals one digit and its position.
   * Using a hint costs 2 turns.
   * A hint cannot be used more than once or when fewer than 2 turns remain.
8. The player wins by obtaining 4 Strikes.
9. The player loses when all turns are used without finding the correct code.
10. After the game ends, press Enter to close the window.

## Prerequisites

* .NET 10 SDK

Install from:

https://dotnet.microsoft.com/

## Build & Run

From the repository root:

```bash
dotnet restore SpyBreaker/SpyBreaker.fsproj
dotnet run --project SpyBreaker
```


Alternatively:

```bash
cd SpyBreaker
dotnet restore
dotnet run
```

This command builds and launches the game window.

## Dependencies

* .NET 10 SDK
* Raylib-cs

The project uses the Raylib-cs NuGet package for graphics rendering. Native runtime files are included in the repository. If native library errors occur, ensure that you are running on a supported platform with matching runtime files.

## Requirements Changes

No requirement changes were made after the proposal submission.

## LLM Usage

GitHub Copilot was used to assist with Raylib-cs UI implementation, event handling, and general F# programming tasks.

Some generated code required manual modification because the suggestions did not always match the project requirements, particularly in game UI.

The main limitation was that Copilot sometimes misunderstood the intended game rules, so requirement-specific behavior had to be implemented and verified manually.
