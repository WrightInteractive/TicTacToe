# TicTacToe
TicTacToe game for Illumix Code Review

## Whitebox Game Prototype
TicTacToe is a whitebox game prototype that uses no external packages, and no additional unity packages outside of URP and TMPro.

This will allow a design team to examine all fuctionality without placeholder assets or design bias. 

## Functionality
- **Solo Play Mode**: Player will get to play the game against either the DumbAI(Beginner) or SmartAI(Expert).
- **Two Player Mode**: Two Players will get to play against each other, alternating turns.
- **Dumb AI**: Randomly selects a square, checks to see if it has been selected, and if not, chooses that square. If square has been selected, call same function in recursion
- **Smart AI**: Checks for any winning square, if one is found, it will play the winning square. If no winning square is found, it will check for and blocks to keep opponent from winning, and will play the blocking square. If no blocking square is found, it will randomly select an available square