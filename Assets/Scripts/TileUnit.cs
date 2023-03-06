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
  [SerializeField] private Color baseColor;
  [SerializeField] private new SpriteRenderer renderer;
  [SerializeField] private TextMeshProUGUI tileText;
  [SerializeField] private TextMeshProUGUI pointText;
  [SerializeField] public int pointValue { get; private set; }
  [SerializeField] private HoverObject hoverObject;
  public Vector2 position { get; private set; }
  public Vector2 gridPoint { get; private set; }
  public System.Func<int, int> PointModifier { get; private set; }
  public bool locked { get; private set; }


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
    baseColor = Color.white;
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
    renderer.color = c;
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
