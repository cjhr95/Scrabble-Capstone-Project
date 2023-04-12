using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TileUnit : MonoBehaviour
{
  [SerializeField] private Color baseColor;                           // The default color for a tile.
  [SerializeField] private new SpriteRenderer renderer;               // Placeholder to access GameObject's renderer
  [SerializeField] private TextMeshProUGUI tileText;                  // Placeholder to access GameObject's letter mesh
  [SerializeField] private TextMeshProUGUI pointText;                 // Placeholder to access GameObject's point mesh
  [SerializeField] public int pointValue { get; private set; }        // The tile's point value
  [SerializeField] private HoverObject hoverObject;                   // Placeholder to access GameObject's Hover mechanic
  public Vector2 position { get; private set; }                       // World position
  public Vector2 gridPoint { get; private set; }                      // Position in tile array (see GridManager)
  public System.Func<int, int> PointModifier { get; private set; }    // Function to be called when scoring this tile if it is special
  public bool locked { get; private set; }                            // Whether or not the tile can be accessed anymore.


  [SerializeField] private Color specialColor;
  [SerializeField] private bool isSpecial;

  // Description: Initializes the TileUnit according to
  //              passed parameters.
  // Parameters:  pos                 - The actual position in-game of the tile
  //              arrPos              - The position in the tiles array in GridManager
  //              pointVal            - The number of points the tile is worth
  //              tileLetter          - The starting letter for the tile.
  //              special             - Whether or not the tile is a special tile.
  //              color               - Color to use when base (white) is not desired.
  //              specialModifierFunc - function called when points are
  //                                     being scored.
  public void Initialize(Vector2 pos, Vector2 arrPos, int pointVal = 0, string tileLetter = "", bool special = false, Color? color = null, System.Func<int, int>? specialModifierFunc = null)
  {
    locked = false;
    baseColor = Color.gray;
    hoverObject = Instantiate(hoverObject);
    hoverObject.Initialize();
    hoverObject.transform.position = gameObject.transform.position;
    hoverObject.name = $"{gameObject.name} - Hover";
    hoverObject.gameObject.SetActive(false);

    position = pos;
    gridPoint = arrPos;

    renderer = GetComponent<SpriteRenderer>();
    isSpecial = special;
    specialColor = color ?? baseColor;
    PointModifier = specialModifierFunc ?? ((int s) => s);
    pointValue = pointVal;
    renderer.color = color ?? baseColor;

    tileText.text = tileLetter;
    pointText.text = pointVal.ToString();
  }

  public void OnMouseDown()
  {
    GridManager grid = (GridManager)FindObjectOfType(typeof(GridManager));
    if (!(grid.isTypingHorizontal || grid.isTypingVertical))
    {
      hoverObject.HoldActivation();
    }
  }

  public void OnMouseUp()
  {
    hoverObject.Deactivate();
    GridManager grid = (GridManager)FindObjectOfType(typeof(GridManager));
    if (hoverObject.isTypingHorizontal || hoverObject.isTypingVertical)
    {
      if (!(grid.isTypingHorizontal || grid.isTypingVertical))
      {
        grid.StartTyping(this, hoverObject.isTypingHorizontal);
        hoverObject.ResetHover();
      }
    }
    hoverObject.gameObject.SetActive(false);
  }

  public void OnMouseEnter()
  {
    hoverObject.gameObject.SetActive(true);
  }

  public void OnMouseExit()
  {
    if (!hoverObject.isActive)
    {
      hoverObject.gameObject.SetActive(false);
    }
  }

  public void ChangeColor(Color c)
  {
    if (!locked) renderer.color = c;
  }

  public void LockTyping()
  {
    locked = true;
  }

  // Description: Changes the letter of the tile
  //              to passed string. Note that it
  //              does not validate that you sent
  //              only a single letter.
  public void ChangeLetter(string letter)
  {
    if (!locked) tileText.text = letter;
  }

  // Description: Retrieves the current letter displayed.
  public string GetLetter()
  {
    return tileText.text;
  }

  // Description: Called before the first frame update
  void Start()
  {
  }

  // Description: Called once per frame
  void Update()
  {
  }
}
