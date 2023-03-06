using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField] public int width, height;
  [SerializeField] private TileUnit tilePrefab;
  [SerializeField] private new Transform camera;
  private TileUnit[,] tiles;
  
  void GenerateGrid()
  {
    System.Random rand = new System.Random();
    tiles = new TileUnit[width, height];
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        var createdTile = Instantiate(tilePrefab);
        createdTile.transform.position = new Vector3(x, y);
        createdTile.name = $"Tile ({x},{y})";
        tiles[x,y] = createdTile;

        if (rand.Next(2) == 0)
        {
          createdTile.Initialize(new Vector2(x, y));
        }
        else
        {
          createdTile.Initialize(new Vector2(x, y), 10, special: true, color: Color.green);
        }
      }
    }

    AddWordToGrid(new Vector2(1,3), "bone", false);
  }
  // Start is called before the first frame update
  void Start()
  {
    GenerateGrid();

    camera.transform.position = new Vector3((float)width / 2, (float)height / 2, -15);
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public bool AddWordToGrid(Vector2 startCell, string word, bool horizontal)
  {
    char[] letters = word.ToCharArray();
    if (horizontal)
    {
      if (width - (int)startCell.x - (letters.Length - 1) < 0) return false;
      int x = (int)startCell.x;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[x, (int)startCell.y].ChangeLetter(letters[i]);
        x++;
      }
    } 
    else
    {
      if ((int)startCell.y - (letters.Length - 1) < 0) return false;
      int y = (int)startCell.y;
      for (int i = 0; i < letters.Length; i++)
      {
        tiles[(int)startCell.x, y].ChangeLetter(letters[i]);
        y--;
      }
    }

    return true;
  }
}
