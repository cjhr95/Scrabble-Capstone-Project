using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField] public int width, height;
  [SerializeField] private TileUnit tilePrefab;
  private TileUnit[,] tiles;

  private Vector2 activeTile;
  private int minIndex;
  public bool isTypingHorizontal { get; private set; }
  public bool isTypingVertical { get; private set; }
  private int typeOffset;
  private string currentWord;
  private List<TileUnit> wordTiles;
  
  // Description: Generates a grid based on the width
  //              and height fields. Also fills out
  //              the tiles array for easier reference.
  void GenerateGrid()
  {
    tiles = new TileUnit[width, height];
    wordTiles = new List<TileUnit>();
    isTypingHorizontal = false;
    isTypingVertical = false;
    typeOffset = 0;
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        var createdTile = Instantiate(tilePrefab);
        createdTile.transform.position = new Vector3(x, y);
        createdTile.name = $"Tile ({x},{y})";
        tiles[x,y] = createdTile;


        // TODO: Add logic to determine if tile is special.
        createdTile.Initialize(new Vector2(x, y), new Vector2(x, y));
      }
    }
  }

  // Description: Called before the first frame update
  void Start()
  {
    GenerateGrid();

    Camera.main.transform.position = new Vector3((float)width / 2, (float)height / 2, -10);
    Camera.main.orthographicSize = 9;
  }

  // Description: Called once per frame.
  void Update()
  {

    // ################### Typing Logic ###################
    if (isTypingHorizontal)
    {
      // Code will check if enter/backspace is pressed. If not,
      // then it checks what the user is typing in.
      if (Input.GetKeyDown(KeyCode.Backspace))
      {
        if ((int)activeTile.x + typeOffset > minIndex) typeOffset--;
        int xPos = (int)activeTile.x + typeOffset;
        while (tiles[xPos, (int)activeTile.y].locked && xPos > minIndex)
        {
          typeOffset--;
          xPos = (int)activeTile.x + typeOffset;
        }
        if (!tiles[xPos, (int)activeTile.y].locked)
        {
          tiles[xPos, (int)activeTile.y].ChangeLetter("");
          currentWord = currentWord.Remove(currentWord.Length - 1, 1);
        }
      }
      else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
      {
        isTypingHorizontal = false;
        // Check if there is a letter afterwards.
        int xPos = (int)activeTile.x + typeOffset;
        while (xPos < width && tiles[xPos, (int)activeTile.y].GetLetter() != "")
        {
          currentWord += tiles[xPos, (int)activeTile.y].GetLetter();
          typeOffset++;
          xPos = (int)activeTile.x + typeOffset;
        }
        typeOffset = 0;

        // TODO: VALIDATE WORD
        Debug.Log(currentWord);
        // TODO: SCORE WORD
        int score = 0;
        foreach (TileUnit t in wordTiles)
        {
          score += t.pointValue;
          score = t.PointModifier(score);
          t.LockTyping();
        }
        Debug.Log(score);
        wordTiles.Clear();
        currentWord = "";
      }
      else
      {
        // Input needs to be filtered out before used. Unity/C# will read ALL
        // input, including ones that don't produce a legible character.
        string rawInput = Input.inputString;
        string filteredInput = Regex.Replace(rawInput, "[^A-Za-z]", "");
        if (filteredInput != "")
        {
          // Loop is used to skip over characters to allow users to "add" to a word.
          int xPos = (int)activeTile.x + typeOffset;
          while (xPos < width && tiles[xPos, (int)activeTile.y].GetLetter() != "")
          {
            currentWord += tiles[xPos, (int)activeTile.y].GetLetter();
            typeOffset++;
            xPos = (int)activeTile.x + typeOffset;
          }
          if (xPos < width)
          {
            string input = Input.inputString.Trim()[0].ToString();
            tiles[xPos, (int)activeTile.y].ChangeLetter(input);
            wordTiles.Add(tiles[xPos, (int)activeTile.y]);
            currentWord += input;
            typeOffset++;
          }
        }
      }
    }
    else if (isTypingVertical)
    {
      if (Input.GetKeyDown(KeyCode.Backspace))
      {
        if ((int)activeTile.y - typeOffset < minIndex) typeOffset--;
        int yPos = (int)activeTile.y - typeOffset;
        while (tiles[(int)activeTile.x, yPos].locked && yPos < minIndex)
        {
          typeOffset--;
          yPos = (int)activeTile.y - typeOffset;
        }
        if (!tiles[(int)activeTile.x, yPos].locked)
        {
          tiles[(int)activeTile.x, yPos].ChangeLetter("");
          currentWord = currentWord.Remove(currentWord.Length - 1, 1);
        }
      }
      else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
      {
        isTypingVertical = false;
        // Check if there is a letter afterwards.
        int yPos = (int)activeTile.y - typeOffset;
        while (yPos > -1 && tiles[(int)activeTile.x, yPos].GetLetter() != "")
        {
          currentWord += tiles[(int)activeTile.x, yPos].GetLetter();
          typeOffset++;
          yPos = (int)activeTile.y - typeOffset;
        }
        typeOffset = 0;

        // TODO: VALIDATE WORD
        Debug.Log(currentWord);
        // TODO: SCORE WORD
        int score = 0;
        foreach (TileUnit t in wordTiles)
        {
          score += t.pointValue;
          score = t.PointModifier(score);
          t.LockTyping();
        }
        Debug.Log(score);
        wordTiles.Clear();
        currentWord = "";
      }
      else
      {
        string rawInput = Input.inputString;
        string filteredInput = Regex.Replace(rawInput, "[^A-Za-z0-9]", "");
        if (filteredInput != "")
        {
          int yPos = (int)activeTile.y - typeOffset;
          while (yPos > -1 && tiles[(int)activeTile.x, yPos].GetLetter() != "")
          {
            currentWord += tiles[(int)activeTile.x, yPos].GetLetter();
            typeOffset++;
            yPos = (int)activeTile.y - typeOffset;
          }
          if (yPos > -1)
          {
            string input = Input.inputString.Trim()[0].ToString();
            tiles[(int)activeTile.x, yPos].ChangeLetter(input);
            wordTiles.Add(tiles[(int)activeTile.x, yPos]);
            currentWord += input;
            typeOffset++;
          }
        }
      }
    }
  }


  // Description: Begins the typing mechanic on the grid. When
  //              this has been activated, keyboard input will
  //              be registered until the user presses enter.
  // Parameters:  startTile - The "calling" TileUnit.
  //              horizontal - True for horizontal typing (L->R)
  //                           False for vertical (Top->Down)
  public void StartTyping(TileUnit startTile, bool horizontal)
  {
    if (horizontal)
    {
      isTypingHorizontal = true;
      minIndex = (int) startTile.gridPoint.x;
    }
    else
    {
      isTypingVertical = true;
      minIndex = (int)startTile.gridPoint.y;
    }
    activeTile = startTile.gridPoint;
    
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
  public bool AddWordToGrid(Vector2 startCell, string word, bool horizontal)
  {
    char[] letters = word.ToCharArray();
    if (horizontal)
    {
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
      if ((int)startCell.y - (letters.Length - 1) < 0) return false;
      int y = (int)startCell.y;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[(int)startCell.x, y].ChangeLetter(letters[i].ToString());
        y--;
      }
    }

    return true;
  }
}
