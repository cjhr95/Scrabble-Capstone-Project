# Scrabble Capstone Project

Repository for the Speed Scrabble project, part of the Missouri S&T Senior Design class, CS 4090

## Description

This variant of scrabble is aimed to be a speed-focused scrabble. One where each second counts and the faster you can recollect your vocabulary, the greater the chance you will win. In a general sense, the game focuses on trying to make the player to think quickly, even if it means they won’t make the longest word they can. With a chess-like turn timer and with letters having a lifetime of their own, the player must quickly be able to create the longest word they can think of as fast as possible. 

### Terminology

    Game time – The overarching timer for the game to record overall elapsed time. 

    Turn time – A particular player’s timer for their turn.
    
    Player - A game player. Refers to both the living and computer players
    
    Token - An in-game playable object. These are essentially just letters but with more information.
    
    Tile - A singular cell in the grid

## Features
### Implemented
    literally nothing lol
    
### To be implemented

    A 15 by 15 playable grid 

    A turn-based flow with only one playable word per turn 

    Horizontal (Left to right) and Vertical (Top to down) word placements 

    Crossword-like word overlapping 

    Words verified against a central dataset (Scrabble dictionary) 

    First played word intersects with the center tile 

    7 Letters to form a “Hand” which is refilled to 7 at the end of each turn 

    Finite number of letters and each being worth a different number of points 

    Game ending when a turn time runs out, letters run out, or all players pass their turn 

    Game scoring based on points accumulated, and remaining turn time (if there is any left). 

    A computer player to play against the user 

    A turn time that stops when a turn ends and continues when the turn begins 

    Letters having a “lifetime” starting at X and giving a point multiplier scaling down to a 1x multiplier at 0 remaining time. 

    An in-game store to purchase things at the cost of points and randomizes the 8 items on each turn 

    “Buffs” to enhance the player such as point multipliers, turn time increasers, or word stealers 

    “Debuffs” to debilitate the opponent such as letter swappers or point reducers 

    Letters which vary in points based on how common they are 

    Obstacles in grid to make word placement more complex 

    Special grid squares that increase (or decrease) point gain if used 

    Improved aesthetics and UI/UX 

    Bingo-like picture rewards that give players points when completing patterns on the grid 

    Ability to have more than two players in a game 

    Difficulty levels (with AI) 
