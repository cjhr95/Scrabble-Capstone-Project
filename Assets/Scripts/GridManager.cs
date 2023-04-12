using Assets;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GridManager : MonoBehaviour
{
  // ####################################### General Variables #######################################
  [SerializeField] public int width, height;            // Width and Height of the grid (in tiles)
  [SerializeField] private TileUnit tilePrefab;         // Placeholder to access the tile prefab
  public TileUnit[,] tiles;                            // Grid array
  public bool firstWordPlaced { get; private set; }     // Flag to force first word to intersect center.
  private Vector2 centerOfGrid;                         // Location of the center of tile array.

  // ######################################### Typing helpers #########################################
  private Vector2 activeTile;                           // Originating tile for typing.
  private int minIndex;                                 // Minimum index to prevent over-erasing
  public bool isTypingHorizontal { get; private set; }  // Reference to determine direction of typing
  public bool isTypingVertical { get; private set; }    // See above
  private int typeOffset;                               // The offset from originiating tile.
  private string currentWord;                           // The current "working" word user is typing.
  private Stack<TileUnit> wordTiles;                    // The tiles that the player has written on. This is *in order*
  private Stack<Token> tokensUsed;
  private int runningScore;

  PlayerManager playerManager;
  
  // Description: Generates a grid based on the width
  //              and height fields. Also fills out
  //              the tiles array for easier reference.
  void GenerateGrid()
  {
    tiles = new TileUnit[width, height];
    wordTiles = new Stack<TileUnit>();
    tokensUsed = new Stack<Token>();
    isTypingHorizontal = false;
    isTypingVertical = false;
    firstWordPlaced = false;
    centerOfGrid = new Vector2(width / 2, height / 2);
    typeOffset = 0;
    playerManager = FindObjectOfType<PlayerManager>();
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        var createdTile = Instantiate(tilePrefab);
        createdTile.transform.position = new Vector3(x, y);
        createdTile.name = $"Tile ({x},{y})";
        tiles[x,y] = createdTile;


        // TODO: Add logic to determine if tile is special.
        if (new Vector2(x,y) == centerOfGrid)
        {
          createdTile.Initialize(new Vector2(x, y), new Vector2(x, y), color: Color.white);
          //createdTile.Initialize(new Vector2(x, y), new Vector2(x, y), color: Color.gray, pointVal: 100, specialModifierFunc: val => (int) (val * 1.25));
        } else
        {
          System.Random random = new System.Random();
          int chance = random.Next(100);
          if (chance <= 5) createdTile.Initialize(new Vector2(x, y), new Vector2(x, y), color: Color.red, pointVal: -random.Next(1, 3));
          else if (chance <= 10) createdTile.Initialize(new Vector2(x, y), new Vector2(x, y), color: Color.green, pointVal: random.Next(1, 10));
          else createdTile.Initialize(new Vector2(x, y), new Vector2(x, y));
        }
      }
    }
  }

  // Description: Called before the first frame update
  void Start()
  {
    GenerateGrid();
    GameDictionary.InitializeDictionary();

    Camera.main.transform.position = new Vector3((float)width / 2, (float)height / 2, -10);
    Camera.main.orthographicSize = 9;
    User.player.RepositionTokens();
  }

  // Description: Called once per frame.
  void Update()
  {
    if (!playerManager.GameOver)
    {
      if (playerManager.IsHumanPlayer())
      {
        // ####################################### Typing Logic #######################################
        // ######################################## Horzontal #########################################
        if (isTypingHorizontal)
        {
          // Code will check if enter/backspace is pressed. If not,
          // then it checks what the user is typing in.
          if (Input.GetKeyDown(KeyCode.Backspace))
          {
            // Skip over locked tiles that already have letters
            if ((int)activeTile.x + typeOffset > minIndex) typeOffset--;
            int xPos = (int)activeTile.x + typeOffset;
            while (tiles[xPos, (int)activeTile.y].locked && xPos > minIndex)
            {
              typeOffset--;
              xPos = (int)activeTile.x + typeOffset;
            }

            // Only empty unlocked tiles.
            if (!tiles[xPos, (int)activeTile.y].locked)
            {
              // Remove runningScore if it was added
              if (runningScore > 0)
              {
                LetterValues letterVal;
                if (Enum.TryParse(tiles[xPos, (int)activeTile.y].GetLetter().ToUpper(), out letterVal))
                {
                  runningScore -= (int)letterVal;
                }
              }

              // Reset tile
              tiles[xPos, (int)activeTile.y].ChangeLetter("");
              wordTiles.Pop();
              User.player.AddToHand(tokensUsed.Pop());
              currentWord = currentWord.Remove(currentWord.Length - 1, 1);
            }
          }
          else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
          {
            isTypingHorizontal = false;

            // Check if there is a letter afterwards and add them to word if needed.
            int xPos = (int)activeTile.x + typeOffset;
            while (xPos < width && tiles[xPos, (int)activeTile.y].GetLetter() != "")
            {
              currentWord += tiles[xPos, (int)activeTile.y].GetLetter();
              LetterValues letterVal;
              if (Enum.TryParse(tiles[xPos, (int)activeTile.y].GetLetter().ToUpper(), out letterVal))
              {
                runningScore += (int)letterVal;
              }
              typeOffset++;
              xPos = (int)activeTile.x + typeOffset;
            }
            typeOffset = 0;

            // TODO: VALIDATE WORD
            if (ValidateWord()) SubmitWord();
          }
          else
          {
            // Input needs to be filtered out before used. Unity/C# will read ALL
            // input, including ones that don't produce a legible character.
            string rawInput = Input.inputString;
            string filteredInput = Regex.Replace(rawInput, "[^A-Za-z]", "").ToUpper();
            Token tokenFromHand;
            if (filteredInput != "" && User.player.RetrieveToken(User.player.GetTokenFromLetter(filteredInput), out tokenFromHand))
            {
              // Loop is used to skip over characters to allow users to "add" to a word.
              // Essentially just skips tiles with letters in them already.
              int xPos = (int)activeTile.x + typeOffset;
              while (xPos < width && tiles[xPos, (int)activeTile.y].GetLetter() != "")
              {
                currentWord += tiles[xPos, (int)activeTile.y].GetLetter();
                LetterValues letterVal;
                if (Enum.TryParse(tiles[xPos, (int)activeTile.y].GetLetter().ToUpper(), out letterVal))
                {
                  runningScore += (int)letterVal;
                }
                typeOffset++;
                xPos = (int)activeTile.x + typeOffset;
              }

              // Prevent from typing outside of grid.
              if (xPos < width)
              {
                tiles[xPos, (int)activeTile.y].ChangeLetter(filteredInput);
                wordTiles.Push(tiles[xPos, (int)activeTile.y]);
                tokensUsed.Push(tokenFromHand);
                currentWord += filteredInput;
                typeOffset++;
              }
            }
          }
        }

        // ######################################## Vertical #########################################
        else if (isTypingVertical)
        {
          if (Input.GetKeyDown(KeyCode.Backspace))
          {
            // Skip over locked tiles that already have letters
            if ((int)activeTile.y - typeOffset < minIndex) typeOffset--;
            int yPos = (int)activeTile.y - typeOffset;
            while (tiles[(int)activeTile.x, yPos].locked && yPos < minIndex)
            {
              typeOffset--;
              yPos = (int)activeTile.y - typeOffset;
            }

            // Only empty unlocked tiles
            if (!tiles[(int)activeTile.x, yPos].locked)
            {
              // Remove score if it was added
              if (runningScore > 0)
              {
                LetterValues letterVal;
                if (Enum.TryParse(tiles[(int)activeTile.x, yPos].GetLetter().ToUpper(), out letterVal))
                {
                  runningScore -= (int)letterVal;
                }
              }

              // Reset tile
              tiles[(int)activeTile.x, yPos].ChangeLetter("");
              wordTiles.Pop();
              User.player.AddToHand(tokensUsed.Pop());
              currentWord = currentWord.Remove(currentWord.Length - 1, 1);
            }
          }
          else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
          {
            isTypingVertical = false;
            // Check if there is a letter afterwards and if there are, add them to word as needed.
            int yPos = (int)activeTile.y - typeOffset;
            while (yPos > -1 && tiles[(int)activeTile.x, yPos].GetLetter() != "")
            {
              currentWord += tiles[(int)activeTile.x, yPos].GetLetter();
              LetterValues letterVal;
              if (Enum.TryParse(tiles[(int)activeTile.x, yPos].GetLetter().ToUpper(), out letterVal))
              {
                runningScore += (int)letterVal;
              }
              typeOffset++;
              yPos = (int)activeTile.y - typeOffset;
            }
            typeOffset = 0;

            // TODO: VALIDATE WORD
            if (ValidateWord()) SubmitWord();
            // TODO: SCORE WORD
          }
          else
          {
            string rawInput = Input.inputString;
            string filteredInput = Regex.Replace(rawInput, "[^A-Za-z0-9]", "").ToUpper();
            Token tokenFromHand;
            if (filteredInput != "" && User.player.RetrieveToken(User.player.GetTokenFromLetter(filteredInput), out tokenFromHand))
            {
              // Skip over tiles with letters in them already
              int yPos = (int)activeTile.y - typeOffset;
              while (yPos > -1 && tiles[(int)activeTile.x, yPos].GetLetter() != "")
              {
                currentWord += tiles[(int)activeTile.x, yPos].GetLetter();
                LetterValues letterVal;
                if (Enum.TryParse(tiles[(int)activeTile.x, yPos].GetLetter().ToUpper(), out letterVal))
                {
                  runningScore += (int)letterVal;
                }
                typeOffset++;
                yPos = (int)activeTile.y - typeOffset;
              }

              // Prevent from typing outside of grid.
              if (yPos > -1)
              {
                tiles[(int)activeTile.x, yPos].ChangeLetter(filteredInput);
                wordTiles.Push(tiles[(int)activeTile.x, yPos]);
                tokensUsed.Push(tokenFromHand);
                currentWord += filteredInput;
                typeOffset++;
              }
            }
          }
        }
      }
    }
  }

  private bool ValidateWord()
  {
    if (!GameDictionary.ValidateWord(currentWord))
    {
      while (wordTiles.Count > 0) wordTiles.Pop().ChangeLetter("");
      while (tokensUsed.Count > 0) User.player.AddToHand(tokensUsed.Pop());
      return false;
    }
    // Check if first word has been placed
    if (!firstWordPlaced)
    {
      // If not, then check if the word to be placed
      // is in the center.
      foreach (TileUnit t in wordTiles)
      {
        if (t.gridPoint == centerOfGrid)
        {
          firstWordPlaced = true;
          break;
        }
      }

      // If it is not in the center, reset the word.
      if (!firstWordPlaced)
      {
        foreach (TileUnit t in wordTiles)
        {
          t.ChangeLetter("");
        }
        while (tokensUsed.Count > 0) User.player.AddToHand(tokensUsed.Pop());
        wordTiles.Clear();
        return false;
      }
    }
    return true;
  }

  private void SubmitWord()
  {
    while (tokensUsed.Count > 0)
    {
      Token t = tokensUsed.Pop();
      runningScore += t.pointValue;
      Destroy(t.gameObject);
    }
    while (wordTiles.Count > 0)
    {
      TileUnit t = wordTiles.Pop();
      runningScore += t.pointValue;
      runningScore = t.PointModifier(runningScore);
      t.ChangeColor(Color.cyan);
      t.LockTyping();
    }
    User.player.DrawToMaxHand();
    User.player.SetScore(User.player.score + runningScore);
    currentWord = "";
    playerManager.ChangeActivePlayer(Computer.player);
  }


  // Description: Begins the typing mechanic on the grid. When
  //              this has been activated, keyboard input will
  //              be registered until the user presses enter.
  // Parameters:  startTile - The "calling" TileUnit.
  //              horizontal - True for horizontal typing (L->R)
  //                           False for vertical (Top->Down)
  public void StartTyping(TileUnit startTile, bool horizontal)
  {
    if (playerManager.IsHumanPlayer())
    {
      if (horizontal)
      {
        isTypingHorizontal = true;
        minIndex = (int)startTile.gridPoint.x;
      }
      else
      {
        isTypingVertical = true;
        minIndex = (int)startTile.gridPoint.y;
      }
      activeTile = startTile.gridPoint;
      runningScore = 0;
    }
  }


  // Description: Adds a full word to the grid starting at a specific
  //              point.
  // Parameters:  startCell - The starting point to write
  //              word - The word to be inserted
  //              horizontal - True for horizontal (L->R)
  //                           False for vertical (Top->Down)
  // Returns:     A bool saying whether or not the word could be written there.
  //              Note that true represents that the word *was* written, not
  //              attempted.
  public bool AddWordToGrid(Vector2 startCell, string word, bool horizontal, Color color, out int score)
  {
    char[] letters = word.ToCharArray();
    score = 0;
    if (horizontal)
    {
      if (width - (int)startCell.x - (letters.Length) <= 0) return false;
      int x = (int)startCell.x;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[x, (int)startCell.y].ChangeLetter(letters[i].ToString());
        tiles[x, (int)startCell.y].ChangeColor(color);
        tiles[x, (int)startCell.y].LockTyping();

        LetterValues letterVal;
        if (Enum.TryParse(letters[i].ToString(), out letterVal)) score += (int)letterVal;
        score += tiles[x, (int)startCell.y].pointValue;
        score = tiles[x, (int)startCell.y].PointModifier(score);
        x++;
      }
    } 
    else
    {
      if ((int)startCell.y - (letters.Length) <= 0) return false;
      int y = (int)startCell.y;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[(int)startCell.x, y].ChangeLetter(letters[i].ToString());
        tiles[(int)startCell.x, y].ChangeColor(color);
        tiles[(int)startCell.x, y].LockTyping();

        LetterValues letterVal;
        if (Enum.TryParse(letters[i].ToString(), out letterVal)) score += (int)letterVal;
        score += tiles[(int)startCell.x, y].pointValue;
        score = tiles[(int)startCell.x, y].PointModifier(score);
        y--;
      }
    }

    return true;
  }

  // Description: Adds a full word to the center of the grid.
  // Parameters:  word - The word to be inserted
  //              horizontal - True for horizontal (L->R)
  //                           False for vertical (Top->Down)
  // Returns:     A bool saying whether or not the word could be written there.
  //              Note that true represents that the word *was* written, not
  //              attempted.
  public bool AddWordToCenterGrid(string word, bool horizontal)
  {
    Vector2 startCell;
    char[] letters = word.ToCharArray();
    if (horizontal)
    {
      startCell = new Vector2(centerOfGrid.x - word.Length/2, centerOfGrid.y);
      if (width - (int)startCell.x - (letters.Length - 1) < 0) return false;
      int x = (int)startCell.x;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[x, (int)startCell.y].ChangeLetter(letters[i].ToString());
        x++;
      }
    }
    else
    {
      startCell = new Vector2(centerOfGrid.x, centerOfGrid.y - word.Length / 2);
      if ((int)startCell.y - (letters.Length - 1) < 0) return false;
      int y = (int)startCell.y;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[(int)startCell.x, y].ChangeLetter(letters[i].ToString());
        y--;
      }
    }

    if (firstWordPlaced == false) firstWordPlaced = true;
    return true;
  }
}
